using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float jumpSpeed = 15f;

    private Rigidbody rb;
    private bool canJump = true;

    public List<GameObject> destinations = new();
    private GameObject currentDestination;

    private GameObject nearestDest;
    private GameObject lastCheckpoint;

    [Space(10)]
    [Header("Nav Mesh")]
    private NavMeshAgent agent;
    public Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        foreach (GameObject dest in GameObject.FindGameObjectsWithTag("Destination"))
        {
            destinations.Add(dest);
        }

        currentDestination = destinations[0];
    }
    void Update()
    {
        if (transform.position.y < -10) transform.position = lastCheckpoint.transform.position;

        //agent.SetDestination(target.position);


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

        Vector3 moveVector = currentDestination.transform.position + new Vector3(Random.Range(0, 2f), 0, Random.Range(0, 2f)) - transform.position;
        Vector3 direction = Vector3.forward * moveVector.z + Vector3.right * moveVector.x;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.07f);

        moveVector.Normalize();

        rb.AddForce(new Vector3(moveVector.x, 0, moveVector.z).normalized * movementSpeed * Time.deltaTime, ForceMode.Acceleration);



    }

    public void Jump()
    {
        canJump = false;
        rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Checkpoint")
        {
            lastCheckpoint = other.gameObject;
        }

        if (other.transform.tag == "JumpArea")
        {
            if (canJump) Jump();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Checkpoint")
        {
            lastCheckpoint = other.gameObject;
        }

        if (other.transform.tag == "JumpArea")
        {
            if (canJump) Jump();
        }
    }
}
