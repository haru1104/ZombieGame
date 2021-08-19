using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace haruroad.szd.singleplayer {

    public class GameManager : MonoBehaviour
    {
        private CinemachineVirtualCamera camSet;

        private PlayerHP playerHP;
        private GameObject playerSpawnPosition;
        private GameObject zombieSpawnPosition;
        private Text roundText;
        private Transform playerSpawnPoint;
        private UiManager ui;
        private int money;
        private int zombieSpawnCount=1;
        private bool isSpawnPlayer;
        private bool isSpawnZombie;
        public bool isPlayerSpawn = true;
        private bool nextGame=false;
        private bool gameOver=false;

        public List<Transform> zombieSpawnPoint = new List<Transform>();
        private List<GameObject> zombieSpawn = new List<GameObject>();
        public List<GameObject> zombieType = new List<GameObject>();

        public int Round = 1;
        public bool isPlayerDead;

        //개발자 전용 무기 하나 제작 

        // Start is called before the first frame update
        void Start()
        {
            roundText = GameObject.Find("RoundText").GetComponent<Text>();
            ui = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
            playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();
            PlayerSpawn();
        }

        // Update is called once per frame
        void Update()
        {
            RoundTextUpdata();
            ZombieSpawn();
            state();
        }
        private void RoundTextUpdata()
        {
            roundText.text = "Round : " + Round;

        }
        
        private void PlayerSpawn()
        {
           
            ui.AttackButton();
        }
        private void state()
        {
            if (nextGame == false)
            {
                //게임클리어를 성공하면 좀비스폰&&특수좀비 스폰 확률 증가.
                //플레이어 통합 (무기)포인트 증가 
                if (playerHP.health > 0 && zombieSpawnCount == 0)
                {
                    nextGame = true;
                    Debug.LogError("다음 라운드");
                    Round++;
                    


                }
                else if(playerHP.health <= 0)
                {
                   //게임오버
                    ui.DeadCheck();
                    StartCoroutine("SceneReset");
                }
            }
            else if (nextGame == true)
            {

            }

        }
        private void ZombieSpawn()
        {
            if (isSpawnZombie == false)
            {
                StartCoroutine("SpawnZombie");

                isSpawnZombie = true;
            }
        }
        public void ZombieDead()
        {
            for (int i = 0; i < zombieSpawn.Count; i++)
            {
                if (zombieSpawn[i].GetComponent<Zombie>().isDead == true)
                {
                    money += 100;
                    GameObject go = zombieSpawn[i];
                    zombieSpawn.RemoveAt(i);
                    Destroy(go);
                    zombieSpawnCount--;

                }
            }
        }
        IEnumerator SceneReset()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("InitialScene");
        }
        IEnumerator SpawnZombie()
        {
            for (int i = 0; i < zombieSpawnCount; i++)
            {
                yield return new WaitForSeconds(1.5f);
                int random = Random.Range(0, 4);
                int random2 = Random.Range(0, 3);
                GameObject go = Instantiate(this.zombieType[random2], zombieSpawnPoint[random].position, Quaternion.identity);
                zombieSpawn.Add(go);
            }
        }
    }
}