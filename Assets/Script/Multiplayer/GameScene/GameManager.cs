using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

    private bool isSpawnPlayer;
    private bool gameOver;

    private int getCoin;
    private int zombieCount = 10;

    public CinemachineVirtualCamera camSet;
    public List<Transform> zombieSpawnPoint = new List<Transform>();
    public GameObject gameStartButton;
    public GameObject timer;

    public bool isPlayerSpawn = true;
    public bool isPlayerDead = false;
    public bool isShowDebugGUI = false;
    public bool isRestTime = false;

    public int round = 1;
    public int money = 1000;
    public int zombieSpawnCount;

    public float restTime = 1f;

    int deadCount;

    void Start() {
        roundText = GameObject.Find("RoundText").GetComponent<Text>();
        ui = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
        ui.GameStartButton(false);
        SpawnSet();
    }

    void Update() {
        if (!ui.isGameStart) {
            timer.SetActive(false);
        }

        CheckRemainZombies();
        onGameOver();
        DeadCam();
    }

    public void RoundTextUpdate() {
        roundText.text = "Round : " + round;
    }

    private void SpawnSet() {
        playerSpawnPoint = GameObject.Find("PlayerSpawnPosition").GetComponent<Transform>();
        player = PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);
        viewId = player.GetPhotonView().ViewID;
        camSet.Follow = player.transform;
        camSet.LookAt = player.transform;
        isPlayerSpawn = true;
        ui.AttackButton();
    }
    private void DeadCam() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            if (players[i].GetPhotonView().IsMine == true && players[i].GetComponent<PlayerHP>().isDead == true) {
                for (int x = 0; x < players.Length; x++) {
                    if (players[x].GetPhotonView().IsMine == false && players[x].GetComponent<PlayerHP>().isDead == false) {
                        camSet.Follow = players[x].transform;
                        camSet.LookAt = players[x].transform;
                    }
                }
            }
            if (players[i].GetPhotonView().IsMine == true && players[i].GetComponent<PlayerHP>().isDead == false) {
                camSet.Follow = players[i].transform;
                camSet.LookAt = players[i].transform;
                players[i].GetComponent<Animator>().SetBool("Dead", false);
            }
        }
    }

    private void CheckRemainZombies() {
        if (ui.isGameStart && !isRestTime && zombieSpawnCount <= 0) {
            killAllZombies();

            timer.SetActive(true);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogWarning("쉬는시간 시작!");

                if (!isRestTime) {
                    isRestTime = true;
                    StartCoroutine("RestTime");
                }
            }
        }
    }

    private void killAllZombies() {
        GameObject[] remainZombies = GameObject.FindGameObjectsWithTag("Zombie");

        for (int i = 0; i < remainZombies.Length; i++) {

        }

        int length = remainZombies.Length;

        if (length == 0) {
            Debug.Log("더 이상 죽일 좀비 개체가 존재하지 않습니다.");
        }
        else {
            for (int i = 0; i < length; i++) {
                
            }

            zombieSpawnCount = 0;
            Debug.LogError("클라이언트 측에만 남아있던 좀비 " + length + "마리를 삭제하였습니다.");
        }
    }

    IEnumerator RestTime() {
        Debug.LogWarning(restTime + "초간 휴식을 취합니다.");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<PlayerHP>().NextRound();
        }
        yield return new WaitForSecondsRealtime(restTime);
        isRestTime = false;

        round++;
        Debug.LogWarning("현재 라운드 수 : " + round);

        EnemySpawn();
    }

    public void EnemySpawn() {
        if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.IsConnected == false) {
            return;
        }

        if (!isRestTime) {
            timer.SetActive(false);

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogWarning("쉬는시간 종료!");

                zombieSpawnCount = zombieCount * round;
                StartCoroutine("Enemy_Spawn");
            }
        }
    }

    private IEnumerator Enemy_Spawn() {
        for (int i = 0; i < zombieSpawnCount; i++) {
            yield return new WaitForSeconds(0.8f);

            int temp = Random.Range(1, 6);
            int transTemp = Random.Range(0, 4);

            ZombieSpawn(temp, transTemp);
        }
    }

    private void ZombieSpawn(int spawnNum, int transTemp) {
        if (spawnNum >= 1 && spawnNum <= 3) {
            PhotonNetwork.Instantiate("Normal Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
        }
        else if (spawnNum == 4) {
            PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
        }
        else if (spawnNum == 5) {
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

            ui.Gameover.SetActive(true);
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
            stream.SendNext(zombieSpawnCount);

            stream.SendNext(isRestTime);
        }
        else {
            round = (int) stream.ReceiveNext();
            money = (int) stream.ReceiveNext();
            deadCount = (int) stream.ReceiveNext();
            zombieSpawnCount = (int) stream.ReceiveNext();

            isRestTime = (bool) stream.ReceiveNext();
        }
    }
}