using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class Barrel : MonoBehaviourPun, IPunObservable {
        private Transform playerTr;
        private SoundManager sound;
        private List<Zombie> zombies = new List<Zombie>();

        private bool isSetted = false;
        private bool isDestory = false;
        public int PurchaseCost = 700;
        public ParticleSystem explosion;

        public float health = 100.0f;

        void Start() {
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        }

        void Update() {
            PositionCheck();
            DamageCheck();
        }
        private void PositionCheck() {
            FindPlayer(GameManager.viewId);

            if (!isSetted) {
                transform.position = playerTr.position;
                transform.rotation = playerTr.rotation;
            }
        }
        private void DamageCheck() {
            if (health <= 0 && isSetted && !isDestory) {
                isDestory = true;
                StartCoroutine("Destory");
            }
        }

        public void onDamaged(float damage) {
            health -= damage;
            sound.BarrelHitSound();
        }

        public void SetPosition(Vector3 position, Quaternion rotation) {
            transform.position = position;
            transform.rotation = rotation;

            photonView.RPC("SetPositionRpc", RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void SetPositionRpc() {
            isSetted = true;
            GetComponent<CapsuleCollider>().isTrigger = false;
        }

        private void OnTriggerEnter(Collider other) {
            Zombie zombie = other.gameObject.transform.root.GetComponent<Zombie>();

            if (other.tag == "Zombie" && !zombies.Contains(zombie)) {
                zombies.Add(zombie);
            }

        }
        private void OnTriggerExit(Collider other) {
            Zombie zombie = other.gameObject.transform.root.GetComponent<Zombie>();

            if (other.tag == "Zombie" && zombies.Contains(zombie)) {
                zombies.Remove(zombie);
            }
        }

        IEnumerator Destory() {
            explosion.Play();
            sound.BombSoundPlay();

            for (int i = 0; i < zombies.Count; i++) {
                zombies[i].onDamaged(100);
            }

            yield return new WaitForSeconds(1.0f);
            PhotonNetwork.Destroy(gameObject);
        }

        private void FindPlayer(int playerId) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++) {
                if (players[i].GetComponent<PhotonView>().ViewID == playerId) {
                    playerTr = players[i].transform;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(health);
            }
            else {
                health = (float) stream.ReceiveNext();
            }
        }
    }
}