using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Com.MyCompany.SlideBall
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        
        public TextMeshProUGUI buttonText;

         public void OnClickConnect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                // If already connected, go to the lobby directly
                OnConnectedToMaster();
            }
        }
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            SceneManager.LoadScene("Lobby");
        }
    }
}      
