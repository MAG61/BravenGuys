using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { None, Turning, Hanging, Pos2Pos, Cannon, Bouncer }
    public ObstacleType obstacleType;

    [SerializeField] private float torque = 10f;
    [SerializeField] private Vector3 torqueVector;
    [SerializeField] private bool Euler;

    [SerializeField] private Vector3 firstPos;
    [SerializeField] private Vector3 lastPos;
    [SerializeField] private float speed;
    [SerializeField] private char axis;
    [SerializeField] private float minWait;
    [SerializeField] private float maxWait;
    private short state = 2;

    [SerializeField] private float maxAngle;
    [SerializeField] private float turnSpeed;
    private bool goinFirst = true;

    [SerializeField] private Transform muzzle;
    [SerializeField] private float power;
    [SerializeField] private Vector3 ballVector;
    private GameObject ballPrefab;
    private bool canShoot = true;

    [SerializeField] private float bouncePower;

    private Rigidbody rb;

    private void Start()
    {
        ballPrefab = Resources.Load("Prefabs/Top") as GameObject;
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
                transform.localRotation = Quaternion.Euler(torqueVector * torque * Time.deltaTime + transform.localRotation.eulerAngles);
            }
            else
            {
                //if (rb.angularVelocity.magnitude < torque / 3 * Time.fixedDeltaTime) torqueVector *= -1; 
                rb.angularVelocity = (torqueVector * torque * Time.fixedDeltaTime);
                //GetComponent<Rigidbody>().maxAngularVelocity = maxTorque;
                //GetComponent<Rigidbody>().AddRelativeTorque(torqueVector * torque, ForceMode.Impulse);
            }



        }

        if (obstacleType == ObstacleType.Hanging)
        {
            if (!goinFirst && transform.localEulerAngles.y >= maxAngle && transform.rotation.eulerAngles.y < 180) goinFirst = !goinFirst;
            if (goinFirst && transform.localEulerAngles.y <= 360 - maxAngle && transform.rotation.eulerAngles.y > 180) goinFirst = !goinFirst;

            float speedMultiplier = 1;

            if (true)
            {
                if (transform.localEulerAngles.y > 180)
                {
                    speedMultiplier = (transform.localEulerAngles.y - (360 - maxAngle) + 5) / maxAngle;
                }
                else if (transform.localEulerAngles.y < 180)
                {
                    speedMultiplier = ((maxAngle - transform.localEulerAngles.y) + 5) / maxAngle;
                }
            }

            //if (!goinFirst)
            //{
            //    if (transform.localEulerAngles.y > 180)
            //    {
            //        speedMultiplier = -maxAngle / (transform.localEulerAngles.y - (360 - maxAngle));
            //    }
            //    else if (transform.localEulerAngles.y < 180)
            //    {
            //        speedMultiplier = maxAngle / transform.localEulerAngles.y;
            //    }
            //}

            if (goinFirst) rb.angularVelocity = (new Vector3(0, 0, 1) * turnSpeed * speedMultiplier * Time.fixedDeltaTime);
            if (!goinFirst) rb.angularVelocity = (new Vector3(0, 0, 1) * -turnSpeed * speedMultiplier * Time.fixedDeltaTime);
        }

        if (obstacleType == ObstacleType.Pos2Pos)
        {
            if (axis == 'x')
            {
                if (transform.localPosition.x >= firstPos.x && state == 1)
                {
                    StartCoroutine(goTwo());
                    state = 0;
                }
                if (transform.localPosition.x <= lastPos.x && state == 2)
                {
                    StartCoroutine(goOne());
                    state = 0;
                }
            }
            if (axis == 'z')
            {
                if (transform.localPosition.z >= firstPos.z && state == 1)
                {
                    StartCoroutine(goTwo());
                    state = 0;
                }
                if (transform.localPosition.z <= lastPos.z && state == 2)
                {
                    StartCoroutine(goOne());
                    state = 0;
                }
            }
            if (axis == 'y')
            {
                if (transform.localPosition.y >= firstPos.y && state == 1)
                {
                    StartCoroutine(goTwo());
                    state = 0;
                }
                if (transform.localPosition.y <= lastPos.y && state == 2)
                {
                    StartCoroutine(goOne());
                    state = 0;
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
                StartCoroutine(shoot());
                canShoot = false;
            }
        }

        if (obstacleType == ObstacleType.Bouncer)
        {

        }
    }

    IEnumerator goOne()
    {
        if (minWait == 0)
        {
            yield return new WaitForEndOfFrame();
            state = 1;
            yield break;
        }
        yield return new WaitForSeconds(0.05f);
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

    IEnumerator shoot()
    {
        GetComponentInChildren<Animation>().Play();

        yield return new WaitForSeconds(0.75f);

        GameObject ball = Instantiate(ballPrefab, muzzle.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().velocity = ballVector * power;
        StartCoroutine(reloadCannon());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (obstacleType == ObstacleType.Bouncer)
        {
            Vector3 bounceVector = new Vector3(collision.transform.position.x - transform.position.x, 0, collision.transform.position.z - transform.position.z).normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(bounceVector * bouncePower, ForceMode.VelocityChange);
            collision.gameObject.GetComponent<CharacterController>().stunned = true;
            collision.gameObject.GetComponent<CharacterController>().reloadStun();
            GetComponentInChildren<Animation>().Play();
        }
    }
}
