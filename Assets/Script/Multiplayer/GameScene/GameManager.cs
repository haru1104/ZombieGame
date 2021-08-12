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

        public CinemachineVirtualCamera camSet;
        public List<Transform> zombieSpawnPoint = new List<Transform>();
        public GameObject gameStartButton;
        public GameObject timer;

        public bool isPlayerSpawn = true;
        public bool isPlayerDead = false;
        public bool isShowDebugGUI = false;
        public bool isRestTime = false;
        public bool isSpawningZombies = false;

        public int round = 0;
        public int money = 1000;
        public int remainZombieCount = -1;

        public float restTime = 1f;

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

            if (zombies != null) {
                Debug.LogWarning("좀비 상태 { 전체 배열 크기 : " + zombies.Length 
                    + " / 생성 가능 좀비 수 : " + getZombieArrayEmptyCount() 
                    + "(" + getAvailableZombieIndex() + ") / 살아 있는 좀비 수 : " + getLivedZombieCount() + "(" + remainZombieCount + ") }");
            }
        }

        public void RoundTextUpdate() {
            roundText.text = "Round : " + round;
        }

        public int getLivedZombieCount() {
            int count = -1;

            if (zombies != null || zombies.Length != 0) {
                count = 0;

                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] != null) {
                        count++;
                    }
                }
            }

            remainZombieCount = count;
            return count;
        }

        public int getZombieArrayEmptyCount() {
            int count = -1;

            if (zombies != null || zombies.Length != 0) {
                count = 0;

                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] == null) {
                        count++;
                    }
                }
            }

            return count;
        }

        public int getAvailableZombieIndex() {
            if (zombies != null) {
                return zombies.Length - getZombieArrayEmptyCount();
            }

            return -1;
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
                if (players[i].GetPhotonView().IsMine && players[i].GetComponent<PlayerHP>().isDead) {
                    for (int x = 0; x < players.Length; x++) {
                        if (!players[x].GetPhotonView().IsMine && !players[x].GetComponent<PlayerHP>().isDead) {
                            camSet.Follow = players[x].transform;
                            camSet.LookAt = players[x].transform;
                        }
                    }
                }

                if (players[i].GetPhotonView().IsMine && players[i].GetComponent<PlayerHP>().isDead) {
                    camSet.Follow = players[i].transform;
                    camSet.LookAt = players[i].transform;
                    players[i].GetComponent<Animator>().SetBool("Dead", false);
                }
            }
        }

        private void CheckRemainZombies() {
            if (ui.isGameStart && !isRestTime && isSpawningZombies && remainZombieCount == 0) {
                killAllZombies();

                timer.SetActive(true);

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                for (int i = 0; i < players.Length; i++) {
                    players[i].GetComponent<PlayerHP>().NextRound();
                }

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
            
        }

        IEnumerator RestTime() {
            Debug.LogWarning(restTime + "초간 휴식을 취합니다.");

            yield return new WaitForSecondsRealtime(restTime);

            EnemySpawn();
        }

        public void EnemySpawn() {
            isRestTime = false;
            isSpawningZombies = true;

            if (!PhotonNetwork.IsMasterClient && !PhotonNetwork.IsConnected) {
                return;
            }

            if (!isRestTime) {
                timer.SetActive(false);

                if (PhotonNetwork.IsMasterClient) {
                    Debug.LogWarning("쉬는시간 종료!");

                    round++;
                    Debug.LogWarning("현재 라운드 수 : " + round);

                    StartCoroutine("Enemy_Spawn");
                }
            }
        }

        private IEnumerator Enemy_Spawn() {
            zombies = new GameObject[round * 10];

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
                zombies[getAvailableZombieIndex()] = go;
            }
            else if (spawnNum == 4) {
                GameObject go = PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
                zombies[getAvailableZombieIndex()] = go;
            }
            else if (spawnNum == 5) {
                GameObject go = PhotonNetwork.Instantiate("Heavy Zombie", zombieSpawnPoint[transTemp].position, Quaternion.identity);
                zombies[getAvailableZombieIndex()] = go;
            }

            isSpawningZombies = true;
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
                stream.SendNext(remainZombieCount);

                stream.SendNext(isRestTime);
                stream.SendNext(isSpawningZombies);
            }
            else {
                round = (int) stream.ReceiveNext();
                money = (int) stream.ReceiveNext();
                remainZombieCount = (int) stream.ReceiveNext();

                isRestTime = (bool) stream.ReceiveNext();
                isSpawningZombies = (bool) stream.ReceiveNext();
            }
        }
    }
}