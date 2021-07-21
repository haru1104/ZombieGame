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
  

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnClickButtonSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
}
