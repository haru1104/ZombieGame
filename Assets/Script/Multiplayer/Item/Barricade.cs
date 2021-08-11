using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class Barricade : MonoBehaviourPun {
        private Transform playerTr;
        private Vector3 moveTr;
        private SoundManager sound;

        private bool isSetted = false;
        private bool isDestory = false;
        public int PurchaseCost = 500;
        public float health = 100.0f;

        void Start() {
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        }

        void Update() {
            MoveSet();
            DamageCheck();
        }

        private void MoveSet() {
            FindPlayer(GameManager.viewId);

            if (!isSetted) {
                moveTr = playerTr.position;
                transform.position = moveTr;
                transform.rotation = playerTr.rotation;
            }
        }

        private void DamageCheck() {
            if (health <= 0 && isSetted && !isDestory) {
                isDestory = true;
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void onDamaged(float damage) {
            sound.BarricadeDamage();
            health -= damage;
        }

        public void SetPosition(Vector3 position, Quaternion rotation) {
            transform.position = position;
            transform.rotation = rotation;

            photonView.RPC("SetPositionRpc", RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void SetPositionRpc() {
            isSetted = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }

        private void FindPlayer(int playerId) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetComponent<PhotonView>().ViewID == playerId) {
                    playerTr = players[i].transform;
                }
            }
        }
    }
}