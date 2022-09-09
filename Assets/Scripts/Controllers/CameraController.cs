using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    public Slider zoomSlider;
    public Joystick joystick;

    public Vector3 offset;
    public Vector3 lookOffset;
    public float damping;
    private float CameraAngle;
    public float CameraAngleSpeed = 0.2f;

    public bool look;

    private void Start()
    {
        //zoomSlider = GameObject.Find("ZoomSlider").GetComponent<Slider>();
        joystick = GameObject.FindGameObjectWithTag("CamJoystick").GetComponent<Joystick>();
    }

    private void Update()
    {
        // offset.z = zoomSlider.value;

        CameraAngle += joystick.Horizontal * CameraAngleSpeed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, target.position, damping) + offset;

        transform.position = target.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * offset;
        transform.rotation = Quaternion.LookRotation(target.position + lookOffset - transform.position, Vector3.up);

        //if (look)
        //{
        //    transform.LookAt(target.position + lookOffset);
        //}
    }
}
