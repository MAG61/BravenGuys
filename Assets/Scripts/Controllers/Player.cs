using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Space(10)]
    [Header("Movement")]
    public float movementSpeed = 10f;
    public float maxSpeed = 15f;
    public float jumpSpeed = 15f;
    public PhysicMaterial ignore;

    public Rigidbody rb;
    public bool canJump = true;
    public bool stunned;

    public GameObject lastCheckpoint;

    public Animator animator;

    [Space(10)]
    [Header("Slope Handling")]
    public float cornerClimbForce;
    public Transform cornerRaycastPos;
    public RaycastHit cornerHit;

    public void Jump()
    {
        animator.SetTrigger("jump");
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

    public void Death()
    {
        rb.velocity = Vector3.zero;
        transform.position = lastCheckpoint.transform.position;
        if (gameObject.name == "Player(Clone)") { FindObjectOfType<CameraController>().transform.position = lastCheckpoint.transform.position + FindObjectOfType<CameraController>().GetComponent<CameraController>().offset; }
    }


    public void reloadStun()
    {
        StartCoroutine(reloadStunn());
    }

    public void CheckFinish()
    {
        if (SceneManager.GetActiveScene().name != "Elimination" && SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "RandomMap")
        {
            if (transform.position.z > GameObject.FindGameObjectWithTag("Finish").transform.position.z)
                GameObject.Find("MapManager").GetComponent<MapManager>().Qualify(this);
        }
    }

    IEnumerator reloadStunn()
    {
        yield return new WaitForSeconds(0.7f);
        stunned = false;
    }
}
