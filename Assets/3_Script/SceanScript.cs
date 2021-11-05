using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanScript : MonoBehaviour
{
    int sceneindex;
    int nextindex;
    public int NextIndex
    {
        get
        {
            return nextindex;
        }
    }
    private void Start()
    {
        sceneindex = SceneManager.GetActiveScene().buildIndex;
        nextindex = sceneindex+1;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextindex);
    }
    IEnumerator LoadScene()
    {
        // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(nextindex).name);
        // Scene�� �ҷ����� ���� �Ϸ�Ǹ�, AsyncOperation�� isDone ���°� �ȴ�.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}   
