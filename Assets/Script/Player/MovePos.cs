using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePos : MonoBehaviour
{
    private Animator playerAni;
    private Rigidbody playerRigid;
    private Vector3 movePos;
    private float h, v;
    private float TurnSpeed = 2;
    private float moveSpeed;
    private float jumpPos;
    private bool isJump;
    private bool isWalk;


    // Start is called before the first frame update
    void Start()
    {
        StartReset();
    }

    // Update is called once per frame
    void Update()
    {
        MovePosSet();
    }
    private void StartReset()
    {
        playerRigid = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
        moveSpeed = 10.0f;
        jumpPos = 5;
        isJump = false;
        isWalk = false;

    }
    private void MovePosSet()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        movePos = v*transform.forward;
        movePos = movePos.normalized * moveSpeed * Time.deltaTime;
        playerRigid.MovePosition(transform.position + movePos);
        playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(0, h * TurnSpeed, 0);
        if (h == 0 && v== 0)
        {
            playerAni.SetBool("Walk", false);
            isWalk = false;
        }
        else
        {
            isWalk = true;
            playerAni.SetBool("Walk", true);
        }

    }


}
