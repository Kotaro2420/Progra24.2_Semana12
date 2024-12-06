using GameJolt.API;
using GameJolt.API.Objects;
using Photon.Pun;
using TMPro;
using UnityEngine;

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
                GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
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

        if (bullet != null && bullet.ownerId != photonView.ViewID) // Asegúrate de que no sea tu propio disparo
        {
            if (photonView.IsMine) // Solo el jugador objetivo recibe el daño
            {
                takeDamage();
                // Trofeo por matar a un jugador
                Trophies.Unlock(251913); // Kill a Player Trophy
            }

            bullet.photonView.RPC("DestroyBullet", RpcTarget.AllBuffered); // Destruye la bala en todos los clientes
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            if (photonView.IsMine) // Solo el jugador local incrementa sus monedas
            {
                coins++;
                if (coins == 1)
                {
                    Trophies.Unlock(251910); // A Coin Trophy
                }
                if (coins == 20)
                {
                    Trophies.Unlock(251911); // 20 Coins Trophy
                }
                if (coins == 50)
                {
                    Trophies.Unlock(251912); // 50 Coins Trophy
                }
            }
            Destroy(other.gameObject); // Destruye la moneda
        }
    }

    private void takeDamage()
    {
        hp -= 1;
        Debug.Log($"Player {photonView.ViewID} took damage. HP left: {hp}");

        if (hp <= 0)
        {
            CheckPlayerIsAlive();
        }
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