using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace haruroad.szd.singleplayer
{
    public class Barricade : MonoBehaviour
    {
        private Transform playerTr;
        private Transform barricadeTr;
        private Vector3 moveTr;
        private SoundManager sound;

        private bool isDestory = false;

        public float health = 100.0f;

        public bool isSetted = false;

        void Start()
        {
            playerTr = GameObject.Find("Player").GetComponent<Transform>();
            barricadeTr = GetComponent<Transform>();
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        }

        void Update()
        {
            MoveSet();
            DamageCheck();
        }

        private void MoveSet()
        {
            if (!isSetted)
            {
                moveTr = playerTr.position;
                barricadeTr.position = moveTr;
                barricadeTr.rotation = playerTr.rotation;
            }
        }

        private void DamageCheck()
        {
            if (health <= 0 && isSetted && !isDestory)
            {
                isDestory = true;
                Destroy(gameObject);
            }
        }

        public void onDamaged(float damage)
        {
            sound.BarricadeDamage();
            health -= damage;
        }
    }
}
