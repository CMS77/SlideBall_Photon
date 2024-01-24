using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_GameController : MonoBehaviour
{

    public static AU_GameController instance;

    public Transform[] spawnPoints;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
