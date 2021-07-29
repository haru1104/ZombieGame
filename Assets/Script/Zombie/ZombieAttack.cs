using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class ZombieAttack : MonoBehaviour {
    private GameManager gm;
    private PlayerHP player_HP;
    private Animator ani;

    private int attackDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        //스폰과 동시에 공격력 세팅
        StartReset();
    }

    private void StartReset()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        ani = transform.parent.gameObject.GetComponent<Animator>();

        if (gm != null)
        {
            //기본 데미지 * 라운드 수
            attackDamage = attackDamage * gm.round;
        }
        else
        {
            Debug.LogError("[Zombie:Attack] GameManager is not found.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player_HP = other.gameObject.GetComponent<PlayerHP>();
            StartCoroutine("AttackPlayer");
        }

        if (other.tag == "Barricade" || other.tag == "Barrel") {
            StartCoroutine("AttackObstacles", other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            StopCoroutine("AttackPlayer");
        }

        if (other.tag == "Barricade" || other.tag == "Barrel") {
            StopCoroutine("AttackObstacles");
        }
    }

    IEnumerator AttackPlayer() {
        Zombie zombie = transform.parent.gameObject.GetComponent<Zombie>();

        if (!zombie.isDead) {
            while (true) {
                zombie.onAttack();

                yield return new WaitForSeconds(0.475f);
                player_HP.onDamaged(zombie.attackDamage);

                yield return new WaitForSeconds(zombie.attackSpeed);
            }
        }
    }

    // 나중에 장애물 더 추가할 예정이라면 Obstacles 클래스로 정리할 것
    IEnumerator AttackObstacles(GameObject obj) {
        Zombie zombie = transform.parent.gameObject.GetComponent<Zombie>();

        if (!zombie.isDead && obj != null) {
            while (true) {
                zombie.onAttack();

                if (obj.tag == "Barrel") {
                    obj.GetComponent<Barrel>().onDamaged(zombie.attackDamage);
                }

                else if (obj.tag == "Barricade") {
                    obj.GetComponent<Barricade>().onDamaged(zombie.attackDamage);
                }

                yield return new WaitForSeconds(zombie.attackSpeed);
            }
        }
    }
}
