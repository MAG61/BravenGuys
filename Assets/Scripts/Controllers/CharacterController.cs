using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : Player
{
    [Header("Input")]
    [SerializeField]
    private Joystick joystick;

    private float doubleClickTime = 0.2f;
    private bool isDoubleClick = false;

    private CameraController cameraCont;

    [Space(10)]
    [Header("Location Slider")]
    public Slider locationSlider;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        foreach (Collider col in FindObjectsOfType<Collider>())
        {
            if (col.sharedMaterial == ignore) Physics.IgnoreCollision(GetComponent<Collider>(), col, true);
        }

        joystick = GameObject.Find("FloatingJoystick").GetComponent<Joystick>();

        locationSlider = GameObject.Find("LocaitonSlider").GetComponent<Slider>();
        locationSlider.minValue = GameObject.FindGameObjectWithTag("Start").transform.position.z;
        locationSlider.maxValue = GameObject.FindGameObjectWithTag("Finish").transform.position.z;

        cameraCont = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
    }

    private void FixedUpdate()
    {
        rb.maxLinearVelocity = maxSpeed;

        if (stunned) return;
        if (joystick == null) return;

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;


        if (horizontal > 0.2f || vertical > 0.2f || horizontal < -0.2f || vertical < -0.2f)
        {
            animator.SetBool("isRunning", true);
            Vector3 direction = cameraCont.transform.forward * -vertical + cameraCont.transform.right * -horizontal;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);

            //moveDirection = new Vector3(horizontal, 0, vertical);
            //if (stunned) return;

            //rb.MovePosition(transform.position + new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime);

            if (OnCorner())
            {
                rb.AddForce(new Vector3(0, 1, 0) * cornerClimbForce * Time.deltaTime, ForceMode.Acceleration);
            }

                rb.AddForce((vertical * cameraCont.transform.forward + horizontal * cameraCont.transform.right).normalized * movementSpeed * Time.deltaTime, ForceMode.Acceleration);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Update()
    {
        locationSlider.value = transform.position.z;

        if (transform.position.y < -10) Death();

        if (stunned) return;

        if (transform.position.z > GameObject.FindGameObjectWithTag("Finish").transform.position.z) animator.SetBool("dance", true);

        CheckFinish();

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (isDoubleClick)
                {
                    if (canJump)
                    {
                        Jump();
                    }
                }
                if (!isDoubleClick)
                {
                    isDoubleClick = true;
                    StartCoroutine(restartDoubleClick());
                }
            }
        }



    }

    IEnumerator restartDoubleClick()
    {
        yield return new WaitForSeconds(doubleClickTime);
        isDoubleClick = false;
    }



    private bool OnCorner()
    {
        if (Physics.Raycast(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), out cornerHit, 25f))
        {
            return true;
        }

        return false;
    }

    public void FindRequirements()
    { 
        joystick = GameObject.Find("FloatingJoystick").GetComponent<Joystick>();

        locationSlider = GameObject.Find("LocaitonSlider").GetComponent<Slider>();
        locationSlider.minValue = GameObject.FindGameObjectWithTag("Start").transform.position.z;
        locationSlider.maxValue = GameObject.FindGameObjectWithTag("Finish").transform.position.z;

        cameraCont = GameObject.Find("PlayerCamera").GetComponent<CameraController>();
    }
}
