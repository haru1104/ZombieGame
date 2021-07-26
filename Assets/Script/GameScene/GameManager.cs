using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class Zombies {
    public GameObject normal;
    public GameObject lite;
    public GameObject heavy;
}

public class GameManager : MonoBehaviourPun
{
    private CinemachineVirtualCamera camSet;

    private GameObject playerPrefab;
    private GameObject playerSpawnPosition;
    private GameObject zombieSpawnPosition;
    private Text roundText;
    private Transform playerSpawnPoint;
    private UiManager ui;
    private int getCoin;

    private bool isSpawnPlayer;
    private bool isSpawnZombie;
    public bool isPlayerSpawn = true;
    private bool nextGame;
    private bool gameOver;

    public List<Transform> zombieSpawnPoint = new List<Transform>();

    public Zombies zombie;

    public int Round = 1;
    public bool isPlayerDead;

    public bool isShowDebugGUI = false;

    //개발자 전용 무기 하나 제작 

    // Start is called before the first frame update
    void Start()
    {
        roundText = GameObject.Find("RoundText").GetComponent<Text>();
        ui = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) {
            RoundTextUpdata();
        }
    }
    private void RoundTextUpdata()
    {
        roundText.text = "Round : " + Round;
    }
    
    void OnGUI() {
        if (isShowDebugGUI) {
            GUI.BeginGroup(new Rect(40, 30, 300, 500));

            GUI.Box(new Rect(0, 0, 300, 600), "게임 디버그 메뉴");

            if (GUI.Button(new Rect(30, 40, 230, 40), "일반 좀비 스폰")) {
                Debug.LogWarning("[Debug] 일반 좀비를 강제로 스폰합니다.");
                Instantiate(zombie.normal, transform.position, Quaternion.identity);
            }

            if (GUI.Button(new Rect(30, 90, 230, 40), "라이트 좀비 스폰")) {
                Debug.LogWarning("[Debug] 라이트 좀비를 강제로 스폰합니다.");
                Instantiate(zombie.lite, transform.position, Quaternion.identity);
            }

            if (GUI.Button(new Rect(30, 140, 230, 40), "헤비 좀비 스폰")) {
                Debug.LogWarning("[Debug] 헤비 좀비를 강제로 스폰합니다.");
                Instantiate(zombie.heavy, transform.position, Quaternion.identity);
            }

            GUI.EndGroup();
        }
    }
    

    private void Spawn()
    {
        playerSpawnPoint = GameObject.Find("PlayerSpawnPosition").GetComponent<Transform>();
        PhotonNetwork.Instantiate("Player", playerSpawnPoint.position, Quaternion.identity);

        isPlayerSpawn = true;
        ui.AttackButton();
    }
    private void GetPoint()
    {
        if (nextGame == true)
        {
            //게임클리어를 성공하면 좀비스폰&&특수좀비 스폰 확률 증가.
            //플레이어 통합 (무기)포인트 증가 

        }
        else if (gameOver == true)
        {
            //게임오버시 초기값으로 돌려주는 루틴 제작
        }
    }
}
