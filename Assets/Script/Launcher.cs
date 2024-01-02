using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Com.MyCompany.SlideBall
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public InputField usernameInput;
        public Text buttonText;

        public void OnClickConnect()
        {
            if (usernameInput.text.Length >= 1) 
            {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
            }
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
        }
    }
}      
