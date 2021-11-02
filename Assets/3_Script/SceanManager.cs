using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManager : MonoBehaviour
{
    int sceneindex;
    int nextindex;
    private void Start()
    {
        sceneindex = SceneManager.GetActiveScene().buildIndex;
        nextindex = sceneindex+1;
    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("stage 1");
        }
    }
    public void NextScene()
    {
        SceneManager.LoadScene(nextindex);
    }
    //IEnumerator LoadScene()
    //{
    //    // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(nextindex).name);
    //    // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}
}   
