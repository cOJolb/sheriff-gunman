using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanScript : MonoBehaviour
{
    static int sceneindex;
    static int nextindex;
    static int index;
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
    private void Update()
    {
        if(GoogleMobileAdTest.isClosed)
        {
            GoogleMobileAdTest.isClosed = false;
            SceneManager.LoadScene(index);
        }
    }
    public static void CurScene()
    {
        var random = Random.value;
        if (random > 0.3f)
        {
            GoogleMobileAdTest.OnClickInterstitial();
            index = sceneindex;
        }
        else
        {
            SceneManager.LoadScene(sceneindex);
        }
    }
    public void NextScene()
    {
        var random = Random.value;
        if (random > 0.3f)
        {
            GoogleMobileAdTest.OnClickInterstitial();
            index = nextindex;
        }
        else
        {
            SceneManager.LoadScene(nextindex);
        }
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
