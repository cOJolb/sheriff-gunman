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

    public GameObject cowboy;
    public GameObject horse;

    public GameObject boss;
    public GameObject bossHorse;
    GameObject[] enemys;
    GameObject[] items;
    GameObject[] obstacles;
    GameObject end;
    GameObject mainCamera;

    private List<GameObject> enemylist = new List<GameObject>();
    private List<Vector2> portratePos = new List<Vector2>();
    private List<Image> enemyProgress = new List<Image>();

    private Image bossProgress;

    float totalTime;
    int startCount;

    float Timing;
    bool TimingOn;
    public bool tooEarly;

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

    //Horse.HorseState horseState = Horse.HorseState.Run;
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
    private GameState prevState;

    public GameState PrevState
    {
        get
        {
            return prevState;
        }
    }
    public GameState state
    {
        get
        {
            return gameState;
        }
        set
        {
            totalTime = 0f;
            finishDirecting = false;

            gameState = value;
            InGameUI.StateInit(value);
            horse.GetComponent<Horse>().StateInit(value);
            cowboy.GetComponent<Cowboy>().StateInit(value);
            boss.GetComponent<Boss>().StateInit(value);
            mainCamera.GetComponent<CameraMove>().StateInit(value);
            bossHorse.GetComponent<BossHorse>().StateInit(value);
            foreach (var enemy in enemys)
            {
                enemy.GetComponent<Enemy>().StateInit(value);
            }
            switch (gameState)
            {
                case GameState.Start:
                    for (int i = 0; i < enemys.Length; i++)
                    {
                        enemylist.Add(enemys[i]);
                        var enemyScript = enemys[i].GetComponent<Enemy>();
                        enemyProgress.Add(InGameUI.EnemyProgress(enemyScript.progress));
                    }
                    bossProgress = InGameUI.EnemyProgress(bossHorse.GetComponent<BossHorse>().progress);
                    break;
                case GameState.Play:
                    prevState = GameState.Play;
                    break;
                case GameState.Trace:
                    prevState = GameState.Trace;
                    bossTimingRange = 0.2f; //범위  0~1
                    bossTiming = Random.Range(bossTimingRange, 1- bossTimingRange);
                    var bossTimingImage = InGameUI.bossBattle.GetComponentInChildren<test>();
                    var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
                    var sliderWidth = slider.gameObject.GetComponent<RectTransform>().sizeDelta.x;

                    bossTimingImage.Setting(bossTiming, sliderWidth, bossTimingRange);
                    break;
                case GameState.RunOver:
                    FadeInOut(5f);
                    break;
                case GameState.Boss:
                    prevState = GameState.Boss;
                    Timing = Random.Range(1f, 5f); // 타이밍 범위 지정
                    break;
                case GameState.finish:
                    break;
                case GameState.GameOver:
                    if (/*horseState != Horse.HorseState.Sleep && */!tooEarly && prevState != GameState.Play)
                    {
                        boss.GetComponent<Boss>().BossShoot();
                    }
                    break;
                case GameState.ReStart:
                    //리스타트를 위한 보스전 초기화
                    TimingOn = false;
                    tooEarly = false;

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
        //Debug.Log(state);
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
            case GameState.RunOver:
                RunOverUpdate();
                break;
            case GameState.Boss:
                BossUpdate();
                break;
            case GameState.GameOver:
                GameOverUpdate();
                break;
            case GameState.finish:
                FinishUpdate();
                break;
            case GameState.ReStart:
                RestartUpdate();
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
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        if (horseScript.SpeedZero)
        {
            cowboyScript.SetAnimation(Cowboy.PlayerAnimation.DisMount);
            state = GameState.GameOver;
        }
    }
    public void ItemCollision()
    {

    }
    public void endCollision()
    {
        //StartCoroutine(CoBossStart());
    }
    private void FadeInOut(float duration)
    {
        var fadeinout = InGameUI.GetComponentInChildren<FadeInOut>();
        var img = fadeinout.GetComponent<Image>();

        StartCoroutine(fadeinout.CoFadeInOut(img.color, Color.black, duration));
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
            Debug.Log("playplay");
            state = GameState.Play;
        }
    }
    private void PlayUpdate()
    {
        var horseScript = horse.GetComponent<Horse>();
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        ProgressSetting();
        //스피드업 게이지 세팅
        float speedValue = horseScript.SpeedUpValue;
        InGameUI.speedUpBar.GetComponentInChildren<SpeedBar>().SettingValue(speedValue / 4f);

        //보스와 일정거리만큼 좁혀지면 추격상태 돌입
        if(Vector3.Distance(horse.transform.position,bossHorse.transform.position) <= catchRange)
        {
            state = GameState.Trace;
        }
    }
    private void TraceUpdate()
    {
        ProgressSetting();

        //타이밍바
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

        //타이밍바 조작
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
    private void ProgressSetting()
    {
        float value = (float)horse.GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
        var progressbar = InGameUI.GetComponentInChildren<ProgressBar>();
        var sliderWidth = (float)progressbar.GetComponent<RectTransform>().sizeDelta.x;

        //플레이어 진행바 세팅
        progressbar.SettingValue(value);

        //악당들 진행바 세팅
        for (int i = 0; i < enemyProgress.Count; i++)
        {
            if (enemyProgress[i] != null && enemylist[i] != null)
            {
                var enemyPos = (float)enemylist[i].GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
                enemyProgress[i].GetComponent<EnemyProgress>().SettingValue(enemyPos, sliderWidth);
            }
        }

        //보스 진행바 세팅
        var bossPos = (float)bossHorse.GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
        bossProgress.GetComponent<EnemyProgress>().SettingValue(bossPos, sliderWidth);

    }
    private void RunOverUpdate()
    {
        if(finishDirecting)
        {
            state = GameState.Boss;
        }
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
        if (finishDirecting)
        {
            InGameUI.GetComponent<InGameUI>().OnGameOver();
        }
    }
    private void FinishUpdate()
    {
        if(finishDirecting)
        {
            InGameUI.GetComponent<InGameUI>().OnClear();
        }
    }
    private void RestartUpdate()
    {
        //switch (prevState)
        //{
        //    case GameState.Play:
        //        totalTime += Time.deltaTime;
        //        if(totalTime > 2f)
        //        {
        //            state = prevState;
        //        }
        //        break;
        //    case GameState.Trace:
        //    case GameState.Boss:
        //        if (finishDirecting)
        //        {
        //            state = prevState;
        //        }
        //        break;
        //    default:
        //        break;
        //}
        if (finishDirecting)
        {
            state = prevState;
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
                    tooEarly = false;
                    state = GameState.finish;
                }
                else
                {
                    tooEarly = true;
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
                tooEarly = false;
                state = GameState.finish;
            }
            else
            {
                tooEarly = true;
                state = GameState.GameOver;
            }
        }
    }
    //public void HorseStateInit(Horse.HorseState value)//말의 상태에 따른 
    //{
    //    horseState = value;
    //    switch (value)
    //    {
    //        case Horse.HorseState.Run:
    //            break;
    //        case Horse.HorseState.Sleep:
    //            var cowboyScript = cowboy.GetComponent<Cowboy>();
    //            cowboyScript.SetAnimation(Cowboy.PlayerAnimation.DisMount);
    //            state = GameState.GameOver;
    //            break;
    //        case Horse.HorseState.Death:
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //public void BossRun()//말이 움직이지 못하는 상태 ?
    //{
    //    horse.GetComponent<Horse>().hstate = Horse.HorseState.Sleep;
    //}
    public void GameStart() // 게임 스타트
    {
        state = GameState.Start;
    }
    public void GameReStart() // 게임 리스타트(광고 클릭)
    {
        state = GameState.ReStart;
    }
    public void FinishDirecting() // 종료연출이 끝남
    {
        Debug.Log("finishdirecting");
        finishDirecting = true;
    }
}
