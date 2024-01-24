using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class DisConnectServer : MonoBehaviourPunCallbacks
{

    public void DisconnectFromServer()
    {
        PhotonNetwork.Disconnect();
        Debug.LogWarning("Disconnected from Server");
    }

}
