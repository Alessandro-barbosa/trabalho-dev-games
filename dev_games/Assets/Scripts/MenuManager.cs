using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneName;
    private Vector3 camPosition;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void changeScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void changeScenePrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void cameraPosition(Vector3 pos)
    {
        this.camPosition = pos;
    }
}
