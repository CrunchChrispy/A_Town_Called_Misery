using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private bool isCrouching;
    CharacterController Player;

    public float walk = 4f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    private float speed;
    private Vector3 moveDirection;

    float groundDistance = 0.4f;
    bool isGrounded;

    Vector3 velocity;

    //[SerializeField] private MoveSettings moveSettings = null;
    [SerializeField] private Transform playerCamera = null;
    [Range(0, 1.0f)]
    [SerializeField] private float crouchSpeed = 0.3f;
    private float standHeight;
    private float crouchHeight;

    void Start()
    {
        Player = GetComponent<CharacterController>();
        isCrouching = false;
        standHeight = Player.height;

    }
    
    void Update()
    {

        isCrouching = Input.GetKey(KeyCode.C);
        Move_And_Jump();
     

    }
    void FixedUpdate()
    {
        var desiredHeight = isCrouching ? crouchHeight : standHeight;

        if (Player.height != desiredHeight)
        {
            AdjustHeight(desiredHeight);
            var camPos = playerCamera.transform.position;
            camPos.y = Player.height;

            playerCamera.transform.position = camPos;
        }

    }
    void Move_And_Jump()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        Player.Move(move * walk * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        Player.Move(velocity * Time.deltaTime);

    }

    private void AdjustHeight(float height)
    {
        float center = height / 2;
        Player.height = Mathf.Lerp(Player.height, height, crouchSpeed);
        Player.center = Vector3.Lerp(Player.center, new Vector3(0, center, 0), crouchSpeed);
    }


}



