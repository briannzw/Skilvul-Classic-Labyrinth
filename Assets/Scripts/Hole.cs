using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool entered = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Goal Reached");
        entered = true;
    }
}
