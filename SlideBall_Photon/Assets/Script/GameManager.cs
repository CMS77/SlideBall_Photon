using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText; 
    public TextMeshProUGUI player2ScoreText;
    private int player1Score = 0;
    private int player2Score = 0;

    private string playerScoreUrl = "http://localhost:8081/api/users/{0}/score/";

    public GameObject ball;
    public GameObject player1;
    public GameObject player2;
    public TextMeshProUGUI timerTXT;
     
    public static string playerId;
    public bool scoreRegistered;

    private Vector2 spawn1 = new Vector2(-7f, 0.32f);
    private Vector2 spawn2 = new Vector2(7.5f, 0.32f);
    private Vector2 spawnBall = new Vector2(0.32f, 0f);

    private GameObject playerObject;
    private GameObject ballObject;

    public static void setPlayerId(string Id)
    {
        playerId = Id;
    }


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerObject = PhotonNetwork.Instantiate(player1.name, spawn1, Quaternion.identity);
            ballObject = PhotonNetwork.Instantiate(ball.name, spawnBall, Quaternion.identity);
        }
        else
        {
            playerObject = PhotonNetwork.Instantiate(player2.name, spawn2, Quaternion.identity);
        }

    }

    private void Update()
    {
        if ((timerTXT.text == "03:00" || timerTXT.text == "03:00")&& !scoreRegistered)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(registerPoints(playerId, player2Score));
            } else
            {
                StartCoroutine(registerPoints(playerId, player1Score));
            }
            scoreRegistered = true;
            SceneManager.LoadScene("ConnectToServer");
        }
    }

    private IEnumerator registerPoints(string playerId, int playerPoints)
    {
        string jsonPayload = $"{{\"score\":{playerPoints}}}";

        using (UnityWebRequest www = UnityWebRequest.Put(String.Format(playerScoreUrl, playerId), jsonPayload))
        {
            www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

            yield return www.SendWebRequest();
        }
    }


    public void GoalScored(int scoringPlayer)
    {
        if (scoringPlayer == 1)
        {
            player1Score++;
        }
        else if (scoringPlayer == 2)
        {
            player2Score++;
        }

        Debug.Log("Player 1: " + player1Score + " - Player 2: " + player2Score);

        UpdateScoreText();
        // Reset ball and players to spawn positions
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(ballObject);
            PhotonNetwork.Destroy(playerObject);
            playerObject = PhotonNetwork.Instantiate(player1.name, spawn1, Quaternion.identity);
            ballObject = PhotonNetwork.Instantiate(ball.name, spawnBall, Quaternion.identity);
            
        }
        else
        {
            PhotonNetwork.Destroy(playerObject);
            playerObject = PhotonNetwork.Instantiate(player2.name, spawn2, Quaternion.identity);
        }

    }

    void ResetGame()
    {
        Debug.Log("Resetting game...");
        player1.transform.position = spawn1;
        player2.transform.position = spawn2;
        ball.transform.position = spawnBall;
    }


    

    void UpdateScoreText()
    {
        // Update the UI Text components with the current scores
        player1ScoreText.text = ""+ player1Score;
        player2ScoreText.text = ""+ player2Score;
    }

}
