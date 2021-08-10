using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace haruroad.szd.multiplayer {
    public class PlayerHP : MonoBehaviourPun, IPunObservable {
        private int attackDamage;
        private Animator ani;
        private Image hpBar;

        public float health = 100.0f;
        public bool isDead = false;
        public bool isInvis = false;

        void Start() {
            Reset_();
        }

        void Update() {
            photonView.RPC("HPCheck", RpcTarget.AllBuffered);

            if (photonView.IsMine == true) {
                hpBar.fillAmount = health / 100;
            }
        }

        private void Reset_() {
            health = 100.0f;
            ani = GetComponent<Animator>();
            if (photonView.IsMine) {
                hpBar = GameObject.Find("Hp_Bar").GetComponent<Image>();
            }
        }

        public void playDamagedAnimation() {
            ani.SetTrigger("Damage");
        }


        public void onDamaged(float damage) {
            photonView.RPC("SetDamage", RpcTarget.AllBuffered, damage);
            playDamagedAnimation();
        }

        [PunRPC]
        private void SetHealth(float health) {
            if (photonView.IsMine == true) {
                this.health = health;
            }
        }

        [PunRPC]
        private void SetDamage(float damage) {
            if (!isInvis && photonView.IsMine == true) {
                health -= damage;
            }
        }

        [PunRPC]
        public void HPCheck() {
            if (health <= 0) {
                if (ani != null) {
                    ani.SetBool("Dead", true);
                }

                if (photonView.IsMine == true) {
                    isDead = true;
                }
            }
        }
        public void NextRound() {
            if (photonView.IsMine == true) {
                photonView.RPC("SetHealth", RpcTarget.AllBuffered, 100.0f);

                if (isDead == true) {
                    isDead = false;
                    ani.SetBool("Dead", false);
                }

            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(isDead);
                stream.SendNext(isInvis);
            }
            else {
                isDead = (bool) stream.ReceiveNext();
                isInvis = (bool) stream.ReceiveNext();
            }
        }
    }
}