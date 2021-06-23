using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Tester : MonoBehaviour
{
    public GameObject capsule;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDebugButton()
    {
        print("number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

    }

    public void OnClickChangeColor()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("ChangeColor", RpcTarget.All);
    }

    [PunRPC]
    public void ChangeColor()
    {
        capsule.GetComponent<Renderer>().material.color = Color.red;
    }
}
