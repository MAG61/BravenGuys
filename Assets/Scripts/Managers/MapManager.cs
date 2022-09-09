using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public List<Player> qualifieds = new();
    public short qualifyAmount;
    public GameManager.Phase phase;

    private Animator animator;
    private TextMeshProUGUI qualifyText;
    void Start()
    {
        animator = GameObject.Find("QualifyText").GetComponent<Animator>();

        phase = GameManager.instance.phase;
        if (phase == GameManager.Phase.Start) qualifyAmount = 8;
        else if (phase == GameManager.Phase.SemiFinal) qualifyAmount = 4;
        else if (phase == GameManager.Phase.Final) qualifyAmount = 1;

        qualifyText = GameObject.Find("QualifyText").GetComponent<TextMeshProUGUI>();
        GameManager.instance.SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        qualifyText.text = "<color=blue> Qualifieds: </color><color=red>" + qualifieds.Count.ToString() + " / " + qualifyAmount + "</color>"; 
    }

    public void Qualify(Player player)
    {
        if (qualifieds.Count < qualifyAmount && !qualifieds.Contains(player))
        {
            qualifieds.Add(player);
            animator.SetTrigger("qualify");
            if (qualifieds.Count == qualifyAmount)
            {
                GameManager.instance.MapEnd(qualifieds);
            }
        }
    }

}
