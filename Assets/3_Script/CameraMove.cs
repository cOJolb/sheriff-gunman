using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float distance = 10f;
    public float height = 5f;
    public float speed = 3f;
    public GameObject BossFight;
    public GameObject playerEye;
    Transform horse;
    Transform boss;
    Transform cowboy;
    void Start()
    {
        cowboy = GameObject.FindGameObjectWithTag("Player").transform;
        horse = GameObject.FindGameObjectWithTag("Horse").transform;
        boss = GameObject.FindGameObjectWithTag("Boss").transform;

        transform.position = boss.position;
        transform.position += boss.transform.up * 1.5f /*+ boss.transform.right * 0.5f */+ boss.transform.forward * 1.5f;
        
        transform.rotation = boss.transform.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    void LateUpdate()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:

                break;
            case GameManager.GameState.Play:
                Vector3 targetPosition = new Vector3(horse.position.x, horse.position.y + height, horse.position.z) - horse.forward * distance;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

                transform.LookAt(horse);
                //var cameraPos = player.transform.position;
                //cameraPos.y += 10;
                //cameraPos.z -= 10;

                //transform.position = cameraPos;
                //transform.LookAt(player.transform);
                break;
            case GameManager.GameState.Boss:
                Vector3 BossFightPos = BossFight.transform.position;
                Vector3 BosstargetPos = new Vector3(BossFightPos.x, BossFightPos.y + 14, BossFightPos.z) + BossFight.transform.right * 10 - BossFight.transform.forward * 10;
                //transform.position = Vector3.Lerp(transform.position, BosstargetPos, Time.deltaTime * speed);
                transform.position = BosstargetPos;

                transform.LookAt(BossFight.transform);
                //cameraPos = player.transform.position;
                //cameraPos.x += 10;
                //cameraPos.y += 10;
                //cameraPos.z -= 10;

                //transform.position = cameraPos;
                //transform.LookAt(player.transform);
                break;
            case GameManager.GameState.Trace:
                transform.position = playerEye.transform.position;
                transform.position += transform.up * 0.2f;
                transform.LookAt(boss.position);

                break;

            default:
                break;
        }
    }
    public void StateInit(GameManager.GameState value)
    {
        switch (value)
        {
            case GameManager.GameState.Idle:
                break;
            case GameManager.GameState.Start:
                transform.position = cowboy.position;
                transform.position += cowboy.transform.up * 5f + cowboy.transform.right * 0.5f - cowboy.transform.forward * 5f;
                transform.LookAt(cowboy.transform);
                transform.rotation = cowboy.rotation * Quaternion.Euler(20f, 0f, 0f);
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.Trace:
                break;
            case GameManager.GameState.RunOver:
                break;
            case GameManager.GameState.Boss:
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.ReStart:
                break;
            default:
                break;
        }
    }
}
