using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks {
    private string gameVer = "0.1";
    private Text debugText;
    private int maxCount = 2;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinServer() {
        if (PhotonNetwork.IsConnected == true) {
            PhotonNetwork.JoinLobby();
        }
        else {
            PhotonNetwork.GameVersion = gameVer;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.LogError("마스터 클라이언트가 서버에 연결");
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new RoomOptions { MaxPlayers = (byte) maxCount }, TypedLobby.Default);

    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.LogError("연결 종료" + cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom("GameRoom", new RoomOptions { MaxPlayers = (byte) maxCount });
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        ClientCheck();
    }

    public override void OnJoinedLobby() {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = (byte) maxCount }, TypedLobby.Default);
    }

    private void ClientCheck() {
        Debug.LogError(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GamePlayScene");
    }
}