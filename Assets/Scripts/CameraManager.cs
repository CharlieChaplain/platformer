using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<string> cameraNames;
    public List<CinemachineVirtualCameraBase> cameraObjects;
    Dictionary<string, CinemachineVirtualCameraBase> allCamerasInScene;

    public CinemachineVirtualCameraBase CurrentCamera;

    // Start is called before the first frame update
    void Start()
    {
        allCamerasInScene = new Dictionary<string, CinemachineVirtualCameraBase>();
        int maxCount = cameraNames.Count;
        if (cameraObjects.Count < maxCount)
            maxCount = cameraObjects.Count;
        for(int i = 0; i<maxCount; i++)
        {
            allCamerasInScene.Add(cameraNames[i], cameraObjects[i]);
        }

    }

    public static void ChangeCamera(string newCamName)
    {
        CameraManager.Instance.CurrentCamera.Priority = 5; //arbitrarily 5 and 10. The old cam just needs lower priority and the new cam needs higher
        CameraManager.Instance.allCamerasInScene[newCamName].Priority = 10;
        CameraManager.Instance.CurrentCamera = CameraManager.Instance.allCamerasInScene[newCamName];
    }
}
