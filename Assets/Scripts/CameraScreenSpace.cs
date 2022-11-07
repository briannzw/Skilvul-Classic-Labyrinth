using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScreenSpace : MonoBehaviour
{
    public RawImage bg;

    WebCamTexture backCamera;
    Coroutine cameraStarter;

    private void OnEnable()
    {
        if (cameraStarter != null) return;
        cameraStarter = StartCoroutine(StartCamera());
    }

    private void OnDisable()
    {
        if(backCamera != null && backCamera.isPlaying)
        StopCoroutine(cameraStarter);
    }

    private void Update()
    {
        if(cameraStarter == null && (backCamera == null || !backCamera.isPlaying))
        {
            StartCoroutine(StartCamera());
            return;
        }
    }

    int flipY, orient;

    IEnumerator StartCamera()
    {
#if UNITY_EDITOR
        Debug.Log("Unity Remote Connecting");
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Unity Remote Connected");
#endif

        WebCamDevice[] devices = WebCamTexture.devices;
        foreach(WebCamDevice device in devices)
        {
            if (!device.isFrontFacing)
                backCamera = new WebCamTexture(device.name, Screen.width, Screen.height, 60);
        }

        if(backCamera == null)
        {
            Debug.Log("Back Camera not found");
            yield break;
        }

        bg.texture = backCamera;
        backCamera.Play();

        while (!backCamera.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        while(backCamera.width < 100)
        {
            yield return new WaitForEndOfFrame();
        }

        flipY = backCamera.videoVerticallyMirrored ? -1 : 1;
        bg.rectTransform.localScale = new Vector3(1, flipY, 1);

        orient = -backCamera.videoRotationAngle;
        bg.rectTransform.rotation = Quaternion.Euler(0, 0, orient);

        bg.rectTransform.sizeDelta = new Vector3(backCamera.width, backCamera.height);
    }
}
