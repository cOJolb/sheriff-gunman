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
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.Trace:
                bossBattle.SetActive(true);
                break;
            case GameManager.GameState.Boss:
                //bossBattle.SetActive(true); 타이밍바
                break;
            case GameManager.GameState.finish:
                bossTiming.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                //bossBattle.SetActive(false);
                bossTiming.SetActive(false);
                //GameOverUI.SetActive(true);
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
}
