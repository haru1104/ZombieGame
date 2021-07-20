using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour {
    private Transform playerTr;
    private Transform barricadeTr;
    private Vector3 moveTr;

    public float health = 100.0f;

    public bool isSetted = false;

    void Start() {
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        barricadeTr = GetComponent<Transform>();

    }

    void Update() {
        MoveSet();
        DamageCheck();
    }

    private void MoveSet() {
        if (!isSetted) {
            moveTr = playerTr.position;
            barricadeTr.position = moveTr;
            barricadeTr.rotation = playerTr.rotation;
        }
    }

    private void DamageCheck() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void onDamaged(float damage) {
        health -= damage;
        Debug.Log("[Barricade] " + damage + "��ŭ�� ������� ����. ���� ü��: " + health);
    }
}
