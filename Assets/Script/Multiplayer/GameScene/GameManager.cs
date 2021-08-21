using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using Photon.Pun;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class GameManager : MonoBehaviourPun, IPunObservable {
        private string prefix = "[Multiplayer::GameManager] ";

        public static int viewId = 0;

        public static float spawnCooldown = 1f;
        public static float restCooldown = 60f;

        private int nowRound = 0;
        private int nowMoney = 1000;

        private bool isPlayerSpawned = false;
        private bool isZombieSpawnComplete = false;

        private GameObject[] zombies;

        private GameObject player;
        private WaitForSecondsRealtime spawnSeconds = new WaitForSecondsRealtime(spawnCooldown);
        private WaitForSecondsRealtime restSeconds = new WaitForSecondsRealtime(restCooldown);

        public List<Transform> zombieSpawnPoint = new List<Transform>();

        public GameObject timer;

        public Transform playerSpawnPoint;
        public UiManager ui;
        public CinemachineVirtualCamera vcam;

        void Start() {
            timer.SetActive(false);

            player = PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);
            viewId = player.GetPhotonView().ViewID;

            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;

            isPlayerSpawned = true;

            ui.AttackButton();
        }

        void Update() {
            onPlayerDead();

            if (isPlayerSpawned) {
                if (PhotonNetwork.IsMasterClient) {
                    checkZombies();
                }
            }
        }

        private bool isZombiesNull() {
            if (zombies == null || zombies.Length == 0) {
                return true;
            }

            return false;
        }

        private int getLivedZombieCount() {
            int count = -1;

            if (!isZombiesNull()) {
                count = 0;

                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] != null) {
                        count++;
                    }
                }
            }

            return count;
        }

        private int getEmptyZombieCount() {
            int count = -1;

            if (!isZombiesNull()) {
                count = 0;

                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] == null) {
                        count++;
                    }
                }
            }

            return count;
        }

        private IEnumerator startRestTime() {
            yield return restSeconds;

            photonView.RPC("onRoundStart", RpcTarget.All);
        }

        private IEnumerator initZombies() {
            for (int i = 0; i < zombies.Length; i++) {
                yield return spawnSeconds;

                int zombie = Random.Range(1, 6);
                int position = Random.Range(0, 4);

                spawnZombies(zombie, position);
            }

            Debug.LogWarning(prefix + "총 " + zombies.Length + "마리의 좀비 스폰을 완료하였습니다.");
            isZombieSpawnComplete = true;
        }

        private void spawnZombies(int zombieNum, int spawnPoint) {
            GameObject go;

            if (zombieNum >= 1 && zombieNum <= 3) {
                go = PhotonNetwork.Instantiate("Normal Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
                zombies[zombies.Length - getEmptyZombieCount()] = go;
            }
            else if (zombieNum == 4) {
                go = PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
                zombies[zombies.Length - getEmptyZombieCount()] = go;
            }
            else if (zombieNum == 5) {
                go = PhotonNetwork.Instantiate("Heavy Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
                zombies[zombies.Length - getEmptyZombieCount()] = go;
            }
        }

        private void checkZombies() {
            if (isZombieSpawnComplete && zombies != null) {
                if ((getEmptyZombieCount() == zombies.Length) && (getLivedZombieCount() == 0)) {
                    isZombieSpawnComplete = false;

                    photonView.RPC("onStartRestTime", RpcTarget.All);
                }
            }
        }

        private void killAllZombies() {
            StopCoroutine("initZombies");

            for (int i = 0; i < zombies.Length; i++) {
                if (zombies[i] != null) {
                    zombies[i].GetComponent<Zombie>().onDamaged(float.MaxValue);
                }
            }
        }

        [PunRPC]
        private void onGameStartRPC() {
            Debug.LogError(prefix + "모든 플레이어가 접속하였습니다. 게임을 시작합니다!");

            nowRound = 0;
            onRoundStart();
        }

        public void onInitGameStart() {
            photonView.RPC("onGameStartRPC", RpcTarget.All);
        }

        public void onForceStopGame() {
            Debug.LogWarning(prefix + "게임을 강제로 종료합니다.");

            killAllZombies();
        }

        [PunRPC]
        public void onRoundStart() {
            Debug.LogError(prefix + "라운드를 준비하고 있습니다...");

            ui.GamePlayTime();
            timer.SetActive(false);

            if (PhotonNetwork.IsMasterClient) {
                nowRound++;

                zombies = new GameObject[nowRound * 10];

                Debug.LogWarning(prefix + nowRound + "라운드를 시작합니다. 좀비 스폰 중...");
                StartCoroutine("initZombies");
            }
        }

        [PunRPC]
        public void onStartRestTime() {
            Debug.LogError(prefix + "모든 좀비를 처치하였습니다. " + restCooldown + "초간 휴식을 취합니다...");

            ui.RestTime();

            timer.SetActive(true);
            onAllPlayersRespawn();

            if (PhotonNetwork.IsMasterClient) {
                StopCoroutine("initZombies");
                StartCoroutine("startRestTime");
            }
        }

        public void onGameOver() {
            int deadCount = 0;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetComponent<PlayerHP>().isDead) {
                    deadCount++;
                }
            }

            if (deadCount == players.Length) {
                ui.Gameover.SetActive(true);
            }
        }

        public void onAllPlayersRespawn() {
            Debug.LogError(prefix + "모든 플레이어의 체력을 100으로 설정하고 죽은 플레이어를 리스폰합니다.");

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetPhotonView().IsMine) {
                    players[i].GetComponent<PlayerHP>().NextRound();

                    vcam.Follow = players[i].transform;
                    vcam.LookAt = players[i].transform;
                }
            }
        }

        public void onPlayerDead() {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetPhotonView().IsMine && players[i].GetComponent<PlayerHP>().isDead) {
                    for (int j = 0; j < players.Length; j++) {
                        if (!players[j].GetPhotonView().IsMine && !players[j].GetComponent<PlayerHP>().isDead) {
                            vcam.Follow = players[j].transform;
                            vcam.LookAt = players[j].transform;
                        }
                    }
                }
            }
        }

        public void addMoney(int amount) {
            if (PhotonNetwork.IsConnected) {
                nowMoney += amount;
            }

            ui.updateMoneyAmount();
        }

        public void removeMoney(int amount) {
            if (PhotonNetwork.IsConnected) {
                nowMoney -= amount;
            }

            ui.updateMoneyAmount();
        }

        public int getMoney() {
            return nowMoney;
        }

        public int getRound() {
            return nowRound;
        }

        public bool hasLocalPlayerSpawned() {
            return isPlayerSpawned;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(nowRound);
                stream.SendNext(nowMoney);
            }
            else {
                nowRound = (int) stream.ReceiveNext();
                nowMoney = (int) stream.ReceiveNext();
            }
        }
    }
}