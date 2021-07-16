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

    void OnGUI() {
        GUI.BeginGroup(new Rect(175, 40, 300, 500));

        GUI.Box(new Rect(0, 0, 300, 600), "���� ����� �޴�");

        if (GUI.Button(new Rect(30, 40, 230, 40), "�Ϲ� ���� ����")) {
            Debug.LogWarning("[Debug] �Ϲ� ���� ������ �����մϴ�.");
        }

        if (GUI.Button(new Rect(30, 90, 230, 40), "����Ʈ ���� ����")) {
            Debug.LogWarning("[Debug] ����Ʈ ���� ������ �����մϴ�.");
        }

        if (GUI.Button(new Rect(30, 140, 230, 40), "��� ���� ����")) {
            Debug.LogWarning("[Debug] ��� ���� ������ �����մϴ�.");
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
            //����Ŭ��� �����ϸ� ������&&Ư������ ���� Ȯ�� ����.
            //�÷��̾� ���� (����)����Ʈ ���� 

        }
        else if (gameOver == true)
        {
            //���ӿ����� �ʱⰪ���� �����ִ� ��ƾ ����
        }
    }
}
