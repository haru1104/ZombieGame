using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public enum DebugType {
    Default, Photon
}

[System.Serializable]
public class ZombiePrefabs {
    public GameObject lite;
    public GameObject normal;
    public GameObject heavy;
}

public class DebugManager : MonoBehaviour {
    public DebugType type;

    public ZombiePrefabs zombie;

    private UiManager uiManager;
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("GamePlayUi").GetComponent<UiManager>();
    }

    void OnGUI() {
        int allBtn = 190;
        string typeName = type == DebugType.Default ? "기본" : "포톤";

        GUI.BeginGroup(new Rect(20, 20, 300, 500));

        if (PhotonNetwork.IsMasterClient) {
            GUI.Box(new Rect(0, 0, 300, 500), "게임 디버그 메뉴 (" + typeName + ")");

            if (type == DebugType.Photon) {
                photonGUI();
            }
            else if (type == DebugType.Default) {
                defaultGUI();
            }

            allBtn = 190;
        }
        else {
            allBtn = 40;
        }

        if (GUI.Button(new Rect(30, allBtn, 230, 40), "라운드 상태 전환")) {
            uiManager.istimeCheck = !uiManager.istimeCheck;
            Debug.LogWarning("[Debug:All] 플레이어의 라운드 대기 상태를 " + uiManager.istimeCheck + "(으)로 변경하였습니다.");
        }

        if (GUI.Button(new Rect(30, allBtn + 50, 230, 40), "돈 추가")) {
            gameManager.addMoney(1000);
            Debug.LogWarning("[Debug:All] 플레이어들의 돈을 1000만큼 추가하여 현재 " + gameManager.money + "원이 되었습니다.");
        }

        GUI.EndGroup();
    }

    private void defaultGUI() {
        if (GUI.Button(new Rect(30, 40, 230, 40), "일반 좀비 스폰")) {
            Debug.LogWarning("[Debug:GameObject] 일반 좀비를 강제로 스폰합니다.");
            Instantiate(zombie.normal, transform.position, Quaternion.identity);
        }

        if (GUI.Button(new Rect(30, 90, 230, 40), "라이트 좀비 스폰")) {
            Debug.LogWarning("[Debug:GameObject] 라이트 좀비를 강제로 스폰합니다.");
            Instantiate(zombie.lite, transform.position, Quaternion.identity);
        }

        if (GUI.Button(new Rect(30, 140, 230, 40), "헤비 좀비 스폰")) {
            Debug.LogWarning("[Debug:GameObject] 헤비 좀비를 강제로 스폰합니다.");
            Instantiate(zombie.heavy, transform.position, Quaternion.identity);
        }
    }

    private void photonGUI() {
        if (GUI.Button(new Rect(30, 40, 230, 40), "일반 좀비 스폰")) {
            Debug.LogWarning("[Debug:Photon] 일반 좀비를 강제로 스폰합니다.");
            PhotonNetwork.Instantiate("Normal Zombie", transform.position, Quaternion.identity);
        }

        if (GUI.Button(new Rect(30, 90, 230, 40), "라이트 좀비 스폰")) {
            Debug.LogWarning("[Debug:Photon] 라이트 좀비를 강제로 스폰합니다.");
            PhotonNetwork.Instantiate("Lite Zombie", transform.position, Quaternion.identity);
        }

        if (GUI.Button(new Rect(30, 140, 230, 40), "헤비 좀비 스폰")) {
            Debug.LogWarning("[Debug:Photon] 헤비 좀비를 강제로 스폰합니다.");
            PhotonNetwork.Instantiate("Heavy Zombie", transform.position, Quaternion.identity);
        }
    }
}
