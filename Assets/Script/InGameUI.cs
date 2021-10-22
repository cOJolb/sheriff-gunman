using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public GameObject bossBattle;
    public Image enemyCheck;
    public Image enemyProgress;
    public GameObject bossTiming;
    private void Start()
    {
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
    public void UiInit(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.Boss:
                //bossBattle.SetActive(true); 타이밍바
                break;
            case GameManager.GameState.finish:
                
                break;
            case GameManager.GameState.GameOver:
                //bossBattle.SetActive(false);
                bossTiming.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void TimingOn()
    {
        bossTiming.SetActive(true);
    }
}
