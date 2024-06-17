using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    public static CameraPositionManager Instance { get; private set; }
    public Vector3 CameraPosition { get; private set; }
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
    public void SetCameraPosition(Vector3 position)
    {
        this.CameraPosition = position;
    }
}
