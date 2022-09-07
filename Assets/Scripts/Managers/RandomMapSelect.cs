using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RandomMapSelect : MonoBehaviour
{

    public string[] scenes;
    public string selectedScene;

    void Start()
    {
        selectedScene = scenes[Random.Range(0, scenes.Length)];
        GameObject.Find("SceneName").GetComponent<TextMeshProUGUI>().text = selectedScene;
        StartCoroutine(goMap());
    }

    IEnumerator goMap()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.SetMap(selectedScene);
    }
}
