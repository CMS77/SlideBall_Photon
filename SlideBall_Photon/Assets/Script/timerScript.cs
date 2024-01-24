using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTXT;
    float elapsedTime;


        void Update()
    {
         elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerTXT.text = formattedTime;
        
    }
}
