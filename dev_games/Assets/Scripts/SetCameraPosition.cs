using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    private void Start()
    {
        // Verifica se h� uma posi��o de c�mera armazenada
        if (CameraPositionManager.Instance != null)
        {
            Vector3 savedCameraPosition = CameraPositionManager.Instance.CameraPosition;
            // Encontra todos os GameObjects com a tag "MainCamera" na cena
            GameObject[] mainCameras = GameObject.FindGameObjectsWithTag("MainCamera");
            // Define a posi��o de todos os GameObjects encontrados
            foreach (GameObject mainCamera in mainCameras)
            {
                mainCamera.transform.position = savedCameraPosition;
            }
        }
    }
}
