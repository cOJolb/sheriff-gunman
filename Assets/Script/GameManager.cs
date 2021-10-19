using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector3 playerPos;

    GameObject player;
    GameObject boss;
    GameObject[] enemys;
    GameObject[] items;
    GameObject[] humans;
    GameObject end;
    GameObject mainCamera;

    private float power;

    float totalTime;

    int startCount;
    int bossHit = 5;
    int imsiMax = 30;
    Touch touch;

    public static GameManager instance;
    
    public enum GameState
    {
        Start,
        Play,
        Boss,
        finish
    }
    private GameState gameState;
    public GameState state
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.Start:
                    break;
                case GameState.Play:
                    break;
                case GameState.Boss:
                    break;
                case GameState.finish:
                    //Debug.Log("test");
                    var hash = iTween.Hash("position",boss.transform.position, "speed", 10f, /*"easetype", iTween.EaseType.linear,*/ "looptype", iTween.LoopType.none, "oncomplete", "BackWard", "oncompletetarget", gameObject);
                    iTween.MoveTo(player, hash);
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        state = GameState.Start;
        startCount = 3;
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Item");
        humans = GameObject.FindGameObjectsWithTag("Human");
        boss = GameObject.FindGameObjectWithTag("Boss");
        end = GameObject.FindGameObjectWithTag("end");

        playerPos = end.transform.position;
    }

    void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case GameState.Start:
                StartUpdate();
                break;
            case GameState.Play:
                //PlayUpdate();
                break;
            case GameState.Boss:
                BossUpdate();
                break;
            case GameState.finish:
                break;
            default:
                break;
        }
    }
    public void EnemyCollision(GameObject Enemy)
    {
        var rigid = Enemy.GetComponent<Rigidbody>();
        rigid.AddForce(player.transform.forward * 50f + Vector3.up * 50f, ForceMode.Impulse);
    }
    public void HumanCollision()
    {
        player.transform.position = new Vector3(0, 5, 0);
        state = GameState.Start;
    }
    public void ItemCollision(float _power)
    {
        power += _power;
        var EquipItem = player.GetComponentInChildren<EquipItem>();
        EquipItem.transform.localScale *= 1f + _power /10f;
    }
    public void endCollision()
    {
        state = GameState.Boss;
    }
    private void StartUpdate()
    {
        totalTime += Time.deltaTime;
        if (totalTime >= 1f)
        {
            totalTime = 0f;
            startCount--;
        }
        if (startCount <= 0)
        {
            state = GameState.Play;
        }
    }
    private void BossUpdate()
    {
        totalTime += Time.deltaTime;

        if(Input.touchCount == 1)
        {
            touch = Input.touches[0];
            if(touch.phase == TouchPhase.Began)
            {
                bossHit++;
                Debug.Log($"{bossHit}");
            }
        }
        if(bossHit >= imsiMax || totalTime >= 3f)
        {
            totalTime = 0f;
            state = GameState.finish;
            //Debug.Log("test");
        }
    }
    private void BackWard()
    {
        boss.GetComponent<Rigidbody>().AddForce(player.transform.forward * 50f + Vector3.up * 10f, ForceMode.Impulse);
        iTween.MoveTo(player, iTween.Hash("position", playerPos, "speed", 5f, "easetype", iTween.EaseType.linear));
    }
}
