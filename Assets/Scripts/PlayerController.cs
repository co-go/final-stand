using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Camera topDownCam;
    public Camera firstPersonCam;
    public Camera thirdPersonCam;
    public GameObject bullet;
    public Text healthBar;
    public Text ammoBar;
    public Text reloadText;

    private Animator playerAnimator;
    private int state;
    private float sprint;
    private Rigidbody rb;
    private float time;
    private float jump_wait;
    private float jump_time;
    private int reload_delay;

    private int health;
    private int ammo;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        firstPersonCam.enabled = false;
        thirdPersonCam.enabled = true;
        topDownCam.enabled = false;
        sprint = 0.0f;
        time = 0.0f;
        jump_wait = 1.2f;
        reload_delay = 2;

        health = 100;
        ammo = 10;
        reloadText.text = "";
        healthBar.text = "Health: " + health;
        ammoBar.text = "Ammo: " + ammo;

        Physics.gravity = new Vector3(0, -30.0F, 0);
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
        if (state == 1)
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
        transform.Translate(new Vector3(moveHorizontal, 0.0f, moveVertical) * Time.fixedDeltaTime * (speed + (sprint * 6)));

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
                firstPersonCam.enabled = true;
                thirdPersonCam.enabled = false;
                state = 1;
            }
            else if (state == 1)
            {
                topDownCam.enabled = true;
                firstPersonCam.enabled = false;
                state = 2;
            }
            else
            {
                thirdPersonCam.enabled = true;
                topDownCam.enabled = false;
                state = 0;
            }
        }

        //reload if r is pressed
        if (Input.GetKeyDown("r"))
        {
            //reload with 'reload_delay' second delay
            reloadText.text = "Reloading...";
            Invoke("reload", reload_delay);
        }

        //set sprint to 1 on shift press
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprint = 1.0f;
        }

        //set shift to 0 on shift release 
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = 0.0f;
        }

        //shoot bullet
        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }
    }


    private void jump()
    {
        //wait jump_time seconds before next jump
        if (time - jump_time > jump_wait)
        {
            rb.velocity = rb.velocity + transform.up * 18;
            jump_time = time;
        }
    }

    private void shoot()
    {
        if (ammo > 0)
        {
            Quaternion myRotation = Quaternion.identity;
            myRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y + 90, 90);
            GameObject obj = Instantiate(bullet, transform.position, myRotation) as GameObject;
            Rigidbody bulletBody = obj.GetComponent<Rigidbody>();
            bulletBody.AddForce(transform.forward * 7000);
            Destroy(obj, 2.0f);
            ammo--;
            ammoBar.text = "Ammo: " + ammo;
        }
        else
        {
            //reload with 'reload_delay' second delay
            reloadText.text = "Reloading...";
            Invoke("reload", reload_delay);
        }
    }

    private void reload()
    {
        ammo = 10;
        ammoBar.text = "Ammo: " + ammo;
        reloadText.text = "";
    }
}
