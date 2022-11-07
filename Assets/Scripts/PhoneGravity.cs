using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGravity : MonoBehaviour
{
    public Rigidbody rb;
    public float gravityMagnitude;

    bool useGyro;
    bool isTeleporting;
    Vector3 spawnPos;

    private void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            useGyro = false;
            return;
        }

        spawnPos = transform.position;
        Input.gyro.enabled = true;
    }

    Vector3 inputDir, gravityDir;
    private void Update()
    {
        inputDir = useGyro ? Input.gyro.gravity : Input.acceleration;
        gravityDir = new Vector3(inputDir.x, 0, inputDir.y);
    }

    private void FixedUpdate()
    {
        if (isTeleporting) return;

        rb.AddForce(gravityDir * gravityMagnitude, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTeleporting) return;
        if (collision.gameObject.CompareTag("Out"))
        {
            StartCoroutine(DelayedTeleport());
        }
    }

    IEnumerator DelayedTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(3);
        rb.velocity = Vector3.zero;
        transform.position = spawnPos;
        isTeleporting = false;
    }
}
