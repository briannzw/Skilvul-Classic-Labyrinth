using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotation : MonoBehaviour
{
    private void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(90f, 90f, 0f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}
