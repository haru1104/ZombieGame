using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
   private GameObject go;
   private GameObject player;
   public GameObject barricadePrefab;
   public GameObject barrelPrefab;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

    }
    public void Barricade()
    {
        go = Instantiate(barricadePrefab, player.transform.position, Quaternion.identity);
    }
    public void Barrel()
    {
        go = Instantiate(barrelPrefab,player.transform.position,Quaternion.identity);
    }
    public void IsInstall()
    {
        if (go != null)
        {
            if (go.tag == "Barricade")
            {
                go.GetComponent<Barricade>().isSetted = true;
                go = null;
            }
            if (go.tag == "Barrel")
            {
                go.GetComponent<Barrel>().isSetted = true;
                go = null;
            }
        }
    }
    public void IsCancel()
    {
        if (go != null)
        {
            Destroy(go.gameObject);
            go = null;
        }
    }
}
