using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField RoomInputField;
    public GameObject LobbyPanel;
    public GameObject RoomPanel;
    public TextMeshProUGUI roomName;
    public Button startButton;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float TimeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); 
        }
        
        
    }
    public void PlayGame()
    {
        //PhotonNetwork.CurrentRoom.Players;
        PhotonNetwork.LoadLevel("Game");

    }

    public void ReturnScene()
    {
        SceneManager.LoadScene("ConnectToServer");
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            startButton.gameObject.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // after connecting to the Master Server
    }

    public void OnClickCreate()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (RoomInputField.text.Length >= 1)
            {
                PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions() { MaxPlayers = 2 });
            }
        }
        else
        {
            Debug.LogError("Not connected to the Photon Server yet. Wait for OnConnectedToMaster callback.");
        }
    }

    public override void OnJoinedRoom()
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
        }
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdadeRoomList(roomList);
            nextUpdateTime = Time.time + TimeBetweenUpdates;
        }
    
    }

    void UpdadeRoomList (List<RoomInfo> list)
    {
        roomItemsList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            RoomInfo info = list[i];
            if (!info.RemovedFromList)
            {
                RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                newRoom.SetRoomName(info.Name);
                roomItemsList.Add(newRoom);
            }
        }
    }
    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
    public void OnClickLeaveRoom ()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    } 
}
