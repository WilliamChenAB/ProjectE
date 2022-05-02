using UnityEngine;
using System.Collections;

public class FighterController : MonoBehaviour {

    [SerializeField]
    public int maxSpeed;

    [SerializeField]
    private int turnSpeed;

    [SerializeField]
    private Blaster[] blasters;

    [SerializeField]
    private TrailRenderer[] trailRenderers;

    private Transform tr;
    private Rigidbody rb;

    private float ntruePitch;
    private float ntrueYaw;
    private float trueYaw;
    private float truePitch;

    void Start () {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update () {
        truePitch = -Input.GetAxis("Mouse Y");
        trueYaw = Input.GetAxis("Mouse X");

        ntrueYaw = Mathf.Lerp(ntrueYaw, trueYaw, Time.deltaTime * 4);
        ntruePitch = Mathf.Lerp(ntruePitch, truePitch, Time.deltaTime * 4);

        if (Input.GetMouseButton(0)) {
            foreach(var b in blasters) {
                b.Shoot();
            }
        } 
    }

    void FixedUpdate() {
        float accel = 0;
        float moveX = 0;
        float moveY = 0;
        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;

        if (Input.GetKey(KeyCode.W))
        {
            accel = 5000;
            foreach (var t in trailRenderers)
            {
                t.startWidth = 0.5f;
            }
        }
        //Complete rest of code for trail width effects
        else if (Input.GetKey(KeyCode.S))
        {
            accel = -5000;
            foreach (var t in trailRenderers)
            {
                t.startWidth = 0f;
                t.endWidth = 0f;
            }
        }
        else
        {
            foreach (var t in trailRenderers)
            {
                t.startWidth = 0.2f;
            }
        }
        Quaternion newRot = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y, 0);
        if (Input.GetKey(KeyCode.A)) {
            moveX = -5000;
            newRot = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y, 60);
        } else if (Input.GetKey(KeyCode.D)) {
            moveX = 5000;
            newRot = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y, -60);
        }

        tr.rotation = Quaternion.Slerp(tr.rotation, newRot, Time.deltaTime * turnSpeed);

        if (Input.GetKey(KeyCode.Space)) {
            moveY = 2000;
            Cursor.lockState = CursorLockMode.Locked;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            moveY = -2000;
        }

        Quaternion rot = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y, 0);
        rb.AddForce(rot * Vector3.forward * accel);
        rb.AddForce(rot * Vector3.right * moveX);
        rb.AddForce(rot * Vector3.up * moveY);
        tr.Rotate(ntruePitch, ntrueYaw, 0);

        if (currentSpeed > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
