using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    void Start()
    {
        GameManager.instance.SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
