using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Praticemode()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void mirrormode()
    {
        SceneManager.LoadSceneAsync(3);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
