using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public GameObject bossBattle;
    public Image enemyCheck;
    public GameObject enemyProgress;
    public GameObject bossTiming;
    public GameObject speedUpBar;
    public GameObject GameOverUI;
    public GameObject GameStart;
    public GameObject StageClear;
    public GameObject progressbar;
    public GameObject Shop;
    public GameObject ShopHorse;
    public GameObject[] catchImage;
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    private void Update()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.Boss:
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                break;
            default:
                break;
        }
    }
    public void StateInit(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Start:
                GameStart.SetActive(false);
                speedUpBar.SetActive(false);
                break;
            case GameManager.GameState.Play:
                progressbar.SetActive(true);
                speedUpBar.SetActive(true);
                break;
            case GameManager.GameState.Trace:
                speedUpBar.SetActive(false);
                bossBattle.SetActive(true);
                break;
            case GameManager.GameState.Boss:
                bossBattle.SetActive(false);
                progressbar.SetActive(false);
                break;
            case GameManager.GameState.finish:
                bossTiming.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                bossBattle.SetActive(false);
                bossTiming.SetActive(false);
                break;
            case GameManager.GameState.ReStart:
                GameOverUI.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void TimingOn()
    {
        bossTiming.SetActive(true);
    }
    public Image EnemyProgress(Image enemy)
    {
        //var progressbar = GetComponentInChildren<ProgressBar>();
        //return Instantiate(enemyProgress, progressbar.transform);
        return Instantiate(enemy, enemyProgress.transform);
    }

    public void OnGameOver()
    {
        GameOverUI.SetActive(true);
    }
    public void OnClear(int enemyCatch)
    {
        StageClear.SetActive(true);
        for (int i = 0; i < enemyCatch; i++)
        {
            catchImage[i].SetActive(true);
        }
    }
    public void OnShop()
    {
        //상점에 들어가면
        GameStart.SetActive(false);
        Shop.SetActive(true);
        ShopHorse.SetActive(true);
    }
    public void ExitShop()
    {
        //상점에서 나와라
        Shop.SetActive(false);
        GameStart.SetActive(true);
        ShopHorse.SetActive(false);
    }
}
