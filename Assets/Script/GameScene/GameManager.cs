using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Zombies {
    public GameObject normal;
    public GameObject lite;
    public GameObject heavy;
}

public class GameManager : MonoBehaviour
{
    private CinemachineVirtualCamera camSet;
    private GameObject playerPrefab;
    private GameObject zombiePrefab;
    private GameObject playerSpawnPosition;
    private GameObject zombieSpawnPosition;
    private Transform playerSpawnPoint;
    public List<Transform> zombieSpawnPoint = new List<Transform>();
  

    private bool isSpawnPlayer;
    private bool isSpawnZombie;
    public bool isPlayerDead;
    private bool nextGame;
    private bool gameOver;
    public int Round=1;
    private int getCoin;

    public Zombies zombie;

    //개발자 전용 무기 하나 제작 

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        GUI.BeginGroup(new Rect(40, 30, 300, 500));

        GUI.Box(new Rect(0, 0, 300, 600), "게임 디버그 메뉴");
        
        if (GUI.Button(new Rect(30, 40, 230, 40), "일반 좀비 스폰")) {
            Debug.LogWarning("[Debug] 일반 좀비를 강제로 스폰합니다.");
            Instantiate(zombie.normal, transform.position, Quaternion.identity);
        }

        if (GUI.Button(new Rect(30, 90, 230, 40), "라이트 좀비 스폰")) {
            Debug.LogWarning("[Debug] 라이트 좀비를 강제로 스폰합니다.");
        }

        if (GUI.Button(new Rect(30, 140, 230, 40), "헤비 좀비 스폰")) {
            Debug.LogWarning("[Debug] 헤비 좀비를 강제로 스폰합니다.");
        }

        if (GUI.Button(new Rect(30, 190, 230, 40), "현재 위치에 장애물 설치")) {
            Debug.LogWarning("[Debug] 폭발물(Barrel)을 설치하였습니다.");

            GameObject barrel = GameObject.FindGameObjectWithTag("Barrel");
            Material mat = barrel.GetComponent<MeshRenderer>().material;

            barrel.GetComponent<Barrel>().isOk = true;
            
        }

        GUI.EndGroup();
    }

    private void Spawn()
    {
        playerSpawnPoint = GameObject.Find("PlayerSpawnPosition").GetComponent<Transform>();
    
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
