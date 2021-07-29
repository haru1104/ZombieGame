using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using Cinemachine;

using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPun, IPunObservable {
    public static int viewId = 0;

    private GameObject player;
    private Text roundText;
    private Transform playerSpawnPoint;
    private UiManager ui;

    /*private bool isSpawnPlayer;
    private bool isSpawnZombie;
    private bool gameOver;

    private int getCoin;
    private int zombieCount = 10;*/

    public CinemachineVirtualCamera camSet;
    public List<Transform> zombieSpawnPoint = new List<Transform>();
    public GameObject gameStartButton;

    public bool isPlayerSpawn = true;
    public bool isPlayerDead = false;
    public bool isShowDebugGUI = false;
    public bool isRestTime = false;

    public int round = 1;
    public int money = 1000;
    public int zombieSpawnCount;

    public float restTime = 1f;

    int deadCount;

    void Start()
    {
        roundText = GameObject.Find("RoundText").GetComponent<Text>();
        ui = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
        ui.GameStartButton(false); 
        SpawnSet();
    }

    void Update()
    {
        CheckRemainZombies();
        onGameOver();
        DeadCam();
    }

    public void RoundTextUpdate()
    {
        roundText.text = "Round : " + round;
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
            if (players[i].GetPhotonView().IsMine == true && players[i].GetComponent<PlayerHP>().isDead == false)
            {
                camSet.Follow = players[i].transform;
                camSet.LookAt = players[i].transform;
                players[i].GetComponent<Animator>().SetBool("Dead", false);
            }
        }
    }

    private void CheckRemainZombies() {
        if (PhotonNetwork.IsMasterClient && ui.isGameStart && !isRestTime && zombieSpawnCount <= 0) {
            Debug.LogWarning("쉬는시간 시작!");

            if (!isRestTime) {
                isRestTime = true;
                StartCoroutine("RestTime");
            }
        }
    }

    IEnumerator RestTime() {
        Debug.LogWarning(restTime + "초간 휴식을 취합니다.");

        yield return new WaitForSecondsRealtime(restTime);
        isRestTime = false;

        round++;
        Debug.LogWarning("현재 라운드 수 : " + round);

        EnemySpawn();
    }

    public void EnemySpawn()
    {
        if (PhotonNetwork.IsMasterClient == false && PhotonNetwork.IsConnected == false)
        {
            return;
        }
        if (!isRestTime && PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("쉬는시간 종료!");

            zombieSpawnCount = 1; // zombieCount * Round;
            StartCoroutine("Enemy_Spawn");
        }
    }

    private IEnumerator Enemy_Spawn()
    {
        Debug.Log("Start coroutine");

        for (int i = 0; i < zombieSpawnCount; i++)
        {
            yield return new WaitForSeconds(0.8f);

            int temp = Random.Range(1, 6);
            int transTemp = Random.Range(0, 4);

            ZombieSpawn(temp, transTemp);
        }

        // isSpawnZombie = true;
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

    private void onGameOver() {
        int _deadCount = 0;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            if (players[i].GetComponent<PlayerHP>().isDead) {
                _deadCount++;

                isPlayerDead = true;
            }
        }

        if (_deadCount == players.Length && isPlayerDead) {
            isPlayerDead = false;

            // 게임오버 화면 띄우기
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
            stream.SendNext(round);
            stream.SendNext(money);
            stream.SendNext(deadCount);

            stream.SendNext(isRestTime);
        }
        else {
            round = (int) stream.ReceiveNext();
            money = (int) stream.ReceiveNext();
            deadCount = (int) stream.ReceiveNext();

            isRestTime = (bool) stream.ReceiveNext();
        }
    }
}
