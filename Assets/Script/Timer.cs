using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks, IPunObservable {
    GameManager manager;
    Text text;
    private int time = 60;

    public override void OnEnable() {
        FindCheck();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) {
            if (manager.isRestTime == true) {
                time = 60;
                StartCoroutine("TimeDecrease");
            }
        }
    }

    void Update() {
        TimeCheck();
    }

    private void FindCheck() {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = GetComponent<Text>();
    }

    private void TimeCheck() {
        text.text = "Break Time : " + time;
    }

    IEnumerator TimeDecrease() {
        yield return new WaitForSeconds(1);
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
