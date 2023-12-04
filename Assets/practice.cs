using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class practice : MonoBehaviour
{
    public void Forehand()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void Volley()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void Smash()
    {
        SceneManager.LoadSceneAsync(6);
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
