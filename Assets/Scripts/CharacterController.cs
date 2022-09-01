using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private Joystick joystick;

    [Space(10)]
    [Header("Movement")]
    public float movementSpeed = 10f;
    public float maxSpeed = 15f;
    public float jumpSpeed = 15f;
    private Vector3 moveDirection;

    private float doubleClickTime = 0.2f;
    private bool isDoubleClick = false;

    private Rigidbody rb;
    private bool canJump = true;
    private bool stunned;

    private GameObject lastCheckpoint;

    private Animator animator;

    [Space(10)]
    [Header("Location Slider")]
    public Slider locationSlider;

    [Space(10)]
    [Header("Slope Handling")]
    public float cornerClimbForce;
    public Transform cornerRaycastPos;
    private RaycastHit cornerHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        locationSlider.minValue = GameObject.FindGameObjectWithTag("Start").transform.position.z;
        locationSlider.maxValue = GameObject.FindGameObjectWithTag("Finish").transform.position.z;
    }

    private void FixedUpdate()
    {
        rb.maxLinearVelocity = maxSpeed;

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;


        if (horizontal > 0.2f || vertical > 0.2f || horizontal < -0.2f || vertical < -0.2f)
        {
            animator.SetBool("isRunning", true);
            Vector3 direction = Vector3.forward * -vertical + Vector3.right * -horizontal;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);

            moveDirection = new Vector3(horizontal, 0, vertical);
            //if (stunned) return;

            //rb.MovePosition(transform.position + new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime);

            if (Physics.Raycast(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), out cornerHit, transform.localScale.x * 1.3f))
            {
                Debug.DrawRay(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), Color.red, 25f);
                Debug.Log(cornerHit.collider.name);
            }

            if (OnCorner())
            {
                rb.AddForce(new Vector3(0, 1, 0) * cornerClimbForce * Time.deltaTime, ForceMode.Acceleration);
            }

                rb.AddForce(new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime, ForceMode.Acceleration);

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

        if (transform.position.z > GameObject.FindGameObjectWithTag("Finish").transform.position.z) animator.SetBool("dance", true);

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (isDoubleClick)
                {
                    if (canJump)
                    {
                        Jump();
                        animator.SetTrigger("jump");
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

    public void Jump()
    {
        canJump = false;
        animator.SetBool("isOnAir", !canJump);
        rb.AddForce(new Vector3(0, jumpSpeed, 0) * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        animator.SetBool("isOnAir", !canJump);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Checkpoint")
        {
            lastCheckpoint = other.gameObject;
        }
        if (other.transform.tag == "DeadArea")
        {
            Death();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Checkpoint")
        {
            lastCheckpoint = other.gameObject;
        }
    }

    IEnumerator restartDoubleClick()
    {
        yield return new WaitForSeconds(doubleClickTime);
        isDoubleClick = false;
    }

    public void Death()
    {
        rb.velocity = Vector3.zero;
        transform.position = lastCheckpoint.transform.position;
        FindObjectOfType<CameraController>().transform.position = lastCheckpoint.transform.position + FindObjectOfType<CameraController>().GetComponent<CameraController>().offset;
    }

    private bool OnCorner()
    {
        if (Physics.Raycast(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), out cornerHit, 25f))
        {
            Debug.DrawRay(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), Color.red, 25f);
            return true;
        }

        return false;
    }

}
