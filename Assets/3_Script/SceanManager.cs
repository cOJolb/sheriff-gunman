using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("stage_6");
            Debug.Log("stage6");
        }
    }
}   
