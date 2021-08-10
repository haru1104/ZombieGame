using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace haruroad.szd.singleplayer
{
    public class Barricade : MonoBehaviour
    {
        private Transform playerTr;
        private Vector3 moveTr;
        private SoundManager sound;

        public bool isSetted = false;
        private bool isDestory = false;
        public int PurchaseCost = 500;
        public float health = 100.0f;

        void Start()
        {
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
            playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
                transform.position = moveTr;
                transform.rotation = playerTr.rotation;
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

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

  
        private void SetPositionRpc()
        {
            isSetted = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }

       
    }
}
