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
    //    // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(nextindex).name);
    //    // Scene�� �ҷ����� ���� �Ϸ�Ǹ�, AsyncOperation�� isDone ���°� �ȴ�.
    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}
}   
