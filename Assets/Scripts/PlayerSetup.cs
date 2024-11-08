using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Photon;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public PlayerMovement movement;
    public GameObject camera;
    public TextMeshPro nicknameText;
    public Attach player;

    public void IsLocalPlayer()
    {
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nicknameText.text = _name;
    }
}