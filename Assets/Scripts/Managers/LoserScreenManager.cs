using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoserScreenManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(tapText());   
    }

    void Update()
    {
        if (GameObject.Find("TapText").activeSelf && Input.touchCount > 0)
        {
            GameManager.instance.GoMainMenu();
        }
    }

    IEnumerator tapText()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("TapText").SetActive(true);
    }
}
