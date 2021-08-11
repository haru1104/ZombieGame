using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace haruroad.szd.singleplayer
{
    public class UiManager : MonoBehaviour
    {
        public GameObject attackButton;
        public GameObject shopButton;
        public GameObject destoryButton;
        public GameObject installButton;
        public GameObject hpBer;
        public GameObject shopInven;
        public GameObject Timer;
        private ItemSpawn itemSpawn;
        private GameObject deadScreen;
        private Button gunFireButton;
        private GameManager gm;
        private Gun gun;

        public bool istimeCheck = false;
        public bool isShopDown = false;

        private void Start()
        {
            itemSpawn = GameObject.Find("ItemSpawn").GetComponent<ItemSpawn>();
            gunFireButton = GameObject.Find("AttackButton").GetComponent<Button>();
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            deadScreen = GameObject.Find("DeadScreen");
            deadScreen.SetActive(false);
            
        }
        public void DeadCheck()
        {
            deadScreen.SetActive(true);
        }
        public void Breaktime()
        {
            attackButton.SetActive(false);
            shopButton.SetActive(true);
            Timer.SetActive(true);
            hpBer.SetActive(false);
        }
        public void GamePlayTime()
        {
            Timer.SetActive(false);
            shopButton.SetActive(false);
            attackButton.SetActive(true);
            hpBer.SetActive(true);
        }

        public void ShopOnclick()
        {
            isShopDown = !isShopDown;
        }
        public void OnClickInstall()
        {
            itemSpawn.IsInstall();
        }
        public void OnClickCancel()
        {
            itemSpawn.IsCancel();
        }
        public void OnClickBarrel()
        {
            itemSpawn.Barrel();
        }
        public void OnClickBarricade()
        {
            itemSpawn.Barricade();
        }
        public void Update()
        {
            if (istimeCheck == true)
            {
                Breaktime();
            }
            else
            {
                GamePlayTime();
            }
            ShopUiDown();

        }
        private void ShopUiDown()
        {
            // int downSpeed = 10;

            Transform shopTr = shopInven.GetComponent<Transform>();
            Vector3 targetPos = new Vector3(shopTr.position.x, 735, shopTr.position.z);
            if (isShopDown == true)
            {
                shopTr.position = Vector3.MoveTowards(shopTr.position, targetPos, 10);
                destoryButton.SetActive(true);
                installButton.SetActive(true);
            }
            else
            {
                targetPos = new Vector3(shopTr.position.x, 1420, shopTr.position.z);
                shopTr.position = Vector3.MoveTowards(shopTr.position, targetPos, 10);
                installButton.SetActive(false);
                destoryButton.SetActive(false);
            }

        }
        public void AttackButton()
        {
            if (gm.isPlayerSpawn == true)
            {
                Debug.Log(gunFireButton);

                gun = GameObject.Find("Player").GetComponentInChildren<Gun>();
                gunFireButton.onClick.AddListener(gun.Fire);
            }
        }

    }
}
