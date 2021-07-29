using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerHP : MonoBehaviourPun {
    private int attackDamage;
    private Animator ani;
    public float health = 100.0f;
    public bool isDead = false;

    void Start() {
        Reset();
    }

    void Update() {
        HPCheck();
        photonView.RPC("HpCheck", RpcTarget.AllBuffered);
    }

    private void Reset() {
        health = 100.0f;
        ani = GetComponent<Animator>();
    }

    public void playDamagedAnimation() {
        ani.SetTrigger("Damage");
    }

  
    public void onDamaged(float damage) {
        photonView.RPC("SetHealth", RpcTarget.AllBuffered,damage);
        playDamagedAnimation();
    }

    [PunRPC]
    private void SetHealth(float damage) {
        if (photonView.IsMine == true)
        {
            health -= damage;
        }
    }
    [PunRPC]
    public void HPCheck()
    {
        if (health <= 0 )
        {
            ani.SetBool("Dead",true);
            if (photonView.IsMine == true)
            {
                isDead = true;
            }
        }
    }
}
