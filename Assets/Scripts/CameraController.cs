using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smooth = 0.3f;
    public Vector3 cameraOffset = new Vector3(15, 3, 0);
    public Vector3 lookAtOffset = new Vector3(0, 10, 0);

    private Vector3 moveVelocity = Vector3.zero;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + cameraOffset;
        Vector3 lookAtTarget = player.transform.position + lookAtOffset;
        transform.LookAt(lookAtTarget);
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Vector3 cameraPosTarget = player.transform.position + cameraOffset;
        Vector3.SmoothDamp(transform.position, cameraPosTarget, ref moveVelocity, smooth);
        rb.velocity = moveVelocity;
        // transform.position = player.transform.position + cameraOffset;
        // transform.LookAt(player.transform);
    }
}
