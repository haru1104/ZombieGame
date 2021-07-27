using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

public class Barricade : MonoBehaviour {
    private Transform playerTr;
    private Vector3 moveTr;
    private SoundManager sound;

    private bool isSetted = false;
    private bool isDestory = false;

    public float health = 100.0f;

    void Start() {
        sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    void Update() {
        MoveSet();
        DamageCheck();
    }

    private void MoveSet() {
        FindPlayer();

        if (!isSetted) {
            moveTr = playerTr.position;
            transform.position = moveTr ;
            transform.rotation = playerTr.rotation;
        }
    }

    private void DamageCheck() {
        if (health <= 0 && isSetted && !isDestory) {
            isDestory = true;
            Destroy(gameObject);
        }
    }

    public void onDamaged(float damage) {
        sound.BarricadeDamage();
        health -= damage;
    }

    public void SetPosition() {
        isSetted = true;
        GetComponent<BoxCollider>().isTrigger = false;
    }

    private void FindPlayer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            if (players[i].GetComponent<PhotonView>().IsMine) {
                playerTr = players[i].transform;
            }
        }
    }
}
