using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Camera topDownCam;
    public Camera firstPersonCam;
    public Camera thirdPersonCam;

    private Animator playerAnimator;
    private int state;
    private float sprint;
    private Rigidbody rb;
    private float time;
    private float jump_wait;
    private float jump_time;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();

        firstPersonCam.enabled = true;
        thirdPersonCam.enabled = false;
        topDownCam.enabled = false;
        sprint = 0.0f;
        time = 0.0f;
        jump_wait = 1.0f;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //get wasd input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //get mouse input
        float mouseMove = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //rotate character
        transform.Rotate(new Vector3(0.0f, mouseMove, 0.0f) * Time.fixedDeltaTime * 1000);
        if (state == 0)
        {
            if (firstPersonCam.transform.eulerAngles.x > 30 && firstPersonCam.transform.eulerAngles.x <= 180 && mouseY < 0)
            {

            }
            else if (firstPersonCam.transform.eulerAngles.x > 180 && firstPersonCam.transform.eulerAngles.x < 330 && mouseY > 0)
            {

            }
            else
            {
                firstPersonCam.transform.Rotate(new Vector3(-mouseY, 0.0f, 0.0f) * Time.fixedDeltaTime * 750);
            }
        }

        //calculate animation state
        float animationState = Mathf.Max(Mathf.Abs(moveHorizontal), Mathf.Abs(moveVertical));
        if (animationState > 0)
        {
            animationState += sprint;
        }
        //set animation state
        playerAnimator.SetFloat("State", animationState);

        //move character horizontally
        transform.Translate(new Vector3(moveHorizontal, 0.0f, moveVertical) * Time.fixedDeltaTime * (speed + (sprint * 5)));

        //update time variable 
        time += Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            jump();
        }

            if (Input.GetKeyDown("v"))
        {
            if (state == 0)
            {
                thirdPersonCam.enabled = true;
                firstPersonCam.enabled = false;
                state = 1;
            }
            else if (state == 1)
            {
                topDownCam.enabled = true;
                thirdPersonCam.enabled = false;
                state = 2;
            }
            else
            {
                firstPersonCam.enabled = true;
                topDownCam.enabled = false;
                state = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprint = 1.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = 0.0f;
        }
    }


    private void jump()
    {
        //wait jump_time seconds before next jump
        if (time - jump_time > jump_wait)
        {
            rb.velocity = rb.velocity + transform.up * 5;
            jump_time = time;
        }
    }
}
