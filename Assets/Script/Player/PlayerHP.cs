using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float playerHP;
    private int attackDamage;
    private Animator ani;


    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.LogError("플레이어의 현재 데미지 : "+playerHP);
        
    }
    private void Reset()
    {
        playerHP = 100f;
        ani = GetComponent<Animator>();
    }
    public void DamageAni()
    {
        ani.SetTrigger("Damage");
    }

}
