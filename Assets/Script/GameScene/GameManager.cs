using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;


public class GameManager : MonoBehaviourPun, IPunObservable {
    public static int viewId = 0;

    private GameObject player;
    private Text roundText;
    private Transform playerSpawnPoint;
    private UiManager ui;

    private bool isSpawnPlayer;
    private bool isSpawnZombie;
    private bool nextGame;
    private bool gameOver;

    private int getCoin;
    private int zombieCount = 10;
    private int zombieSpawnCount;

    public CinemachineVirtualCamera camSet;
    public List<Transform> zombieSpawnPoint = new List<Transform>();
    public GameObject gameStartButton;

    public bool isPlayerSpawn = true;
    public bool isPlayerDead;
    public bool isShowDebugGUI = false;

    public int Round = 1;
    public int money = 1000;

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
     
        State();
        DeadCam();

    }

    public void RoundTextUpdata()
    {
        roundText.text = "Round : " + Round;
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
    private void DeadCam()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetPhotonView().IsMine == true && players[i].GetComponent<PlayerHP>().isDead == true)
            {
                for (int x = 0; x < players.Length; x++)
                {
                    if (players[x].GetPhotonView().IsMine == false && players[x].GetComponent<PlayerHP>().isDead == false)
                    {
                        camSet.Follow = players[x].transform;
                        camSet.LookAt = players[x].transform;
                    }
                }
            }
        }
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

 

    public void addMoney(int amount) {
        if (PhotonNetwork.IsConnected) {
            money += amount;
        }

        ui.updateMoneyAmount();
    }

    public void removeMoney(int amount) {
        if (PhotonNetwork.IsConnected) {
            money -= amount;
        }

        ui.updateMoneyAmount();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(money);
        }
        else {
            money = (int) stream.ReceiveNext();
        }
    }
}
