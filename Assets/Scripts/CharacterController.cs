using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;

    public float movementSpeed = 10f;
    public float maxSpeed = 15f;
    public float jumpSpeed = 15f;

    public float doubleClickTime = 0.1f;
    private bool isDoubleClick = false;

    private Rigidbody rb;
    private bool canJump = true;
    private bool stunned;

    private GameObject lastCheckpoint;

    private Animator animator;

    public Slider locationSlider;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.07f);

            //if (stunned) return;

            //rb.MovePosition(transform.position + new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime);

            rb.AddForce(new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime, ForceMode.Acceleration);

            //Vector3 addPos = new Vector3(horizontal, 0, vertical).normalized * movementSpeed * Time.deltaTime;
            //transform.position += (addPos);
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

        if (Mathf.Abs(rb.velocity.x) > 5f || Mathf.Abs(rb.velocity.z) > 5f)
        {
            stunned = true;
        }
        else
        {
            stunned = false;
        }

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
}
