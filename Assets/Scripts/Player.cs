using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun.Demo.Asteroids;
public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;

    private Rigidbody rb;
    [SerializeField] private float speedMove;

    public static GameObject LocalInstance { get { return localInstance; } }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
            bulletSpeed = GameData.shootingSpeed;

        }
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
            obj.GetComponent<Bullet>().SetUp(transform.forward * bulletSpeed, photonView.ViewID);
        }
    }

}