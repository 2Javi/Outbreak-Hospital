using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isStrafingLeftHash;
    int isStrafingRightHash;
    int isWalkingBackHash;
    int isAimingHash;
    int aimingLayerIndex;
    public Rig playerRig;
    int isCruchHash;

    void Start()
    {

        animator = GetComponent<Animator>();
        aimingLayerIndex = animator.GetLayerIndex("Aiming");

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isStrafingLeftHash = Animator.StringToHash("isStrafingLeft");
        isStrafingRightHash = Animator.StringToHash("isStrafingRight");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        isAimingHash = Animator.StringToHash("isAiming");
        isCruchHash = Animator.StringToHash("isCrouch");
    }

    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isStrafingLeft = animator.GetBool(isStrafingLeftHash);
        bool isStrafingRight = animator.GetBool(isStrafingRightHash);
        bool isAiming = animator.GetBool(isAimingHash);
        bool isCrouch = animator.GetBool(isCruchHash);

        bool forwardPressed = Input.GetKey("w");
        bool backPress = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        bool aimPressed = Input.GetMouseButton(1);
        bool crouchPressed = Input.GetKeyDown("c");




        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
            Debug.Log("isCrouch: " + animator.GetBool(isCruchHash) + " isWalking: " + animator.GetBool(isWalkingHash));
        }


        if (isWalking && !forwardPressed)
            animator.SetBool(isWalkingHash, false);

        if (!isWalkingBack && backPress)
            animator.SetBool(isWalkingBackHash, true);
        if (isWalkingBack && !backPress)
            animator.SetBool(isWalkingBackHash, false);

        if (!isRunning && (forwardPressed && runPressed))
            animator.SetBool(isRunningHash, true);

        if (isRunning && (!forwardPressed || !runPressed))
            animator.SetBool(isRunningHash, false);

        if (!isStrafingLeft && leftPressed)
            animator.SetBool(isStrafingLeftHash, true);

        if (isStrafingLeft && !leftPressed)
            animator.SetBool(isStrafingLeftHash, false);

        if (!isStrafingRight && rightPressed)
            animator.SetBool(isStrafingRightHash, true);

        if (isStrafingRight && !rightPressed)
            animator.SetBool(isStrafingRightHash, false);

        if (!isAiming && aimPressed)
        {
            animator.SetLayerWeight(aimingLayerIndex, 1);
            playerRig.weight = 1f;
            animator.SetBool(isAimingHash, true);
        }

        if (isAiming && !aimPressed)
        {
            animator.SetLayerWeight(aimingLayerIndex, 0);
            playerRig.weight = 0f;
            animator.SetBool(isAimingHash, false);
        }

        if (Input.GetKeyDown("c"))
        {
            // Debug.Log("before: " + isCrouch);
            isCrouch = !isCrouch;
            // Debug.Log("after: " + isCrouch);
            animator.SetBool(isCruchHash, isCrouch);
            Debug.Log("animator says: " + animator.GetBool(isCruchHash));
            Debug.Log("isCrouch: " + animator.GetBool(isCruchHash) + " isWalking: " + animator.GetBool(isWalkingHash));
        }




    }
}