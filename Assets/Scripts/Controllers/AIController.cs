using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class AIController : Player
{
    #region Names
    private string[] names = {"Harry", "Ross",
                        "Bruce", "Cook",
                        "Carolyn", "Morgan",
                        "Albert", "Walker",
                        "Randy", "Reed",
                        "Larry", "Barnes",
                        "Lois", "Wilson",
                        "Jesse", "Campbell",
                        "Ernest", "Rogers",
                        "Theresa", "Patterson",
                        "Henry", "Simmons",
                        "Michelle", "Perry",
                        "Frank", "Butler",
                        "Shirley", "Brooks",
                    "Rachel","Edwards",
                    "Christopher","Perez",
                    "Thomas","Baker",
                    "Sara","Moore",
                    "Chris","Bailey",
                    "Roger","Johnson",
                    "Marilyn","Thompson",
                    "Anthony","Evans",
                    "Julie","Hall",
                    "Paula","Phillips",
                    "Annie","Hernandez",
                    "Dorothy","Murphy",
                    "Alice","Howard"};

    #endregion

    public List<GameObject> destinations = new();
    private GameObject currentDestination;

    private GameObject nearestDest;
    public GameObject AIObject;

    [Space(10)]
    [Header("Name")]
    public string userName;
    public TextMeshPro nameText;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        userName = names[Random.Range(0, names.Length)] + Random.Range(0, 999);
        nameText.text = userName;
    }
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Elimination" || SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "RandomMap") return;
        rb.maxLinearVelocity = maxSpeed;

        if (currentDestination == null) return;

        Vector3 moveVector = currentDestination.transform.position + new Vector3(Random.Range(0, 2f), 0, Random.Range(0, 2f)) - transform.position;
        Vector3 direction = Vector3.forward * -moveVector.z + Vector3.right * -moveVector.x;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.07f);

        moveVector.Normalize();

        if (moveVector == Vector3.zero)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }

        if (OnCorner())
        {
            rb.AddForce(new Vector3(0, 1, 0) * cornerClimbForce * Time.deltaTime, ForceMode.Acceleration);
        }

        rb.AddForce(new Vector3(moveVector.x, 0, moveVector.z).normalized * movementSpeed * Time.deltaTime, ForceMode.Acceleration);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Elimination" || SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "RandomMap") return;
        if (transform.position.y < -10 && lastCheckpoint != null) transform.position = lastCheckpoint.transform.position;

        CheckFinish();
        nameText.transform.LookAt(GameObject.Find("PlayerCamera(Clone)").transform);

        // Destination
        List<GameObject> foreDests = new();
        foreach (GameObject dest in destinations)
        {
            if (dest.transform.position.z > transform.position.z)
            {
                foreDests.Add(dest);
            }
        }
        foreach (GameObject dest in foreDests)
        {
            if (nearestDest == null) nearestDest = dest;

            if (dest.transform.position.z < nearestDest.transform.position.z) nearestDest = dest;
        }
        currentDestination = nearestDest;
        nearestDest = null;
        //
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "JumpArea" && canJump)
        {
            Jump();
        }
    }

    public void FindDests()
    {
        destinations.Clear();

        foreach (GameObject dest in GameObject.FindGameObjectsWithTag("Destination"))
        {
            destinations.Add(dest);
        }

        currentDestination = destinations[0];
    }

    private bool OnCorner()
    {
        if (Physics.Raycast(cornerRaycastPos.position, cornerRaycastPos.TransformDirection(Vector3.back), out cornerHit, 25f))
        {
            return true;
        }

        return false;
    }

}
