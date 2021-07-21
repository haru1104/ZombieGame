using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {
    private Transform myTr;
    private Transform playerTr;
    
    private List<Zombie> zombies = new List<Zombie>();

    public ParticleSystem explosion;

    public float health = 100.0f;
    public bool isSetted = false;

    void Start() {
        myTr = GetComponent<Transform>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();

    }

    void Update() {
        PositionCheck();
        DamageCheck();
    }
    private void PositionCheck() {
        if (isSetted == false) {
            myTr.position = playerTr.position;
            myTr.rotation = playerTr.rotation;
        }
    }
    private void DamageCheck() {
        if (health <= 0 && isSetted) {
            StartCoroutine("Destory");
        }
    }

    public void onDamaged(float damage) {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Zombie") {
            zombies.Add(other.gameObject.GetComponent<Zombie>());
           
            Debug.LogError(other.tag);
        }

    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Zombie"){
            zombies.Remove(other.gameObject.GetComponent<Zombie>());
        }
    }
    IEnumerator Destory() {
        explosion.Play();

        for (int i = 0; i < zombies.Count; i++) {
            zombies[i].onDamaged(100);
        }

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}