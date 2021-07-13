using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform gunPivot;
    public Transform leftHandPos;
    public Transform rightHandPos;

    private Animator plaAni;

    // Start is called before the first frame update
    void Start()
    {
        plaAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = plaAni.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ������ �����̿� �����
        plaAni.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        plaAni.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        plaAni.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);
        plaAni.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);

        // IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� ������ �����̿� �����
        plaAni.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        plaAni.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
      
        plaAni.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.position);
        plaAni.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.rotation);
    }
}
