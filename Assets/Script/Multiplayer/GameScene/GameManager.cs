using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using Photon.Pun;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class GameManager : MonoBehaviourPun, IPunObservable {
        private string prefix = "[Multiplayer::GameManager] ";

        public static int viewId = 0;

        public static float spawnSecond = 1f;
        public static float restSecond = 60f;

        private int nowRound = 0;
        private int nowMoney = 1000;

        private bool isRoundStarted = false;
        private bool isPlayerSpawned = false;
        private bool isZombieSpawnComplete = false;

        private GameObject[] zombies;

        private GameObject player;
        private WaitForSecondsRealtime spawnDelay = new WaitForSecondsRealtime(spawnSecond);
        private WaitForSecondsRealtime restDealy = new WaitForSecondsRealtime(restSecond);

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
            if (PhotonNetwork.IsMasterClient && isRoundStarted) {
                checkZombies();
            }

            onPlayerDead();
            onGameOver();
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

        private void addZombie(GameObject go) {
            if (!isZombiesNull()) {
                for (int i = 0; i < zombies.Length; i++) {
                    if (zombies[i] == null) {
                        zombies[i] = go;
                        break;
                    }
                }
            }
        }

        private IEnumerator startRestTime() {
            yield return restDealy;

            onRoundStart();
            photonView.RPC("onReadyGameStartRPC", RpcTarget.AllBufferedViaServer);
        }

        private IEnumerator initZombies() {
            for (int i = 0; i < zombies.Length; i++) {
                yield return spawnDelay;

                int zombie = UnityEngine.Random.Range(1, 6);
                int position = UnityEngine.Random.Range(0, 4);

                spawnZombies(zombie, position);
            }

            Debug.LogWarning(prefix + "총 " + zombies.Length + "마리의 좀비 스폰을 완료하였습니다.");
            isZombieSpawnComplete = true;
        }

        private void spawnZombies(int zombieNum, int spawnPoint) {
            GameObject go = null;

            if (zombieNum >= 1 && zombieNum <= 3) {
                go = PhotonNetwork.Instantiate("Normal Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
            }
            else if (zombieNum == 4) {
                go = PhotonNetwork.Instantiate("Lite Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
            }
            else if (zombieNum == 5) {
                go = PhotonNetwork.Instantiate("Heavy Zombie", zombieSpawnPoint[spawnPoint].position, Quaternion.identity);
            }

            if (go != null) {
                addZombie(go);
            }
            else {
                Debug.LogError("[Multiplayer:GameManager] 좀비를 관리 목록에 추가하지 못했습니다.");
            }
        }

        private void checkZombies() {
            if (isZombieSpawnComplete && zombies != null) {
                Debug.LogWarning("[Multiplayer:GameManager] Zombies[] Info { Length: " + zombies.Length + " / Lived: " + getLivedZombieCount() + " / Died: " + getEmptyZombieCount() + " }");

                if ((getEmptyZombieCount() == zombies.Length) && (getLivedZombieCount() == 0)) {
                    isZombieSpawnComplete = false;

                    photonView.RPC("onStartRestTime", RpcTarget.AllBufferedViaServer);
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
        private void addMoneyRPC(int amount) {
            if (PhotonNetwork.IsConnected) {
                nowMoney += amount;
            }
        }

        [PunRPC]
        private void removeMoneyRPC(int amount) {
            if (PhotonNetwork.IsConnected) {
                nowMoney -= amount;
            }
        }

        [PunRPC]
        private void onGameStartRPC() {
            Debug.LogError(prefix + "모든 플레이어가 접속하였습니다. 게임을 시작합니다!");

            nowRound = 0;

            photonView.RPC("onReadyGameStartRPC", RpcTarget.AllBufferedViaServer);
            onRoundStart();
        }

        [PunRPC]
        private void onReadyGameStartRPC() {
            ui.toggleUIButtons("GamePlayTime");

            timer.SetActive(false);
        }

        public void onInitGameStart() {
            photonView.RPC("onGameStartRPC", RpcTarget.AllBufferedViaServer);
        }

        public void onForceStopGame() {
            Debug.LogWarning(prefix + "게임을 강제로 종료합니다.");

            killAllZombies();
        }

        public void onRoundStart() {
            Debug.LogError(prefix + "라운드를 준비하고 있습니다...");

            isRoundStarted = true;

            if (PhotonNetwork.IsMasterClient) {
                nowRound++;

                zombies = new GameObject[nowRound * 10];

                Debug.LogWarning(prefix + nowRound + "라운드를 시작합니다. 좀비 스폰 중...");
                StartCoroutine("initZombies");
            }
        }

        [PunRPC]
        public void onStartRestTime() {
            isRoundStarted = false;

            Debug.LogError(prefix + "모든 좀비를 처치하였습니다. " + restSecond + "초간 휴식을 취합니다...");

            try {
                killAllZombies();
            } catch(NullReferenceException) {
                Debug.LogError("[Multiplayer:GameManager] 죽일 좀비가 없습니다.");
            } finally {
                ui.toggleUIButtons("RestTime");
            }

            timer.SetActive(true);

            photonView.RPC("onAllPlayersRespawn", RpcTarget.AllBufferedViaServer);

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

        [PunRPC]
        public void onAllPlayersRespawn() {
            Debug.LogError(prefix + "모든 플레이어의 체력을 100으로 설정하고 죽은 플레이어를 리스폰합니다.");

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetPhotonView().IsMine) {
                    players[i].GetComponent<PlayerHP>().NextRound();
                }
            }
        }

        public void onPlayerDead() {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetPhotonView().IsMine) {
                    if (players[i].GetComponent<PlayerHP>().isDead) {
                        for (int j = 0; j < players.Length; j++) {
                            if (!players[j].GetPhotonView().IsMine && !players[j].GetComponent<PlayerHP>().isDead) {
                                vcam.Follow = players[j].transform;
                                vcam.LookAt = players[j].transform;
                            }
                        }
                    }
                    else {
                        for (int j = 0; j < players.Length; j++) {
                            if (players[j].GetPhotonView().IsMine) {
                                vcam.Follow = players[j].transform;
                                vcam.LookAt = players[j].transform;
                            }
                        }
                    }
                }
            }
        }

        public void addMoney(int amount) {
            photonView.RPC("addMoneyRPC", RpcTarget.AllBuffered, amount);

            ui.updateMoneyAmount();
        }

        public void removeMoney(int amount) {
            photonView.RPC("removeMoneyRPC", RpcTarget.AllBuffered, amount);

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
                
                if (PhotonNetwork.IsMasterClient) {
                    stream.SendNext(isRoundStarted);
                }
            }
            else {
                nowRound = (int) stream.ReceiveNext();
                nowMoney = (int) stream.ReceiveNext();

                isRoundStarted = (bool) stream.ReceiveNext();
            }
        }
    }
}