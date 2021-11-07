using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScean : MonoBehaviour
{
    private void Awake()
    {
        
    }
    void Start()
    {
        if(GoogleMobileAdTest.isFailed == false)
        {
            GoogleMobileAdTest.Init();
        }
        var loadData = SaveSystem.LoadGame();
        if(loadData != null)
        {
            SceneManager.LoadScene(loadData.stageSave);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    void Update()
    {
        
    }
    IEnumerator LoadScene(int sceanNumber)
    {
        // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(sceanNumber).name);
        // Scene�� �ҷ����� ���� �Ϸ�Ǹ�, AsyncOperation�� isDone ���°� �ȴ�.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
