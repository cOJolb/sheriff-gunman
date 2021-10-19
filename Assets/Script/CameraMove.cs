using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                var cameraPos = player.transform.position;
                cameraPos.y += 20;
                cameraPos.z -= 20;

                transform.position = cameraPos;
                transform.LookAt(player.transform);
                break;
            case GameManager.GameState.Boss:
                cameraPos = player.transform.position;
                cameraPos.x += 10;
                cameraPos.y += 10;
                cameraPos.z -= 10;

                transform.position = cameraPos;
                transform.LookAt(player.transform);
                break;
            default:
                break;
        }
    }
}
