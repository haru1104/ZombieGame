using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks
{
    private string gameVer = "0.1";
    private Text debugText;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 포톤에서 자동으로 로딩된 씬을 동기화 해주는 프로퍼티
    }
    public void JoinServer()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVer;
            PhotonNetwork.ConnectUsingSettings();//위에서 설정한 게임버전을 사용해서 포톤 네트워크에 접속을 하겠다는 이야기
        }

    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();//마스터 클라이언트를우선으로 찾고 만약에 마스터 클라이언트가 없으면 자신이 방을 만들게 된다
        Debug.LogError("마스터 클라이언트가 서버에 연결");
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("연결 종료" + cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom("GameRoom", new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()// 방에 접속을 성공했을때 실행되는 메소드
    {
        base.OnJoinedRoom();
        ClientChack();
    }
    public override void OnJoinedLobby()//로비(포톤에서의 로비는 단일생성가능.)
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }
    private void ClientChack()
    {
        //마스터 클라이언트가 방에 접속을하면 레벨을 로딩을 하는데 이 레벨이 로딩을 한 상태가 서버에 올라가고
        //클라이언트는 해당 서버로 방에 접속을 하면 그 상태를 받아와서 알아서 마스터클라이언트가 들어가있는 씬에 로딩을 시켜줌 

        StartCoroutine("LoadingScene");

    }
    IEnumerator LoadingScene()
    {
        Debug.LogError("접속완료 3초뒤 로비로 이동합니다.");
        Debug.LogError(PhotonNetwork.CurrentRoom.Name);
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LoadLevel("GamePlayScene");

    }
}  
