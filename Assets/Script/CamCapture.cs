using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class CamCapture : MonoBehaviour
{
    private PhotoCapture photoCaptureObject = null;
    public GameObject debugPanel;
    public GameObject imgPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Starttest()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
        texts.DescriptionText.text += " " + "PhotoCaptureCreated";
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;
        texts.DescriptionText.text += " " + "PhotoCaptureCreated";

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        texts.DescriptionText.text += " " + "PhotoCaptureStopped";
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
            texts.DescriptionText.text += " " + "PhotoCaptureStarted";
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            Debug.Log(targetTexture);
            DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
            texts.DescriptionText.text += " " + "PhotoCaptured " + targetTexture;
            // Do as we wish with the texture such as apply it to a material, etc.
            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture = targetTexture;
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

}
