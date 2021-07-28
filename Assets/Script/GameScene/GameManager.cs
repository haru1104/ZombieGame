using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;


public class GameManager : MonoBehaviourPun
{
    public CinemachineVirtualCamera camSet;

    private GameObject player;
    private Text roundText;
    private Transform playerSpawnPoint;
    private UiManager ui;

    public List<Transform> zombieSpawnPoint = new List<Transform>();
    public GameObject gameStartButton;
    private int getCoin;
    private int zombieCount = 10;
    private int zombieSpawnCount;
    public int Round = 1;
    private bool isSpawnPlayer;
    private bool isSpawnZombie;
    public bool isPlayerSpawn = true;
    private bool nextGame;
    private bool gameOver;
    public bool isPlayerDead;
    public bool isShowDebugGUI = false;
    public static int viewId=0;

    //개발자 전용 무기 하나 제작 

    void Start()
    {
        roundText = GameObject.Find("RoundText").GetComponent<Text>();
        ui = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
        ui.GameStartButton(false); 
        SpawnSet();
    }

    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) {
            if (PhotonNetwork.IsMasterClient == true)
            {
                ui.GameStartButton(true);
            }
            RoundTextUpdata();
        }
        State();
    }
    private void RoundTextUpdata()
    {
        roundText.text = "Round : " + Round;
    }
    public void OnClickStartButton()
    {
        if (ui.isGameStart == false)
        {
            ui.isGameStart = true;
        }
    }
    private void SpawnSet()
    {
        playerSpawnPoint = GameObject.Find("PlayerSpawnPosition").GetComponent<Transform>();
        player = PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);
        viewId = player.GetPhotonView().ViewID;
        camSet.Follow = player.transform;
        camSet.LookAt = player.transform;
        isPlayerSpawn = true;
        ui.AttackButton();
    }
    private void State()
    {

    }
    private void EnemySpawn()
    {
        if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.IsConnected == false)
        {
            return;
        }
        if (nextGame == true)
        {
            zombieSpawnCount = zombieCount * Round;
            StartCoroutine("Enemy_Spawn");
        }
    }
    private IEnumerator Enemy_Spawn()
    {
        for (int i = 0; i >= zombieSpawnCount; i++)
        {
            yield return new WaitForSeconds(0.8f);
            int temp = Random.Range(1, 6);
            int transTemp = Random.Range(1, 5);
            ZombieSpawn(temp, transTemp);
        }
        isSpawnZombie = true;
    }
    private void ZombieSpawn(int spawnNum, int transTemp)
    {
        if (spawnNum >= 1 && spawnNum <= 3)
        {
            PhotonNetwork.Instantiate("Normal Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
        }
        else if (spawnNum == 4)
        {
            PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
        }
        else if (spawnNum == 5)
        {
            PhotonNetwork.Instantiate("Heavy Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
        }
    }
}
