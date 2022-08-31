using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Slider zoomSlider;

    public Vector3 offset;
    public Vector3 lookOffset;
    public float damping;

    public bool look;

    private void Update()
    {
        offset.z = zoomSlider.value;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, damping) + offset;
        if (look)
        {
            transform.LookAt(target.position + lookOffset);
        }
    }
}
