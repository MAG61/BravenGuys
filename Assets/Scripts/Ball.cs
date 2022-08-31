using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(despawn());  
    }

    IEnumerator despawn()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
