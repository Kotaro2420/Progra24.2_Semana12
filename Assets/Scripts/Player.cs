using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun.Demo.Asteroids;
using UnityEngine.SocialPlatforms.Impl;
public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;

    private Rigidbody rb;
    [SerializeField] private float speedMove = 1f;
    [SerializeField] private int coins;
    [SerializeField] private int hp;

    public static GameObject LocalInstance { get { return localInstance; } }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject shootPoint;

    private bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
            bulletSpeed = GameData.shootingSpeed;

            playerRenderer.material.color = GameData.playerColor;
        }
        DontDestroyOnLoad(gameObject);

    }

    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = playerName;
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
        Shoot();
        CheckPlayerIsAlive();
        UpdateData();
    }

    void Move()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal * speedMove, rb.velocity.y, vertical * speedMove);

        rb.velocity = movement;

        if (horizontal != 0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }
    }

    void Shoot()
    {
        if (isAlive == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, shootPoint.transform.position, Quaternion.identity);
                obj.GetComponent<Bullet>().SetUp(transform.forward * bulletSpeed, photonView.ViewID);

                obj.GetComponent<Renderer>().material.color = GameData.bulletColor;
            }
        }
    }

    private void CheckPlayerIsAlive()
    {
        if (hp <= 0)
        {
            speedMove = 0;
            isAlive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if (other.gameObject.CompareTag("Bullet"))
        {
            takeDamage();
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            coins++;
            Destroy(other.gameObject);
        }
    }

    private void takeDamage()
    {
        hp -= 1;
    }

    private void UpdateData()
    {
        if (isAlive)
        {
            GameData.playerHp = hp;
            GameData.playerScore = coins;
        }
    }

}