using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace haruroad.szd.multiplayer {
    public class PlayerGun : MonoBehaviour {
        public Transform gunPivot;
        public Transform leftHandPos;
        public Transform rightHandPos;

        private Animator plaAni;

        // Start is called before the first frame update
        void Start() {
            plaAni = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {

        }
        private void OnAnimatorIK(int layerIndex) {
            gunPivot.position = plaAni.GetIKHintPosition(AvatarIKHint.RightElbow);

            // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            plaAni.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            plaAni.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            plaAni.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);
            plaAni.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);

            // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            plaAni.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            plaAni.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            plaAni.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.position);
            plaAni.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.rotation);
        }
    }
}