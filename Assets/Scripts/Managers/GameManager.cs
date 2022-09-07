using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CharacterController Player;
    public List<AI> bots = new();
    public MapManager currentMap;

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
            foreach (AI bot in bots)
            {
                bot.gameObject.SetActive(false);
            }
            Player.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject bot = Instantiate(Resources.Load("Prefabs/AI") as GameObject);
            bots.Add(bot.GetComponent<AI>());
        }

        GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject);
        Player = player.GetComponent<CharacterController>();

        SelectRandomMap();
    }

    public void SpawnPlayers()
    {
        GetCurrentMap();

        foreach (AI bot in bots)
        {
            bot.gameObject.SetActive(true);
        }
        Player.gameObject.SetActive(true);

        Player.transform.position = currentMap.spawnPoints[0].position;
        Player.FindRequirements();

        GameObject playerCamera = Instantiate(Resources.Load("Prefabs/PlayerCamera") as GameObject);
        playerCamera.transform.position = Player.transform.position + playerCamera.GetComponent<CameraController>().offset;
        playerCamera.GetComponent<CameraController>().target = Player.transform;

        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].transform.position = currentMap.spawnPoints[i + 1].position;
            bots[i].GetComponent<AIController>().FindDests();
        }
    }

    public void GetCurrentMap()
    {
        currentMap = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    public void RemoveBot(AI bot) { bots.Remove(bot); }

    public void SelectRandomMap() { SceneManager.LoadScene("RandomMap"); }

    public void SetMap(string map) { SceneManager.LoadScene(map); }

    public void StartWithDelay(float delay) { StartCoroutine(StartWithDelayy(delay)); }
    IEnumerator StartWithDelayy(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }
}
