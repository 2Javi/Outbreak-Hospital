using System;
using Unity.Mathematics;
using UnityEngine;

public class MovementStateManager : MonoBehaviour

{
    public float walkSpeed = 1.5f;
    public float runSpeed = 2.5f;
    public float crouchSpeed = 0.5f;
    public float noiseLevel = 0f;
    public float currentSpeed = 0f;
    [HideInInspector] public Vector3 dir;
    float horizontal_input, vertical_input;
    CharacterController controller;
    [SerializeField] float groundYOffSet;
    Vector3 spherePoss;

    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    AnimationStateController animationStateControllerReference;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animationStateControllerReference = GetComponent<AnimationStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        getDirectionAndMove();
        Gravity();
    }

    void getDirectionAndMove()
    {

        if (animationStateControllerReference.isCrouch)
        {
            currentSpeed = crouchSpeed;
            noiseLevel = 1f;
        }
        else if (animationStateControllerReference.isRunning)
        {
            currentSpeed = runSpeed;
            noiseLevel = 3f;
        }
        else if (animationStateControllerReference.isWalking)
        {
            currentSpeed = walkSpeed;
            noiseLevel = 2f;
        }
        else
        {
            noiseLevel = 0f;
        }

        horizontal_input = Input.GetAxis("Horizontal");
        vertical_input = Input.GetAxis("Vertical");

        dir = transform.forward * vertical_input + transform.right * horizontal_input;

        controller.Move(dir * currentSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePoss = new Vector3(transform.position.x, transform.position.y - groundYOffSet, transform.position.z);
        if (Physics.CheckSphere(spherePoss, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePoss, controller.radius - 0.05f);
    }




}
