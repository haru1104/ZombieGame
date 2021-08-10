using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace haruroad.szd.singleplayer
{
    public class PlayerHP : MonoBehaviour
    {
        private int attackDamage;
        private Animator ani;

        public float health = 100.0f;

        void Start()
        {
            Reset();
        }

        void Update()
        {

        }
        private void Reset()
        {
            health = 100.0f;
            ani = GetComponent<Animator>();
        }

        public void playDamagedAnimation()
        {
            ani.SetTrigger("Damage");
        }

        public void onDamaged(float damage)
        {
            health -= damage;
            playDamagedAnimation();
        }

    }
}