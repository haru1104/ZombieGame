using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class ItemSpawn : MonoBehaviourPun {
        private GameObject obstacle;
        private GameObject player;
        private GameManager gm;
        public GameObject barricade;
        public GameObject barrel;

        private void Start() {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void Barricade() {

            FindPlayer(GameManager.viewId);
            obstacle = Instantiate(barricade, player.transform.position, Quaternion.identity);
        }

        public void Barrel() {
            FindPlayer(GameManager.viewId);
            obstacle = Instantiate(barrel, player.transform.position, Quaternion.identity);
        }

        public void IsInstall() {
            if (obstacle != null) {
                Destroy(obstacle);
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                if (obstacle.tag == "Barricade") {
                    obstacle = PhotonNetwork.Instantiate("Barricade", player.transform.position, Quaternion.identity);

                    for (int i = 0; i < players.Length; i++) {
                        if (players[i].GetPhotonView().IsMine && obstacle.GetPhotonView().IsMine) {
                            player = players[i];

                            obstacle.GetComponent<Barricade>().SetPosition(player.transform.position, player.transform.rotation);
                            gm.removeMoney(500);
                            break;
                        }
                    }

                }
                if (obstacle.tag == "Barrel") {
                    obstacle = PhotonNetwork.Instantiate("Barrel", player.transform.position, Quaternion.identity);

                    for (int i = 0; i < players.Length; i++) {
                        if (players[i].GetPhotonView().IsMine && obstacle.GetPhotonView().IsMine) {
                            player = players[i];

                            obstacle.GetComponent<Barrel>().SetPosition(player.transform.position, player.transform.rotation);
                            gm.removeMoney(700);
                            break;
                        }
                    }
                }
            }
        }

        public void IsCancel() {
            if (obstacle != null) {
                Destroy(obstacle.gameObject);
                obstacle = null;
            }
        }

        private void FindPlayer(int playerId) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetComponent<PhotonView>().ViewID == playerId) {
                    player = players[i];

                    break;
                }
            }
        }
    }
}