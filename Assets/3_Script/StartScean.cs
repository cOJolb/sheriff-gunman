using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScean : MonoBehaviour
{
    void Start()
    {
        GoogleMobileAdTest.instance.OnClickInit();
        var loadData = SaveSystem.LoadGame();
        if(loadData != null)
        {
            StartCoroutine(LoadScene(loadData.stageSave));
        }
        else
        {
            StartCoroutine(LoadScene(1));
        }
    }

    void Update()
    {
        
    }
    IEnumerator LoadScene(int sceanNumber)
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
