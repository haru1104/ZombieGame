using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace haruroad.szd.multiplayer {
    public class MovePos : MonoBehaviourPunCallbacks {
        private Animator playerAni;
        private Rigidbody playerRigid;
        private Vector3 movePos;

        private PlayerHP hp;

        private float h, v;
        private float TurnSpeed = 2;
        
        [SerializeField]
        private float moveSpeed = 12f;

        void Start() {
            StartReset();
        }

        void Update() {
            MovePosSet();

        }
        private void StartReset() {
            playerRigid = GetComponent<Rigidbody>();
            playerAni = GetComponent<Animator>();
            hp = GetComponent<PlayerHP>();

        }
        private void MovePosSet() {
            if (!photonView.IsMine && PhotonNetwork.IsConnected || hp.health <= 0) {
                return;
            }

            // h = Input.GetAxis("Horizontal");
            // v = Input.GetAxis("Vertical");

            movePos = v * transform.forward;
            movePos = movePos.normalized * moveSpeed * Time.deltaTime;

            playerRigid.MovePosition(transform.position + movePos);
            playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(0, h * TurnSpeed, 0);

            if (h == 0 && v == 0) {
                playerAni.SetBool("Walk", false);
            }
            else {
                playerAni.SetBool("Walk", true);
            }

        }

        public void OnStickChanged(Vector3 stickPos) {
            h = stickPos.x;
            v = stickPos.y;
        }

    }
}