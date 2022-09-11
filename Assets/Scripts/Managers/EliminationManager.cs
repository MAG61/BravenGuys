using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EliminationManager : MonoBehaviour
{
    public GameObject[] platforms;
    private short platformAmount;
    private List<Player> eliminateds = new();
    public TextMeshProUGUI text;

    public GameManager.Phase phase;

    void Start()
    {
        phase = GameManager.instance.phase;

        if (phase == GameManager.Phase.Start) platformAmount = 16;
        else if (phase == GameManager.Phase.SemiFinal) platformAmount = 8;
        else if (phase == GameManager.Phase.Final) platformAmount = 4;

        text.text = platformAmount.ToString();

        for (int i = 0; i < platformAmount; i++)
        {
            platforms[i].SetActive(true);
        }

        GameManager.instance.EliminateRquirements();
    }

    public void Eliminate(List<Player> allPlayers, List<Player> qualifieds, CharacterController player)
    {
        allPlayers.Add(player.GetComponent<Player>());
        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].transform.position = platforms[i].transform.position + new Vector3(0, 0.5f, 0);
        }

        StartCoroutine(Eliminatee(allPlayers, qualifieds, player));
    }

    IEnumerator Eliminatee(List<Player> allPlayers, List<Player> qualifieds, CharacterController player)
    {
        yield return new WaitForSecondsRealtime(5f);

        foreach (Player bot in allPlayers)
        {
            if (!qualifieds.Contains(bot))
            {
                eliminateds.Add(bot);
                platforms[allPlayers.IndexOf(bot)].AddComponent<Rigidbody>();
                platforms[allPlayers.IndexOf(bot)].GetComponent<Rigidbody>().angularDrag = 0.3f;
            }
        }

        allPlayers.Remove(player.GetComponent<Player>());
        text.text = qualifieds.Count.ToString();
        StartCoroutine(Next(qualifieds));
    }

    IEnumerator Next(List<Player> qualifieds)
    {
        yield return new WaitForSecondsRealtime(5f);
        GameManager.instance.Eliminate(qualifieds);
    }

    void Update()
    {

    }
}
