using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerScreenManager : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI tapText;
    public Transform playerSlot;
    public Player winner;
    void Start()
    {
        GameManager.instance.WinnerRequirements();
    }

    private void Update()
    {
        if (tapText.gameObject.activeSelf && Input.touchCount > 0)
        {
            GameManager.instance.GoMainMenu();
        }
    }

    public void Winner(Player winner)
    {
        this.winner = winner;
        if (winner.TryGetComponent(out AIController AI))
        {
            winnerText.text = winner.GetComponent<AIController>().userName;
            winner.GetComponent<AIController>().nameText.gameObject.SetActive(false);
        }


        winner.transform.SetParent(playerSlot);
        winner.transform.localPosition = Vector3.zero;
        winner.GetComponent<Animator>().SetBool("dance", true);
        GetComponent<Animation>().Play("WinnerRoll");

        StartCoroutine(TapText());
    }

    IEnumerator TapText()
    {
        yield return new WaitForSeconds(4f);
        tapText.gameObject.SetActive(true);
    }
}
