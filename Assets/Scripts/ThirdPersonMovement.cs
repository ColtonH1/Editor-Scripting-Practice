using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public int walkingSpeed;
    public int runningSpeed;
    private int originalWalkingSpeed;
    private Vector3 playerVelocity;
    private float gravity = -9.81f*2;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    //private float ySpeed;
    public int jumpHeight;

    private PlayerData playerData;
    private Player player;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        playerData = player.playerData;
        if(!playerData.canWalk || playerData.walkSpeed == 0)
        {
            Debug.LogError("Player was not set to be allowed to walk or walk speed is equal to zero");
        }
        walkingSpeed = playerData.walkSpeed;
        runningSpeed = playerData.runSpeed;
        jumpHeight = playerData.jumpHeight;
        originalWalkingSpeed = walkingSpeed;
        //direction = Vector3.zero;
    }

    void Update()
    {
        if (playerData.canRun)
            Running();
        Walking();


    }

    private void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkingSpeed = runningSpeed;
        }
        else
        {
            walkingSpeed = originalWalkingSpeed;
        }
    }

    private void Walking()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && playerVelocity.y < 0)
        {
            Debug.Log("reset velocity");
            playerVelocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        /*
        if (controller.isGrounded && playerVelocity.y < 0.1f)
        {
            playerVelocity.y = 0f;
        }*/

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * walkingSpeed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Is jumping");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log(playerVelocity.y);
        }

        playerVelocity.y = gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        /*float magnitude = Mathf.Clamp01(direction.magnitude) * walkingSpeed;
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            //ySpeed = jumpHeight;
            controller.Move()
            direction.y = jumpHeight;
        }
        direction.y -= 9.81f;
        //Vector3 velocity = direction * direction.magnitude;
        //velocity.y = ySpeed;

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jumped");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Debug.Log("Player Velocity is: " + playerVelocity.y);*/
    }
}
