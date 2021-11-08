using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanScript : MonoBehaviour
{
    static int sceneindex;
    static int nextindex;
    static int index;
    public int CurIndex
    {
        get
        {
            return sceneindex;
        }
    }
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
        if (sceneindex == 30 || GameManager.instance.allClear)
        {
            nextindex = Random.Range(20, 30);
        }
        else
        {
            nextindex = sceneindex + 1;
        }
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
        if (GoogleMobileAdTest.isFailed)
        {
            GoogleMobileAdTest.RequestInterstitial();
            SceneManager.LoadScene(sceneindex);
            return;
        }

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
        if(GoogleMobileAdTest.isFailed)
        {
            GoogleMobileAdTest.RequestInterstitial();
            SceneManager.LoadScene(nextindex);
            return;
        }

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
