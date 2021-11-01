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
    public Transform HorseTarget;
    public Transform BossHorseTarget;
    Transform horse;
    Transform boss;
    Transform bosshorse;
    Transform cowboy;
    Vector3 targetPos;
    Quaternion runRotate;
    void Start()
    {
        cowboy = GameObject.FindGameObjectWithTag("Player").transform;
        horse = GameObject.FindGameObjectWithTag("Horse").transform;
        boss = GameObject.FindGameObjectWithTag("Boss").transform;
        bosshorse = GameObject.FindGameObjectWithTag("BossHorse").transform;

        transform.position = boss.position;
        transform.position += boss.transform.up * 1.5f + boss.transform.forward * 1.5f;
        
        transform.rotation = boss.transform.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    void LateUpdate()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:

                break;
            case GameManager.GameState.Play:
                targetPos = new Vector3(HorseTarget.position.x, HorseTarget.position.y + height, HorseTarget.position.z) - HorseTarget.forward * distance;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

                transform.LookAt(HorseTarget);
                runRotate = transform.rotation;
                break;
            case GameManager.GameState.Boss:
                Vector3 BossFightPos = BossFight.transform.position;
                Vector3 BosstargetPos = new Vector3(BossFightPos.x, BossFightPos.y + 7, BossFightPos.z) + BossFight.transform.right * 5f - BossFight.transform.forward * 7f;
                transform.position = BosstargetPos;

                transform.LookAt(BossFight.transform);
                break;
            case GameManager.GameState.Trace:
                transform.position = playerEye.transform.position;
                transform.position += transform.up * 0.2f;

                transform.LookAt(BossHorseTarget);
                break;
            case GameManager.GameState.GameOver:
                switch (GameManager.instance.PrevState)
                {

                    case GameManager.GameState.Play:
                        break;
                    case GameManager.GameState.Trace:
                        targetPos = new Vector3(BossHorseTarget.position.x, BossHorseTarget.position.y + height, BossHorseTarget.position.z) - BossHorseTarget.forward * distance;
                        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

                        transform.LookAt(BossHorseTarget);
                        break;
                    case GameManager.GameState.Boss:
                        break;
                    default:
                        break;
                }
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
                transform.LookAt(boss.position);
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
