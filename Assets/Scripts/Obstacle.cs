using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { None, Turning, Hanging, Pos2Pos, Cannon }
    public ObstacleType obstacleType;

    [SerializeField] private float torque = 10f;
    [SerializeField] private float maxTorque = 5f;
    [SerializeField] private Vector3 torqueVector;
    [SerializeField] private bool Euler;

    [SerializeField] private Vector3 firstPos;
    [SerializeField] private Vector3 lastPos;
    [SerializeField] private float speed;
    [SerializeField] private char axis;
    [SerializeField] private float minWait;
    [SerializeField] private float maxWait;
    private short state = 2;

    [SerializeField] private Transform muzzle;
    [SerializeField] private float power;
    [SerializeField] private Vector3 ballVector;
    private GameObject ballPrefab;
    private bool canShoot = true;

    private Rigidbody rb;

    private void Start()
    {
        ballPrefab = Resources.Load("Top") as GameObject;
        rb = GetComponent<Rigidbody>();
        foreach (Collider col in FindObjectsOfType<Collider>())
        {
            if (col.gameObject.tag != "Player" && col.gameObject.name != "sinir") Physics.IgnoreCollision(GetComponent<Collider>(), col, true);
        }
    }

    void Update()
    {
        if (obstacleType == ObstacleType.Turning)
        {
            if (Euler)
            {
                transform.rotation = Quaternion.Euler(torqueVector * torque * Time.deltaTime + transform.rotation.eulerAngles);
            }
            else
            {
                //if (rb.angularVelocity.magnitude < torque / 3 * Time.fixedDeltaTime) torqueVector *= -1; 
                rb.angularVelocity = (torqueVector * torque * Time.fixedDeltaTime);
            }


            //GetComponent<Rigidbody>().maxAngularVelocity = maxTorque;
            //GetComponent<Rigidbody>().AddRelativeTorque(torqueVector * torque, ForceMode.Impulse);
        }

        if (obstacleType == ObstacleType.Hanging)
        {

        }

        if (obstacleType == ObstacleType.Pos2Pos)
        {
            if (axis == 'x')
            {
                if (transform.localPosition.x >= firstPos.x && state == 1)
                {
                    state = 0;
                    StartCoroutine(goTwo());
                }
                if (transform.localPosition.x <= lastPos.x && state == 2)
                {
                    state = 0;
                    StartCoroutine(goOne());
                }
            }
            if (axis == 'z')
            {
                if (transform.localPosition.z >= firstPos.z && state == 1)
                {
                    state = 0;
                    StartCoroutine(goTwo());
                }
                if (transform.localPosition.z <= lastPos.z && state == 2)
                {
                    state = 0;
                    StartCoroutine(goOne());
                }
            }
            if (axis == 'y')
            {
                if (transform.localPosition.y >= firstPos.y && state == 1)
                {
                    state = 0;
                    StartCoroutine(goTwo());
                }
                if (transform.localPosition.y <= lastPos.y && state == 2)
                {
                    state = 0;
                    StartCoroutine(goOne());
                }
            }


            if (state == 0) rb.velocity = Vector3.zero;
            if (state == 1) rb.velocity = (firstPos - transform.localPosition).normalized * speed * Time.fixedDeltaTime;
            if (state == 2) rb.velocity = (lastPos - transform.localPosition).normalized * speed * Time.fixedDeltaTime;
        }

        if (obstacleType == ObstacleType.Cannon)
        {
            if (canShoot)
            {
                GameObject ball = Instantiate(ballPrefab, muzzle.position, Quaternion.identity);
                ball.GetComponent<Rigidbody>().velocity = ballVector * power;
                canShoot = false;
                StartCoroutine(reloadCannon());
            }
        }
    }

    IEnumerator goOne()
    {
        if (minWait == 0)
        {
            state = 1;
            yield break;
        }
        yield return new WaitForSeconds(1.2f);
        state = 1;
    }
    IEnumerator goTwo()
    {
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        state = 2;
    }

    IEnumerator reloadCannon()
    {
        yield return new WaitForSeconds(2f);
        canShoot = true;
    }
}
