using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
public class RoomButton : MonoBehaviour
{
    public Text nameText;
    public Text sizeText;

    public string roomName;
    public int roomSize;
    public int maxRoomSize;

    public void SetRoom()
    {
        nameText.text = roomName;
        //sizeText.text = roomSize.ToString() + "/" + maxRoomSize.ToString();
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
