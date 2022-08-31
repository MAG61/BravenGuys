using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public bool hasObstacle;

    public GameObject obstacle;
    private int obstacleType;


    void Start()
    {
        //obstacleType = obstacle.GetComponent<Obstacle>().obstacleType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
