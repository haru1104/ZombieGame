using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;

namespace haruroad.szd.multiplayer {
    public class GameManager : MonoBehaviourPun, IPunObservable {
        public static int viewId = 0;

        private GameObject player;
        private Text roundText;
        private Transform playerSpawnPoint;
        private UiManager ui;

        private GameObject[] zombies;

        private bool isSpawnPlayer;
        private bool gameOver;

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

            if (!isRestTime) {
                timer.SetActive(false);
            }

            CheckRemainZombies();
            onGameOver();
            DeadCam();
        }

        public void RoundTextUpdate() {
            roundText.text = "Round : " + round;
        }

        public int getLiveZombieCount() {
            int count = 0;

            if (zombies.Length != 0) {
                for (int i = 0; i < zombies.Length; i++) {
                    if (!zombies[i].GetComponent<Zombie>().isDead) {
                        count++;
                    }
                }
            }

            return count;
        }

        private int getSpawnRemainCount() {
            int count = 0;

            if (zombies.Length != 0) {
                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] != null) {
                        count++;
                    }
                }

                return zombies.Length - count;
            }

            return count;
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
            if (ui.isGameStart && !isRestTime && getLiveZombieCount() <= 0) {
                killAllZombies();

                timer.SetActive(true);
                Debug.LogWarning("쉬는 시간 시작!");

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                for (int i = 0; i < players.Length; i++) {
                    players[i].GetComponent<PlayerHP>().NextRound();
                }

                if (PhotonNetwork.IsMasterClient) {
                    if (!isRestTime) {
                        isRestTime = true;
                        StartCoroutine("RestTime");
                    }
                }
            }
        }

        private void killAllZombies() {
            List<GameObject> dyingZombies = new List<GameObject>();
            GameObject[] livedZombies = GameObject.FindGameObjectsWithTag("Zombie");

            for (int i = 0; i < livedZombies.Length; i++) {
                if (!livedZombies[i].GetComponent<Zombie>().isDead) {
                    dyingZombies.Add(livedZombies[i]);
                }
            }

            if (dyingZombies.Count <= 0) {
                return;
            }
            else {
                int count = 0;

                if (dyingZombies.Count == count) {
                    return;
                }
                else {
                    for (int i = 0; i < dyingZombies.Count; i++) {
                        GameObject temp = dyingZombies[i];
                        dyingZombies.RemoveAt(i);

                        Destroy(temp);
                    }
                }
            }
        }

        IEnumerator RestTime() {
            Debug.LogWarning(restTime + "초간 휴식을 취합니다.");

            yield return new WaitForSecondsRealtime(restTime);

            round++;
            Debug.LogWarning("현재 라운드 수 : " + round);

            EnemySpawn();
        }

        public void EnemySpawn() {
            isRestTime = false;

            if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.IsConnected == false) {
                return;
            }

            if (!isRestTime) {
                ui.isShopDown = false;
                timer.SetActive(false);

                if (PhotonNetwork.IsMasterClient) {
                    zombies = new GameObject[10 * round];
                    StartCoroutine("Enemy_Spawn");
                }
            }
        }

        private IEnumerator Enemy_Spawn() {
            for (int i = 0; i < zombies.Length; i++) {
                yield return new WaitForSeconds(0.8f);

                int temp = Random.Range(1, 6);
                int transTemp = Random.Range(0, 4);

                ZombieSpawn(temp, transTemp);
            }
        }

        private void ZombieSpawn(int spawnNum, int transTemp) {
            if (spawnNum >= 1 && spawnNum <= 3) {
                GameObject go = PhotonNetwork.Instantiate("Normal Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
                zombies[getSpawnRemainCount()] = go;
            }
            else if (spawnNum == 4) {
                GameObject go = PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
                zombies[getSpawnRemainCount()] = go;
            }
            else if (spawnNum == 5) {
                GameObject go =PhotonNetwork.Instantiate("Heavy Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
                zombies[getSpawnRemainCount()] = go;
            }
        }

        private void onGameOver() {
            int _deadCount = 0;
            isPlayerDead = false;

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

}