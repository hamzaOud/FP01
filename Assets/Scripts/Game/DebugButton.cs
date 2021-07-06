using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DebugButton : MonoBehaviour
{

    public Pokemon pikachu;

    public void OnClickDebugButton()
    {

        for(int i= 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            print("trainer " + i + " has " + GameController.Instance.trainers[i].spawnPoints.Length + " spawnpoints");
        }
    }
}
