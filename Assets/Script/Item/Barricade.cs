using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private Transform playerTr;
    private Transform barricadeTr;
    public bool isOk = false; // isOk == false (�÷��̾ ���� ������) is0k == true (�÷��̾� ���� ����)
    private Vector3 moveTr;
    private int hp = 50;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        barricadeTr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        MoveSet();
        DamageChack();
    }
    private void MoveSet()
    {
        if (isOk == false)
        {
            moveTr = playerTr.position;
            barricadeTr.position = moveTr;
            barricadeTr.rotation = playerTr.rotation;
        }
    }
    private void DamageChack()
    {
        if (hp <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
