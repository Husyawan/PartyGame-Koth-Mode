using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 25;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.GetComponent<Health>())
        {
            other.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
        }else if (other.transform.gameObject.GetComponent<PlayerHealth_Koth>())
        {
            other.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
        }
    }
}
