using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float accelleration = 1.0f;
    public float deadzone = 0.25f;
    public Rigidbody rb;

    private Vector3 movement;
    private Vector3 mouseWorldPos;
    private Vector3 mousePos;
    public Camera cam;

    private float currentMoveSpeed;

    private float inputX;
    private float inputY;

    private Vector3 relativeMovement;

    public Animator animationController;

    private LightGun m_LightGun;

    private float resetProg;
    public float resetTime = 1.0f;

    public AudioSource audioS;
    public AudioClip fireSFX;

    private CameraScript cs;


    private void Awake()
    {
        audioS = gameObject.GetComponent<AudioSource>();
        m_LightGun = GetComponent<LightGun>();
        Debug.Assert(m_LightGun != null, "Unable to get LightGun component", this);
        cam = Camera.main;
        cs = gameObject.GetComponent<CameraScript>();
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

        if(transform.position.y < -15)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //inform the camera script the distance the mouse is from the player
        float md = Vector3.Distance(transform.position, mouseWorldPos);
        cs.SetMouseDistance(md);


    }

    private void FixedUpdate()
    {
        //move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //player manual reset
        if (Input.GetKey(KeyCode.R)){
            resetProg += Time.deltaTime;
        }else{
            resetProg -= Time.deltaTime;
        }

        resetProg = Mathf.Clamp(resetProg, 0, resetTime);

        if(resetProg >= resetTime)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
