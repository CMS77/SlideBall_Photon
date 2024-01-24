using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI score;
    public TextMeshProUGUI position;

    public void SetPlayerName(string _playerName)
    {
        playerName.text = _playerName;
    }
    public void SetScore(string _score)
    {
        score.text = _score;
    }
    public void SetPosition(string _position)
    {
        position.text = _position;
    }
}
