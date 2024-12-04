using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, int ownerId)
    {
        this.direction = direction;
        this.ownerId = ownerId;
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        rb.velocity = direction.normalized * speed;
    }
}