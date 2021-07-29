using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;
public class UiManager : MonoBehaviourPun , IPunObservable
{
    public GameObject attackButton;
    public GameObject shopButton;
    public GameObject destoryButton;
    public GameObject installButton;
    public GameObject hpBar;
    public GameObject shopInven;
    public GameObject startButton;

    public Text roundText;
    public Text moneyText;

    private ItemSpawn itemSpawn;
    private Button gunFireButton;
    private GameManager gm;
    private Gun gun;

    public bool istimeCheck = true;
    public bool isShopDown = false;
    public bool isGameStart = false;

    private void Start()
    {
        itemSpawn = GameObject.Find("ItemSpawn").GetComponent<ItemSpawn>();
        gunFireButton = GameObject.Find("AttackButton").GetComponent<Button>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update() {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1) {
            roundText.text = "Waiting for players... (1/2)";
        }

        if (istimeCheck == true && isGameStart == true) {
            Breaktime();
        }
        else if (isGameStart == true && istimeCheck == false) {
            GamePlayTime();
        }
        else if (isGameStart == false) {
            PlayerWaitingTime();
        }

        ShopUiDown();
        updateMoneyAmount();
    }

    private void ShopUiDown() {
        // int downSpeed = 10;

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

    public void Breaktime()
    {
        attackButton.SetActive(false);
        shopButton.SetActive(true);

        hpBar.SetActive(false);
    }

    public void GamePlayTime()
    {
        shopButton.SetActive(false);
        attackButton.SetActive(true);
        hpBar.SetActive(true);
    }

    public void GameStartButton(bool temp)
    {
        startButton.SetActive(temp);
    }

    public void PlayerWaitingTime()
    {
        attackButton.SetActive(false);
        shopButton.SetActive(false);
        hpBar.SetActive(false);
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

    public void AttackButton()
    {
        if (gm.isPlayerSpawn == true)
        {
            Debug.Log(gunFireButton);

            gun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();
            gunFireButton.onClick.AddListener(gun.Fire);
        }
    }

    public void updateMoneyAmount() {
        moneyText.text = gm.money.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isGameStart);
        }
        else
        {
            isGameStart = (bool) stream.ReceiveNext();
        }
    }
}
