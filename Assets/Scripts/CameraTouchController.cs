using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchController : MonoBehaviour
{
    [Range(0, 10)] public float camSmooth;
    [Range(0, 2)] public float zoomValue;
    public float minZoom, maxZoom;

    float distance;
    float prevDistance, currDistance, deltaDistance;
    Touch touch0, touch1;
    Vector3 touchMovedWorldPos, cameraBeganWorldPos;
    Vector3 touchPrevWorldPos, delta, targetPos;
    Vector3 touch0PrevPos, touch1PrevPos, touchPrevPos;

    private void Start()
    {
        distance = transform.position.y;
    }

    private void Update()
    {
        if(Input.touchCount == 0) return;

        touch0 = Input.GetTouch(0);

        if(Input.touchCount == 1 && touch0.phase == TouchPhase.Moved)
        {
            touchPrevPos = touch0.position - touch0.deltaPosition;
            touchPrevWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touchPrevPos.x, touchPrevPos.y, distance));

            touchMovedWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch0.position.x, touch0.position.y, distance));

            delta = touchMovedWorldPos - touchPrevWorldPos;

            targetPos = transform.position - delta * .5f;
            //transform.position = Vector3.Lerp(transform.position, targetPos, camSmooth * Time.deltaTime);
            transform.position = targetPos;
        }

        if (Input.touchCount < 2) return;

        touch1 = Input.GetTouch(1);

        if(touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
        {
            touch0PrevPos = touch0.position - touch0.deltaPosition;
            touch1PrevPos = touch1.position - touch1.deltaPosition;
            prevDistance = Vector3.Distance(touch0PrevPos, touch1PrevPos);
            currDistance = Vector3.Distance(touch0.position, touch1.position);
            deltaDistance = currDistance - prevDistance;

            transform.position -= new Vector3(0, deltaDistance * zoomValue * Time.deltaTime ,0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minZoom, maxZoom), transform.position.z);
            distance = transform.position.y;
        }
    }
}
