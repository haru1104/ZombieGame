using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

using UnityEngine;
using UnityEngine.UI;

namespace haruroad.szd.multiplayer {
    public class UiManager : MonoBehaviourPun, IPunObservable {
        public GameObject attackButton;
        public GameObject shopButton;
        public GameObject destoryButton;
        public GameObject installButton;
        public GameObject shopInven;
        public GameObject startButton;
        public GameObject Gameover;

        public Text roundText;
        public Text moneyText;

        public bool isShopDown = false;
        public bool isGameStart = false;

        private ItemSpawn itemSpawn;
        private Button gunFireButton;
        private GameManager gm;
        private Gun gun;

        private string obstacleType = "None";

        private void Start() {
            itemSpawn = GameObject.Find("ItemSpawn").GetComponent<ItemSpawn>();
            gunFireButton = GameObject.Find("AttackButton").GetComponent<Button>();
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            Gameover.SetActive(false);
        }

        private void Update() {
            GameStartButton(false);

            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1) {
                PlayerWaitingTime();

                if (isGameStart) {
                    gm.onForceStopGame();
                    isGameStart = false;
                }

                roundText.text = "Waiting for players... (1/2)";
            }
            else {
                if (!isGameStart) {
                    if (PhotonNetwork.IsMasterClient) {
                        GameStartButton(true);
                    }
                    else {
                        PlayerWaitingTime();
                    }

                    roundText.text = "All players connected";
                }

                if (isGameStart) {
                    updateRoundText();
                }
            }

            ShopUiDown();
            updateMoneyAmount();
        }

        private void ShopUiDown() {
            Transform shopTr = shopInven.GetComponent<Transform>();
            Vector3 targetPos = new Vector3(shopTr.position.x, 735, shopTr.position.z);

            if (isShopDown == true) {
                shopTr.position = Vector3.MoveTowards(shopTr.position, targetPos, 10);
                destoryButton.SetActive(true);
                installButton.SetActive(true);
            }
            else {
                targetPos = new Vector3(shopTr.position.x, 1420, shopTr.position.z);
                shopTr.position = Vector3.MoveTowards(shopTr.position, targetPos, 10);
                installButton.SetActive(false);
                destoryButton.SetActive(false);
            }
        }
        public void OnClickStartButton() {
            if (!isGameStart) {
                isGameStart = true;
                gm.onInitGameStart();

                GameStartButton(false);
            }
        }
        public void RestTime() {
            attackButton.SetActive(false);
            shopButton.SetActive(true);
        }

        public void GamePlayTime() {
            shopButton.SetActive(false);
            attackButton.SetActive(true);
        }

        public void GameStartButton(bool temp) {
            startButton.SetActive(temp);
        }

        public void PlayerWaitingTime() {
            attackButton.SetActive(false);
            shopButton.SetActive(false);
        }

        public void ShopOnclick() {
            isShopDown = !isShopDown;
        }

        public void OnClickInstall() {
            itemSpawn.IsInstall();
        }

        public void OnClickCancel() {
            itemSpawn.IsCancel();
        }

        public void OnClickBarrel() {
            itemSpawn.Barrel();
        }

        public void OnClickBarricade() {
            itemSpawn.Barricade();
        }

        public void AttackButton() {
            if (gm.hasLocalPlayerSpawned() == true) {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                for (int i = 0; i < players.Length; i++) {
                    if (players[i].GetPhotonView().IsMine) {
                        gun = players[i].GetComponentInChildren<Gun>();
                        gunFireButton.onClick.AddListener(gun.Fire);
                    }
                }
            }
        }

        public void updateMoneyAmount() {
            moneyText.text = gm.getMoney().ToString();
        }

        private void updateRoundText() {
            roundText.text = "Round " + gm.getRound();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(isGameStart);
                stream.SendNext(obstacleType);
            }
            else {
                isGameStart = (bool) stream.ReceiveNext();
                obstacleType = (string) stream.ReceiveNext();
            }
        }
    }
}
