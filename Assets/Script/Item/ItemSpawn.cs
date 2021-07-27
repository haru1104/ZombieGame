using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
   private GameObject obstacle;
   private GameObject player;

    private void Start()
    {
        
    }

    public void Barricade()
    {
        FindPlayer();
        obstacle = PhotonNetwork.Instantiate("Barricade", player.transform.position, Quaternion.identity);
    }

    public void Barrel()
    {
        FindPlayer();
        obstacle = PhotonNetwork.Instantiate("Barrel", player.transform.position, Quaternion.identity);
    }

    public void IsInstall()
    {
        if (obstacle != null)
        {
            if (obstacle.tag == "Barricade")
            {
                obstacle.GetComponent<Barricade>().SetPosition();
                
            }
            if (obstacle.tag == "Barrel")
            {
                obstacle.GetComponent<Barrel>().SetPosition();
             
            }
        }
    }

    public void IsCancel()
    {
        if (obstacle != null)
        {
            Destroy(obstacle.gameObject);
            obstacle = null;
        }
    }

    private void FindPlayer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            if (players[i].GetComponent<PhotonView>().IsMine) {
                player = players[i];
            }
        }
    }
}
