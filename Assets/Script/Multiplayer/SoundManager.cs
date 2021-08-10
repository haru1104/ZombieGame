using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SoundManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip buttonClick;
        public AudioClip moneySound;
        public AudioClip zombieDeadSound;
        public AudioClip startZombieSound_1;
        public AudioClip startZombieSound_2;
        public AudioClip startZombieSound_3;
        public AudioClip barricadeDamage;
        public AudioClip bombSound;

        public AudioClip gunShot;
        public AudioClip gunReload;
        public AudioClip barrelHit;


        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void BombSoundPlay()
        {
            audioSource.PlayOneShot(bombSound);
        }
        public void OnClickButtonSound()
        {
            audioSource.PlayOneShot(buttonClick);
        }
        public void ZombidDeadSound()
        {
            audioSource.PlayOneShot(zombieDeadSound);
        }
        public void OnClickShopSpawnButton()
        {
            audioSource.PlayOneShot(moneySound);
        }
        public void BarricadeDamage()
        {
            audioSource.PlayOneShot(barricadeDamage);
        }
        public void RoundStartSound()
        {
            int num = Random.Range(1, 3);
            if (num == 1)
            {
                audioSource.PlayOneShot(startZombieSound_1);
            }
            else if (num == 2)
            {
                audioSource.PlayOneShot(startZombieSound_2);
            }
            else
            {
                audioSource.PlayOneShot(startZombieSound_3);
            }
        }
        public void GunShotSound()
        {
            audioSource.PlayOneShot(gunShot);
        }

        public void GunReloadSound()
        {
            audioSource.PlayOneShot(gunReload);
        }

        public void BarrelHitSound()
        {
            audioSource.PlayOneShot(barrelHit);
        }
    }
