using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController Player;
    //[SerializeField] private Transform playerCamera = null;
    public UIManager UIManager;

    public float health;
    public float walk = 4f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private float speed;

    public Transform groundCheck;
    private Vector3 moveDirection;
    float groundDistance = 0.4f;


    Vector3 velocity;
    public bool isGrounded;
    
    //[Range(0, 1.0f)]
    //[SerializeField] private float crouchSpeed = 0.3f;
    //private float standHeight;
    //private float crouchHeight;
    //private bool isCrouching;
    



    
    void Start()
    {
        Player = GetComponent<CharacterController>();

        //isCrouching = false;
        //standHeight = Player.height;





    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Death();
        }
    }
    void Update()
    {


        //isCrouching = Input.GetKey(KeyCode.C);
        Move_And_Jump();
        WeaponEquiped();

    }
    void WeaponEquiped()
    {

    }
    //void FixedUpdate()
    //{
    //    var desiredHeight = isCrouching ? crouchHeight : standHeight;

    //    if (Player.height != desiredHeight)
    //    {
    //        AdjustHeight(desiredHeight);
    //        var camPos = playerCamera.transform.position;
    //        camPos.y = Player.height;

    //        playerCamera.transform.position = camPos;
    //    }

    //}
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

    //private void AdjustHeight(float height)
    //{
    //    float center = height / 2;
    //    Player.height = Mathf.Lerp(Player.height, height, crouchSpeed);
    //    Player.center = Vector3.Lerp(Player.center, new Vector3(0, center, 0), crouchSpeed);
    //}
    public void Damage()
    {

    }
    public void Death()
    {
        Debug.Log("Ded");
        Time.timeScale = 0;
        UIManager.GameOver.enabled = true;
        
    }


}



