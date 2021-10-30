using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InGameUI InGameUI;
    public float catchRange = 10f;
    Vector3 playerPos;

    //Cowboy cowboy;
    //Horse horse;

    //Boss boss;
    //Enemy[] enemys;
    //Item[] items;
    //Obstacle[] obstacles;
    //endSpace end;
    //CameraMove mainCamera;

    GameObject cowboy;
    GameObject horse;

    GameObject boss;
    GameObject bossHorse;
    GameObject[] enemys;
    GameObject[] items;
    GameObject[] obstacles;
    GameObject end;
    GameObject mainCamera;

    private List<GameObject> enemylist = new List<GameObject>();
    private List<Vector2> portratePos = new List<Vector2>();
    private List<Image> enemyProgress = new List<Image>();
    //private Image[] enemyProgresses;
    float totalTime;
    int startCount;

    float Timing;
    bool TimingOn;
    bool bossFightStart;

    float bossTiming;         
    float bossTimingRange;    
    int life;                 

    bool finishDirecting;
    public bool finishAction
    {
        get
        {
            return finishDirecting;
        }
        set
        {
            finishDirecting = value;
        }
    }


    Touch touch;

    public static GameManager instance;

    Horse.HorseState horseState = Horse.HorseState.Run;
    public enum GameState
    {
        Idle,
        Start,
        Play,
        Trace,
        RunOver,
        Boss,
        finish,
        GameOver,
        ReStart
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
            totalTime = 0f;
            gameState = value;
            InGameUI.StateInit(value);
            horse.GetComponent<Horse>().StateInit(value);
            cowboy.GetComponent<Cowboy>().StateInit(value);
            boss.GetComponent<Boss>().StateInit(value);
            mainCamera.GetComponent<CameraMove>().StateInit(value);
            bossHorse.GetComponent<BossHorse>().StateInit(value);
            switch (gameState)
            {
                case GameState.Start:
                    for (int i = 0; i < enemys.Length; i++)
                    {
                        enemylist.Add(enemys[i]);
                        var enemyScript = enemys[i].GetComponent<Enemy>();
                        enemyProgress.Add(InGameUI.EnemyProgress(enemyScript.progress));
                    }
                    break;
                case GameState.Play:
                    for (int i = 0; i < enemys.Length; i++)
                    {
                    }
                    break;
                case GameState.Trace:
                    bossTimingRange = 0.2f; //범위  0~1
                    bossTiming = Random.Range(bossTimingRange, 1- bossTimingRange);
                    var bossTimingImage = InGameUI.bossBattle.GetComponentInChildren<test>();
                    var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
                    var sliderWidth = slider.gameObject.GetComponent<RectTransform>().sizeDelta.x;

                    bossTimingImage.Setting(bossTiming, sliderWidth, bossTimingRange);
                    break;
                case GameState.RunOver:
                    StartCoroutine(CoBossStart());
                    break;
                case GameState.Boss:
                    Timing = Random.Range(1f, 5f); // 타이밍 범위 지정
                    break;
                case GameState.finish:
                    break;
                case GameState.GameOver:
                    if (horseState != Horse.HorseState.Sleep)
                    {
                        boss.GetComponent<Boss>().BossShoot();
                    }
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
        startCount = 3;
        cowboy = GameObject.FindWithTag("Player");
        horse = GameObject.FindGameObjectWithTag("Horse");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Item");
        obstacles = GameObject.FindGameObjectsWithTag("Human");
        boss = GameObject.FindGameObjectWithTag("Boss");
        end = GameObject.FindGameObjectWithTag("end");
        bossHorse = GameObject.FindGameObjectWithTag("BossHorse");
        //playerPos = end.transform.position;
        state = GameState.Idle;
    }

    void Update()
    {
        Debug.Log(state);
        switch (state)
        {
            case GameState.Start:
                StartUpdate();
                break;
            case GameState.Play:
                PlayUpdate();
                break;
            case GameState.Trace:
                TraceUpdate();
                break;
            case GameState.Boss:
                BossUpdate();
                break;
            case GameState.GameOver:
                GameOverUpdate();
                break;
            case GameState.finish:
                break;
            default:
                break;
        }
    }
    public int EnemyCollision(GameObject Enemy)
    {
        var index = enemylist.FindIndex((x) => x == Enemy);
        Destroy(enemyProgress[index]);
        return index;
    }

    public void HumanCollision()
    {
        var horseScript = horse.GetComponent<Horse>();
        if (horseScript.SpeedZero)
        {
            horseScript.hstate = Horse.HorseState.Sleep;
        }
    }
    public void ItemCollision()
    {

    }
    public void endCollision()
    {
        StartCoroutine(CoBossStart());
    }
    IEnumerator CoBossStart()
    {
        var fadeinout = InGameUI.GetComponentInChildren<FadeInOut>();
        var img = fadeinout.GetComponent<Image>();

        yield return StartCoroutine(fadeinout.CoFadeInOut(img.color, Color.black, 2f));
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
    private void PlayUpdate()
    {
        var horseScript = horse.GetComponent<Horse>();
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        float value = (float)horse.GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
        var progressbar = InGameUI.GetComponentInChildren<ProgressBar>();
        progressbar.SettingValue(value);
        for (int i = 0; i < enemyProgress.Count; i++)
        {
            if (enemyProgress[i] != null && enemylist[i] != null)
            {
                var enemyPos = (float)enemylist[i].GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
                var sliderWidth = (float)progressbar.GetComponent<RectTransform>().sizeDelta.x;
                enemyProgress[i].GetComponent<EnemyProgress>().SettingValue(enemyPos, sliderWidth);
            }
        }
        float speedValue = horseScript.SpeedUpValue;
        InGameUI.speedUpBar.GetComponentInChildren<SpeedBar>().SettingValue(speedValue / 2f);

        if(Vector3.Distance(horse.transform.position,bossHorse.transform.position) <= catchRange)
        {
            state = GameState.Trace;
        }
    }
    private void TraceUpdate()
    {
        totalTime += Time.deltaTime;
        var timeSpeed = 2f;// 게이지 왔다갔다 속도 
        if (totalTime >= timeSpeed)
        {
            totalTime = 0f;
        }

        var nowTiming = totalTime / (timeSpeed/2f);
        if(nowTiming>= 1f)
        {
            nowTiming = 2f - nowTiming;
        }

        var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
        slider.value = nowTiming;

        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
           if (slider.value > bossTiming - bossTimingRange && slider.value < bossTiming + bossTimingRange)
           {
               state = GameState.RunOver;
           }
           else
           {
               if (life > 0)
               {
                   life--;
               }
               else
               {
                   state = GameState.GameOver;
               }
           }
        }
        #endif
        #if UNITY_ANDROID

        //Debug.Log(bossTiming);
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log($"{slider.value} , {bossTiming}");

                if (slider.value > bossTiming - bossTimingRange && slider.value < bossTiming + bossTimingRange)
                {
                    state = GameState.RunOver;
                }
                else
                {
                    if (life > 0)
                    {
                        life--;
                    }
                    else
                    {
                        state = GameState.GameOver;
                    }
                }
            }
        }
        #endif    
    }
    private void BossUpdate()
    {
        totalTime += Time.deltaTime;

        if (totalTime >= Timing)
        {
            if (!TimingOn)
            {
                InGameUI.TimingOn();
                TimingOn = true;
            }
        }
        //TimingTouch();
        TimingClick();
        if (totalTime >= Timing + boss.GetComponent<Boss>().Timing)
        {
            state = GameState.GameOver;
        }
    }
    private void GameOverUpdate()
    {
        finishDirecting = true;
        if (finishDirecting)
        {
            InGameUI.GetComponent<InGameUI>().OnGameOver();
            //SetActive(true);
        }
    }
    public void TimingTouch()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (totalTime >= Timing)
                {
                    state = GameState.finish;
                }
                else
                {
                    state = GameState.GameOver;
                }
            }
        }
    }
    public void TimingClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (totalTime >= Timing)
            {
                state = GameState.finish;
            }
            else
            {
                state = GameState.GameOver;
            }
        }
    }
    public void HorseStateInit(Horse.HorseState value)
    {
        horseState = value;
        switch (value)
        {
            case Horse.HorseState.Run:
                break;
            case Horse.HorseState.Sleep:
                var cowboyScript = cowboy.GetComponent<Cowboy>();
                cowboyScript.HorseSleepSoWhat();
                state = GameState.GameOver;
                break;
            case Horse.HorseState.Death:
                break;
            default:
                break;
        }
    }
    public void BossRun()
    {
        horse.GetComponent<Horse>().hstate = Horse.HorseState.Sleep;
    }
    public void GameStart()
    {
        state = GameState.Start;
    }
    public void GameReStart()
    {
        state = GameState.ReStart;
    }
    public void FinishDirecting()
    {
        finishDirecting = true;
    }
    //public enum dataType
    //{
    //    intiger,
    //    boollen,
    //    triger,
    //    floatType
    //}
    //public void AnimationSetting(dataType type, string name, int Ivalue = 0, float Fvalue = 0f,bool Bvalue = false)
    //{

    //}
}
