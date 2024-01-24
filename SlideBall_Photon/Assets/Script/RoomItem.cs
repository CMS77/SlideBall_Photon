using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI RoomName;
    LobbyManager manager;
    public string test;

    private void Start ()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        RoomName.text = _roomName;
        test = _roomName;
    }
    public void OnClickItem()
    {
        manager.JoinRoom(RoomName.text);
    }
}