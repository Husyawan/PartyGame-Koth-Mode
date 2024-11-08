using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class PlayerHealth_Koth : MonoBehaviour
{
    public bool isLocalInstance;
    public int health = 100;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            if (isLocalInstance&&gameObject.tag== "RedPlayer")
            {
                RoomManager_KothMode.Instance.Respawn_Red();
            }else if(isLocalInstance && gameObject.tag == "BluePlayer")
            {
                RoomManager_KothMode.Instance.Respawn_Blue();
            }
                Destroy(gameObject);
        }
    }
}
