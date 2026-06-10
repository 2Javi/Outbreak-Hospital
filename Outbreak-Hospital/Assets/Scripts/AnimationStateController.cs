using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

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
    private PlayerInputActions inputActions;

    public bool isCrouch;
    public bool isRunning;
    public bool isWalking;

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

        inputActions = new PlayerInputActions();
        inputActions.Enable();

        inputActions.Player.Crouch.performed += _ => ToggleCrouch();
    }

    void Update()
    {
        isRunning = animator.GetBool(isRunningHash);
        isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isStrafingLeft = animator.GetBool(isStrafingLeftHash);
        bool isStrafingRight = animator.GetBool(isStrafingRightHash);
        bool isAiming = animator.GetBool(isAimingHash);

        bool forwardPressed = inputActions.Player.WalkForward.IsPressed();
        bool backPressed = inputActions.Player.MoveBack.IsPressed();
        bool leftPressed = inputActions.Player.StrafeLeft.IsPressed();
        bool rightPressed = inputActions.Player.StrafeRight.IsPressed();
        bool runPressed = inputActions.Player.Run.IsPressed();
        bool aimPressed = Input.GetMouseButton(1);

        if (!isWalking && forwardPressed)
            animator.SetBool(isWalkingHash, true);
        if (isWalking && !forwardPressed)
            animator.SetBool(isWalkingHash, false);

        if (!isWalkingBack && backPressed)
            animator.SetBool(isWalkingBackHash, true);
        if (isWalkingBack && !backPressed)
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
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }

    void ToggleCrouch()
    {
        isCrouch = !isCrouch;
        animator.SetBool(isCruchHash, isCrouch);
    }
}