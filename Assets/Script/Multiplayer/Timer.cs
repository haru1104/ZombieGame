using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace haruroad.szd.multiplayer {
    public class Timer : MonoBehaviourPunCallbacks, IPunObservable {
        GameManager manager;
        Text text;

        private int time = 60;

        public override void OnEnable() {
            StartCoroutine("WaitForFrame");
        }

        public override void OnDisable() {
            Debug.Log("타이머 오브젝트 비활성화됨. (마스터 : " + PhotonNetwork.IsMasterClient + ")");
        }

        void Update() {
            TimeCheck();
        }

        private void FindCheck() {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            text = GetComponent<Text>();
        }

        private void TimeCheck() {
            if (text != null) {
                text.text = "Break Time : " + time;
            }
        }

        IEnumerator WaitForFrame() {
            yield return new WaitForEndOfFrame();

            Debug.Log("타이머 오브젝트 활성화됨. (마스터 : " + PhotonNetwork.IsMasterClient + ")");
            FindCheck();

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) {
                if (manager.isRestTime == true) {
                    time = 60;
                    StartCoroutine("TimeDecrease");
                }
            }
        }

        IEnumerator TimeDecrease() {
            yield return new WaitForSeconds(1f);
            time--;
            StartCoroutine("TimeDecrease");
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(time);
            }
            else {
                time = (int) stream.ReceiveNext();
            }
        }
    }

}