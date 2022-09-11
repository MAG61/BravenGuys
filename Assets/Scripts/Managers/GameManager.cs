using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Phase { Start, SemiFinal, Final }
    public Phase phase;

    public CharacterController Player;
    public List<Player> bots = new();
    public MapManager currentMap;
    private List<Player> qualifieds = new();

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
        DontDestroyOnLoad(this);
    }
    void Start()
    {

    }

    void Update()
    {
        if (Player == null) return;
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "RandomMap")
        {
            foreach (Player bot in bots)
            {
                bot.gameObject.SetActive(false);
            }
            Player.gameObject.SetActive(false);
        }
        else
        {
            foreach (Player bot in bots)
            {
                bot.gameObject.SetActive(true);
            }
            Player.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        phase = Phase.Start;

        for (int i = 0; i < 15; i++)
        {
            GameObject bot = Instantiate(Resources.Load("Prefabs/AI") as GameObject);
            bots.Add(bot.GetComponent<Player>());
        }

        GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject);
        Player = player.GetComponent<CharacterController>();

        SelectRandomMap();
    }

    public void SpawnPlayers()
    {
        GetCurrentMap();

        foreach (Player bot in bots)
        {
            bot.gameObject.SetActive(true);
            bot.GetComponent<AIController>().FindDests();
        }
        Player.gameObject.SetActive(true);

        Player.transform.position = currentMap.spawnPoints[0].position;


        GameObject playerCamera = Instantiate(Resources.Load("Prefabs/PlayerCamera") as GameObject);
        playerCamera.transform.position = Player.transform.position + playerCamera.GetComponent<CameraController>().offset;
        playerCamera.GetComponent<CameraController>().target = Player.transform;

        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].transform.position = currentMap.spawnPoints[i + 1].position;
        }

        Player.FindRequirements();
    }

    public void GetCurrentMap()
    {
        currentMap = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    public void RemoveBot(Player bot) { if (bots.Contains(bot)) bots.Remove(bot); }

    public void SelectRandomMap() { SceneManager.LoadScene("RandomMap"); }

    public void SetMap(string map) { SceneManager.LoadScene(map); }

    public void StartWithDelay(float delay) { StartCoroutine(StartWithDelayy(delay)); }
    IEnumerator StartWithDelayy(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }

    public void MapEnd(List<Player> qualifieds)
    {
        this.qualifieds = qualifieds;
        SceneManager.LoadScene("Elimination");
    }

    public void EliminateRquirements()
    {
        foreach (Player bot in bots)
        {
            bot.gameObject.SetActive(true);
            bot.GetComponent<AIController>().enabled = false;
            bot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bot.transform.position = Vector3.zero;
            bot.transform.LookAt(new Vector3(0, -5, -17));
            bot.GetComponent<AIController>().nameText.transform.LookAt(new Vector3(0, 40, 17));
        }
        Player.gameObject.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.transform.position = Vector3.zero;
        Player.transform.LookAt(new Vector3(0, -5, -17));

        GameObject.Find("EliminationManager").GetComponent<EliminationManager>().Eliminate(bots, qualifieds, Player);
    }

    public void Eliminate(List<Player> qualifieds)
    {
        if (phase != Phase.Final)
        {
            if (qualifieds.Contains(Player.GetComponent<Player>()))
            {
                qualifieds.Remove(Player.GetComponent<Player>());
            }
            else
            {
                GameEnd();
            }
            foreach (Player bot in bots)
            {
                if (!qualifieds.Contains(bot))
                {
                    Destroy(bot.gameObject);
                }
            }

            bots = qualifieds;

            foreach (Player bot in bots)
            {
                bot.GetComponent<AIController>().enabled = true;
                bot.gameObject.SetActive(false);
            }
            Player.GetComponent<CharacterController>().enabled = true;
            Player.gameObject.SetActive(false);

            if (phase == Phase.Start) phase = Phase.SemiFinal;
            else if (phase == Phase.SemiFinal) phase = Phase.Final;

            SelectRandomMap();
        }


    }

    public void GameEnd()
    {

    }
}
