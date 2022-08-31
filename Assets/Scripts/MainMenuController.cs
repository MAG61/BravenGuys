using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private int players = 1;
    public TextMeshProUGUI text;

    public void PlayAnimation(string animation)
    {
        GetComponent<Animation>().Play(animation);
    }

    public void StartGame()
    {
        GetComponent<Animation>().Play("Main2Game");
        GameObject.Find("farmer").GetComponent<Animator>().Play("startgame");
        Countdown();
    }

    private void Update()
    {
        text.text = players.ToString() + "/16";

        if (players >= 16)
        {
            StartCoroutine(startGame());
        }
    }

    private void Countdown()
    {
        if (players < 16)
        {
            StartCoroutine(randomSec());
        }
    }

    IEnumerator randomSec()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        players++;
        Countdown();
    }

    IEnumerator starting()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
