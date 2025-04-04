using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoserScreenManager : MonoBehaviour
{
    public GameObject taptext;

    void Start()
    {
        StartCoroutine(tapText());   
    }

    void Update()
    {
        if (taptext.activeSelf && Input.touchCount > 0)
        {
            GameManager.instance.GoMainMenu();
        }
    }

    IEnumerator tapText()
    {
        yield return new WaitForSeconds(3f);
        taptext.SetActive(true);
    }
}
