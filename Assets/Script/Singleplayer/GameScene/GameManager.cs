using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

namespace haruroad.szd.singleplayer {
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
            RoundTextUpdata();
        }
        private void RoundTextUpdata()
        {
            roundText.text = "Round : " + Round;
        }
        

        private void Spawn()
        {
            playerSpawnPoint = GameObject.Find("PlayerSpawnPosition").GetComponent<Transform>();
            
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
};