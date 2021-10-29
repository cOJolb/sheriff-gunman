using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Button retry;
    public Button adButton;

    public void OnAdButton()
    {
        GameManager.instance.state = GameManager.GameState.ReStart;
    }
}
