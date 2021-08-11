using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
namespace haruroad.szd.singleplayer
{
    public enum ZombieType {
    Normal, Heavy, Lite
    }

    public abstract class Zombie : MonoBehaviour
    {
        protected List<Transform> playerTr = new List<Transform>();
        protected List<Transform> obstacleTr = new List<Transform>();

        protected Animator ani;
        protected NavMeshAgent agent;
        protected ParticleSystem particle;

        GameManager gm;

        public bool isDead { get; protected set; }

        public ZombieType type { get; set; }

        public float health { get; set; }
        public float attackDamage { get; set; }
        public float attackSpeed { get; set; }
        public float moveSpeed { get; set; }

        public Color zombieColor { get; set; }

        void OnEnable()
        {
            isDead = false;
             gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            onSpawn();
        }

        void Update()
        {
            onMove();
            onDeath();
        }

        private void findPlayers()
        {
            if (playerTr.Count != 0)
            {
                playerTr.Clear();
            }

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++)
            {
                playerTr.Add(players[i].transform);
            }
        }

        private void findBarricade()
        {
            if (obstacleTr.Count != 0)
            {
                obstacleTr.Clear();
            }

            GameObject[] barricades = GameObject.FindGameObjectsWithTag("Barricade");
            GameObject[] barrels = GameObject.FindGameObjectsWithTag("Barrel");

            for (int i = 0; i < barricades.Length; i++)
            {
                obstacleTr.Add(barricades[i].transform);
            }

            for (int i = 0; i < barrels.Length; i++)
            {
                obstacleTr.Add(barrels[i].transform);
            }
        }

        private void playZombieAnimation()
        {
            if ((agent.velocity.x != 0) && (agent.velocity.z != 0))
            {
                ani.SetBool("Run", true);
            }
            else
            {
                ani.SetBool("Run", false);
            }
        }

        public virtual void onMove()
        {
            if (agent.enabled && !agent.isStopped)
            {
                findPlayers();
                findBarricade();

                int index = 0;
                bool isObstacle = false;
                float dist = float.MaxValue;

                for (int i = 0; i < playerTr.Count; i++)
                {
                    float tempDist = Vector3.Distance(transform.position, playerTr[i].position);

                    if (tempDist <= dist)
                    {
                        isObstacle = false;
                        index = i;
                        dist = tempDist;
                    }
                }

                for (int i = 0; i < obstacleTr.Count; i++)
                {
                    if (obstacleTr[i] != null)
                    {
                        float tempDist = Vector3.Distance(transform.position, obstacleTr[i].position);

                        if (tempDist <= dist)
                        {
                            isObstacle = true;
                            index = i;
                            dist = tempDist;
                        }
                    }
                    else
                    {
                        obstacleTr.RemoveAt(i);
                        onMove();
                        return;
                    }
                }

                if (isObstacle)
                {
                    agent.SetDestination(obstacleTr[index].position);
                }
                else
                {
                    agent.SetDestination(playerTr[index].position);
                }

                playZombieAnimation();
            }
        }

        public virtual void onAttack()
        {
            ani.SetTrigger("Attack");
        }

        public virtual void onDamaged(float damage)
        {
            health -= damage;
            particle.Play();
        }

        public virtual void onDeath()
        {
            if (!isDead && health <= 0)
            {
                isDead = true;

                agent.isStopped = true;
                agent.enabled = false;

                ani.SetTrigger("Die " + Random.Range(1, 3));

                Invoke("destroyBody", 3f);

            }
        }

        public virtual void destroyBody()
        {
            if (isDead)
            {
                //Destroy(gameObject);
                gm.ZombieDead();
            }
        }

        public abstract void onSpawn();
    }
}