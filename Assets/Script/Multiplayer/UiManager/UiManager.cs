using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

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
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1) {
                roundText.text = "Waiting for players... (1/2)";
            }

            if (isGameStart) {
                if (gm.isRestTime) {
                    Breaktime();
                }
                else {
                    GamePlayTime();
                }
            }
            else {
                PlayerWaitingTime();
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && isGameStart == false) {
                if (PhotonNetwork.IsMasterClient == true) {
                    GameStartButton(true);
                }
            }

            ShopUiDown();
            updateMoneyAmount();
            gm.RoundTextUpdate();
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
            if (isGameStart == false) {
                isGameStart = true;
                gm.isRestTime = false;

                gm.EnemySpawn();

                GameStartButton(false);
            }
        }
        public void Breaktime() {
            attackButton.SetActive(false);
            shopButton.SetActive(true);

            // hpBar.SetActive(false);
        }

        public void GamePlayTime() {
            shopButton.SetActive(false);
            attackButton.SetActive(true);
            // hpBar.SetActive(true);
        }

        public void GameStartButton(bool temp) {
            startButton.SetActive(temp);
        }

        public void PlayerWaitingTime() {
            attackButton.SetActive(false);
            shopButton.SetActive(false);
            //  hpBar.SetActive(false);
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
            if (gm.isPlayerSpawn == true) {
                Debug.Log(gunFireButton);

                gun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();
                gunFireButton.onClick.AddListener(gun.Fire);
            }
        }

        public void updateMoneyAmount() {
            moneyText.text = gm.money.ToString();
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
