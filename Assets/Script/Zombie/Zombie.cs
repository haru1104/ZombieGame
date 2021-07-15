using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public enum ZombieType {
    Normal, Heavy, Lite
}

public abstract class Zombie : MonoBehaviour {
    protected List<Transform> playerTr = new List<Transform>();

    protected Animator ani;
    protected NavMeshAgent agent;
    protected ParticleSystem particle;

    public ZombieType type { get; set; }

    public float health { get; set; }
    public float attackDamage { get; set; }
    public float attackDelay { get; set; }
    public float attackCooldown { get; set; }
    public float moveSpeed { get; set; }

    public Color zombieColor { get; set; }

    void OnEnable() {
        Debug.Log(transform.GetComponent<NormalZombie>() == null);

        onSpawn();
    }

    void Update() {
        onMove();
        onDeath();
    }

    public abstract void onSpawn();
    public abstract void onMove();
    public abstract void onAttack();
    public abstract void onDamaged();
    public abstract void onDeath();
}