using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public enum ZombieType {
    Normal, Heavy, Lite
}

public abstract class Zombie : MonoBehaviour {
    protected List<Transform> playerTr = new List<Transform>();
    protected List<Transform> obstacleTr = new List<Transform>();

    protected Animator ani;
    protected NavMeshAgent agent;
    protected ParticleSystem particle;

    public bool isDead { get; protected set; }

    public ZombieType type { get; set; }

    public float health { get; set; }
    public float attackDamage { get; set; }
    public float attackSpeed { get; set; }
    public float moveSpeed { get; set; }

    public Color zombieColor { get; set; }

    void OnEnable() {
        isDead = false;

        onSpawn();
    }

    void Update() {
        onMove();
        onDeath();
    }

    public virtual void onDamaged(float damage) {
        health -= damage;
        particle.Play();

        Debug.LogWarning("[Zombie] 좀비가 " + damage + " 만큼의 대미지를 입음! 현재 체력: " + health);
    }

    public virtual void onDeath() {
        if (!isDead && health <= 0) {
            isDead = true;

            agent.isStopped = true;
            agent.enabled = false;

            ani.SetTrigger("Die " + Random.Range(1, 3));
        }
    }

    public abstract void onSpawn();
    public abstract void onMove();
    public abstract void onAttack();
}