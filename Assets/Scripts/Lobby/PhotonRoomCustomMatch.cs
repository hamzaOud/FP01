using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoomCustomMatch : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomCustomMatch room;
    private PhotonView PV;

    public bool isGameloaded;
    public int currentScene;

    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;

    private void Awake()
    {
        if(PhotonRoomCustomMatch.room == null)
        {
            PhotonRoomCustomMatch.room = this;
        }
        else
        {
            if (PhotonRoomCustomMatch.room != this)
            {
                Destroy(PhotonRoomCustomMatch.room.gameObject);
                PhotonRoomCustomMatch.room = this;
            }
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        DontDestroyOnLoad(this.gameObject);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                startButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
        playersInRoom--;

        ClearPlayerListings();
        ListPlayers();
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            if (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                //startButton.GetComponent<Button>().interactable = false;
            }
        }
        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;

    }

    public void StartGame()
    {
        print(PhotonNetwork.PlayerList.Length);
        isGameloaded = true;
        PhotonNetwork.LoadLevel(2);

    }

    void ClearPlayerListings()
    {
        for(int i = playersPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }
}
