using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
