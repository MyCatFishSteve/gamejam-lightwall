using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float accelleration = 1.0f;
    public float deadzone = 0.25f;
    public Rigidbody rb;

    public Vector3 movement;
    public Vector3 mouseWorldPos;
    public Vector3 mousePos;
    public Camera cam;

    public float currentMoveSpeed;

    float inputX;
    float inputY;

    public Vector3 relativeMovement;

    public Animator animationController;

    private LightGun m_LightGun;

    private void Awake()
    {
        m_LightGun = GetComponent<LightGun>();
        Debug.Assert(m_LightGun != null, "Unable to get LightGun component", this);
        cam = Camera.main;
    }

    ///forwardSpeed = Mathf.Lerp(forwardSpeed, v* MovementSpeed, Time.deltaTime* accellerationGround);
    
    // Update is called once per frame
    private void Update()
    {
        inputX = Mathf.Lerp(inputX, Input.GetAxisRaw("Horizontal"), Time.deltaTime * accelleration);
        inputY = Mathf.Lerp(inputY, Input.GetAxisRaw("Vertical"), Time.deltaTime * accelleration);

        //player movement input
        movement = new Vector3(inputX, 0, inputY);

        //deal with deadzones if a joystick is being used
        if(movement.magnitude < deadzone){
            movement = Vector3.zero;
        }

        //clamp the player movement so that diagonal movement isn't faster than regular movement
        movement = Vector3.ClampMagnitude(movement, 1);

        //currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, moveSpeed * movement.magnitude, Time.deltaTime * accelleration);



        //mouse rotation
        mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y - rb.position.y));
        mouseWorldPos.y = rb.position.y;
        mousePos = Input.mousePosition;

        transform.rotation = Quaternion.LookRotation(mouseWorldPos - rb.position);

        relativeMovement = transform.InverseTransformDirection(new Vector3(inputX, 0, inputY));
        relativeMovement = Vector3.ClampMagnitude(relativeMovement, 1);

        //set animations
        animationController.SetFloat("MoveX", relativeMovement.x);
        animationController.SetFloat("MoveY", relativeMovement.z);

        if (Input.GetButtonDown("Fire1"))
        {
            m_LightGun.Fire();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            m_LightGun.Clear();
        }
    }

    private void FixedUpdate()
    {
        //move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
