using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public int hp = 100;
    private Transform myTr;
    private Transform playerTr;
    public bool isOk = false;
    public ParticleSystem explosion;
    public bool remove = false;

    // Start is called before the first frame update
    void Start()
    {
        myTr = GetComponent<Transform>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        PositionChack();
        DamageChack();
    }
    private void PositionChack()
    {
        if (isOk == false)
        {
            myTr.position = playerTr.position;
            myTr.rotation = playerTr.rotation;
        }
    }
    private void DamageChack()
    {
        if (hp <= 0 && remove == false)
        {
            StartCoroutine("Destory");
            remove = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("[OTEnter()] other.tag: " + other.tag);
    }

    private void OnTriggerExit(Collider other) {
        Debug.LogWarning("[OTExit()] other.tag: " + other.tag);
    }

    IEnumerator Destory()
    {
        explosion.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
       
    }

}
