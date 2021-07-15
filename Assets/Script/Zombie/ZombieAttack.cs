using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private int attackDamage=2;
    private GameManager gm;
    private PlayerHP player_HP;
    private Animator ani;


    // Start is called before the first frame update
    void Start()
    {
        //������ ���ÿ� ���ݷ� ����
        StartReset();

    }

    private void StartReset()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        ani = transform.parent.gameObject.GetComponent<Animator>();

        if (gm != null)
        {
            Debug.Log("GameManager Find Success");
            //�⺻ ������ * ���� ��
            attackDamage = attackDamage * gm.Round;
            Debug.Log("���� ���ݷ� ���� : " + attackDamage);
        }
        else
        {
            Debug.LogWarning("GameManager Fail");
        }

        player_HP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();

        if (player_HP != null)
        {
            Debug.Log("Player Hp Find Success");
        }
        else
        {
            Debug.LogWarning("Player Hp Fail");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            NormalZombie zombie = transform.parent.gameObject.GetComponent<NormalZombie>();

            zombie.onAttack();

            player_HP.playerHP -= zombie.attackDamage;
            player_HP.DamageAni();
        }
    }
}
