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
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(sceanNumber).name);
        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
