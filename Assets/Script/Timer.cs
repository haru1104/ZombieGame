using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Timer : MonoBehaviourPunCallbacks, IPunObservable
{
    GameManager manager;
    Text text;
    private int time = 60;

    // Start is called before the first frame update

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient == true && PhotonNetwork.IsConnected)
        {
            FindCheck();
            if (manager.isRestTime == true)
            {
                time = 60;
                StartCoroutine("TimeDecrease");
            }
            else
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeCheck();
    }
    private void FindCheck()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = GetComponent<Text>();
    }
    private void TimeCheck()
    {
        text.text = "Break Time : " + time;
    }
    IEnumerator TimeDecrease()
    {

        yield return new WaitForSeconds(1);
        time--;
        StartCoroutine("TimeDecrease");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(time);
        }
        else
        {
            time = (int)stream.ReceiveNext();
        }
    }
}
