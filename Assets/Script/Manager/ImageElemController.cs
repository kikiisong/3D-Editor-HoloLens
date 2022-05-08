using UnityEngine;
using System.IO;
using UnityEngine.Windows.WebCam;
using Microsoft.MixedReality.Toolkit.UI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
#endif

public class ImageElemController : MonoBehaviour
{
    public GameObject imgPrefab;
    public GameObject logoPrefab;
    private PhotoCapture photoCaptureObject = null;
    public ButtonConfigHelper capButton;
    private CameraParameters c;
    private string capturedPath;
    // Start is called before the first frame update
    void Start()
    {
        capButton.OnClick.AddListener(CamOpened);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CamOpened()
    {
        capButton.OnClick.RemoveListener(CamOpened);
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        capButton.MainLabelText = "Say \"Capture\" when you are ready";
    }


    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        //captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    public void StartCapture()
    {
        if(photoCaptureObject != null)
        {
            photoCaptureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
        }
        
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {        
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        capturedPath = "";
        capButton.MainLabelText = "Open Camera";
        capButton.OnClick.AddListener(CamOpened);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            capturedPath = filePath;
            photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);           
            //photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            var fileContent = File.ReadAllBytes(capturedPath);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            // Do as we wish with the texture such as apply it to a material, etc.
            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture = tex;
            ImportedImg imagePath = (ImportedImg)(imgPanel.GetComponent("ImportedImg"));
            imagePath.m_imagePath = capturedPath;

            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            var fileContent = File.ReadAllBytes(capturedPath);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            // Do as we wish with the texture such as apply it to a material, etc.
            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture = tex;
            ImportedImg imagePath = (ImportedImg)(imgPanel.GetComponent("ImportedImg"));
            imagePath.m_imagePath = capturedPath;
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public void addJHULogo()
    {
        Instantiate(logoPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
    }
    public void createImg()
    {
   
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Overwrite with image", "", "jpg,jpeg,png");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x-0.2f, Camera.main.transform.position.y, 0.6f), Quaternion.identity);
            imgPanel.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture=tex;

            ImportedImg imagePath = (ImportedImg)(imgPanel.GetComponent("ImportedImg"));
            imagePath.m_imagePath = path;
        }
#endif

#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)

        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            //filepicker.FileTypeFilter.Add("*");
            //filepicker.FileTypeFilter.Add(".jpg");

            var file = await picker.PickSingleFileAsync();
            Windows.Storage.Streams.IBuffer buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);                      
            Windows.Storage.Streams.DataReader dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer);
            var fileContent = new byte[buffer.Length];     
            dataReader.ReadBytes(fileContent);
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                var tex = new Texture2D(2, 2);
                tex.LoadImage(fileContent);

                GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
                MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
                imgRenderer.material.mainTexture=tex;

                ImportedImg imagePath = (ImportedImg)(imgPanel.GetComponent("ImportedImg"));
                imagePath.m_imagePath = file.Path;

                Debug.Log("***********************************");
                string name = (file != null) ? file.Name : "No data";
                Debug.Log("Name: " + name);
                Debug.Log("***********************************");
                string path = (file != null) ? file.Path : "No data";
                Debug.Log("Path: " + path);
                Debug.Log("***********************************");

                

                //This section of code reads through the file (and is covered in the link)
                // but if you want to make your own parcing function you can 
                // ReadTextFile(path);
                //StartCoroutine(ReadTextFileCoroutine(path));

            }, false);
        }, false);

#endif

    }
}
