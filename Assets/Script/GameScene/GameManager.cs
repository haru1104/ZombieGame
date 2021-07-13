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
   
    //������ ���� ���� �ϳ� ���� 

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
            //����Ŭ��� �����ϸ� ������&&Ư������ ���� Ȯ�� ����.
            //�÷��̾� ���� (����)����Ʈ ���� 

        }
        else if (gameOver == true)
        {
            //���ӿ����� �ʱⰪ���� �����ִ� ��ƾ ����
        }
    }
}
