using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InGameUI InGameUI;
    
    Vector3 playerPos;

    private int HorseSkinNumber = 0;
    public int HorseSkin
    {
        get
        {
            return HorseSkinNumber;
        }
        set
        {
            HorseSkinNumber = value;
            horse.GetComponent<Horse>().SetSkin(value);
        }
    }

    public GameObject cowboy;
    public GameObject horse;

    public GameObject boss;
    public GameObject bossHorse;
    GameObject[] enemys;
    GameObject[] items;
    GameObject[] obstacles;
    GameObject end;
    GameObject mainCamera;
    public GameObject road;
    public SceanScript sceanscri;
    public SoundManager soundscri;

    private List<GameObject> enemylist = new List<GameObject>();
    private List<Vector2> portratePos = new List<Vector2>();
    private List<Image> enemyProgress = new List<Image>();

    private Image bossProgress;

    float totalTime;
    int startCount;

    public float sensitive;
    public float catchRange = 10f;

    float startTime;
    float Timing;
    bool TimingOn;
    bool TraceOn;
    [HideInInspector]
    public bool tooEarly;

    float bossTiming;         
    public float bossTimingRange;
    public float bosstime;
    public float bossTraceBarSpeed;
    int life;
    bool isTouch;

    [HideInInspector]
    public bool bossRun;
    [HideInInspector]
    public int enemyCatch;
    [HideInInspector]
    public int totalEnemyCatch;
    [HideInInspector]
    public int stageSave;
    [HideInInspector]
    public bool soundSave;
    [HideInInspector]
    public bool allClear;

    bool SoundCheak;
    bool finishDirecting;
    bool justOne;

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
        BossRun,
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
            InGameUI.ChangeColor(new Color(0f, 0f, 0f, 0f));
            totalTime = 0f;
            finishDirecting = false;
            Time.timeScale = 1f;
            SoundCheak = false;
            justOne = false;
            gameState = value;
            InGameUI.StateInit(value);
            horse.GetComponent<Horse>().StateInit(value);
            cowboy.GetComponent<Cowboy>().StateInit(value);
            boss.GetComponent<Boss>().StateInit(value);
            mainCamera.GetComponent<CameraMove>().StateInit(value);
            bossHorse.GetComponent<BossHorse>().StateInit(value);

            foreach (var enemy in enemys)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<Enemy>().StateInit(value);
                }
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
                    SoundManager.instance.GameStart();
                    SoundManager.instance.BgmStart();
                    break;
                case GameState.Play:
                    prevState = GameState.Play;
                    break;
                case GameState.Trace:
                    SoundManager.instance.BgmStop();
                    SoundManager.instance.TraceSound();
                    startTime = Time.realtimeSinceStartup;
                    prevState = GameState.Trace;
                    

                    //???? ?????? ??????
                    //InGameUI.ChangeColor(new Color(0.6f, 0.1f, 0.1f, 0.5f));
                    InGameUI.ChangeColor(new Color32(112, 66,20,122));

                    //bossTimingRange = 0.2f; //????  0~1
                    bossTiming = Random.Range(bossTimingRange, 1- bossTimingRange);
                    var bossTimingImage = InGameUI.bossBattle.GetComponentInChildren<test>();
                    var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
                    var sliderWidth = slider.gameObject.GetComponent<RectTransform>().sizeDelta.x;

                    bossTimingImage.Setting(bossTiming, sliderWidth, bossTimingRange);

                    Time.timeScale = 0.2f;
                    break;
                case GameState.RunOver:
                    prevState = GameState.BossRun;
                    InGameUI.FadeInOut(5f);
                    break;
                case GameState.Boss:
                    prevState = GameState.Boss;
                    SoundManager.instance.VsBossSound();
                    Timing = Random.Range(2.5f, 7f); // ?????? ???? ????
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
                    SoundManager.instance.StopSound();
                    //?????????? ???? ?????? ??????
                    TimingOn = false;
                    tooEarly = false;
                    TraceOn = false;
                    isTouch = false;
                    break;
                default:
                    break;
            }
        }
    }
    private void Awake()
    {
        instance = this;
        cowboy = GameObject.FindWithTag("Player");
        horse = GameObject.FindGameObjectWithTag("Horse");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Item");
        obstacles = GameObject.FindGameObjectsWithTag("Human");
        boss = GameObject.FindGameObjectWithTag("Boss");
        end = GameObject.FindGameObjectWithTag("end");
        bossHorse = GameObject.FindGameObjectWithTag("BossHorse");
        road = GameObject.FindGameObjectWithTag("road");
        //LoadGame
        var gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            HorseSkin = gameData.horseSkin;
            totalEnemyCatch = gameData.totalEnemy;
            stageSave = gameData.stageSave;
            allClear = gameData.allClear;

            soundscri.SoundButton(gameData.soundSave);
            if (gameData.bossRun)
            {
                horse.GetComponent<Horse>().SpeedUpValue += 1f;
            }
        }
    }
    void Start()
    {
        startCount = 3;
        state = GameState.Idle;

        ////LoadGame
        //var gameData = SaveSystem.LoadGame();
        //if (gameData != null)
        //{
        //    HorseSkin = gameData.horseSkin;
        //    totalEnemyCatch = gameData.totalEnemy;
        //    stageSave = gameData.stageSave;
        //    allClear = gameData.allClear;

        //    soundscri.SoundButton(gameData.soundSave);
        //    if(gameData.bossRun)
        //    {
        //        horse.GetComponent<Horse>().SpeedUpValue += 1f;
        //    }
        //}
    }

    void Update()
    {
        //Debug.Log(state);
        //Debug.Log(enemyCatch);
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
            case GameState.BossRun:
                BossRunUpdate();
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
        enemyCatch++;
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
        state = GameState.BossRun;
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
        //???????? ?????? ????
        float speedValue = horseScript.SpeedUpValue;
        InGameUI.speedUpBar.GetComponentInChildren<SpeedBar>().SettingValue(speedValue / 4f);

        //?????? ???????????? ???????? ???????? ????
        if(Vector3.Distance(horse.transform.position,bossHorse.transform.position) <= catchRange)
        {
            state = GameState.Trace;
        }
    }

    private void TraceUpdate()
    {
        if(finishDirecting)
        {
            state = GameState.RunOver;
        }
        ProgressSetting();
        //???? ???????? ????????! 
        totalTime = Time.realtimeSinceStartup - startTime;
        Debug.Log(TraceOn);
        //????????
        //totalTime += Time.deltaTime;
        if (!TraceOn)
        {
            //???????? 1?????? ????
            if(totalTime > 1f)
            {
                startTime = Time.realtimeSinceStartup;
                TraceOn = true;
                InGameUI.bossBattle.SetActive(true);
            }
        }
        else
        {
            
            var timeSpeed = bossTraceBarSpeed;// ?????? ???????? ???? 
            //???????? ????
            //if (totalTime >= timeSpeed) 
            //{
            //    totalTime = 0f;
            //}
            //??????
            if (totalTime >= timeSpeed && !isTouch)
            {
                state = GameState.GameOver;
            }

            var nowTiming = totalTime / timeSpeed;
            if (nowTiming >= 1f)
            {
                nowTiming = 2f - nowTiming;
            }

            var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
            slider.value = nowTiming;

            //Debug.Log(bossTiming);
            if (Input.touchCount == 1)
            {
                isTouch = true;
                InGameUI.ChangeColor(new Color(0f, 0f, 0f, 0f));
                Time.timeScale = 1f;
                InGameUI.GetComponent<InGameUI>().bossBattle.SetActive(false);
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log($"{slider.value} , {bossTiming}");

                    if (slider.value > bossTiming - bossTimingRange && slider.value < bossTiming + bossTimingRange)
                    {
                        cowboy.GetComponent<Cowboy>().SetAnimation(Cowboy.PlayerAnimation.MountShoot);
                        //state = GameState.RunOver;
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
        }
    }
    private void ProgressSetting()
    {
        float value = (float)horse.GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
        var progressbar = InGameUI.GetComponentInChildren<ProgressBar>();
        var sliderWidth = (float)progressbar.GetComponent<RectTransform>().sizeDelta.x;

        //???????? ?????? ????
        progressbar.SettingValue(value);

        //?????? ?????? ????
        for (int i = 0; i < enemyProgress.Count; i++)
        {
            if (enemyProgress[i] != null && enemylist[i] != null)
            {
                var enemyPos = (float)enemylist[i].GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
                enemyProgress[i].GetComponent<EnemyProgress>().SettingValue(enemyPos, sliderWidth);
            }
        }

        //???? ?????? ????
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
        if (totalTime >= Timing + bosstime)
        {
            state = GameState.GameOver;
        }
    }
    private void BossRunUpdate()
    {
        if(finishDirecting)
        {
            state = GameState.GameOver;
        }
    }
    private void GameOverUpdate()
    {
        if (finishDirecting)
        {
            InGameUI.GetComponent<InGameUI>().OnGameOver();
            if(!SoundCheak)
            {
                SoundManager.instance.GameOverSound();
                SoundCheak = true;
            }
        }
    }
    private void FinishUpdate()
    {
        if(finishDirecting)
        {
            if (!justOne)
            {
                InGameUI.GetComponent<InGameUI>().OnClear(enemyCatch);
                cowboy.GetComponent<Cowboy>().ClearDirecting(enemyCatch);
                
                //??????
                totalEnemyCatch += enemyCatch;
                stageSave = sceanscri.NextIndex;
                soundSave = soundscri.soundOnOff.isOn;
                if(sceanscri.CurIndex == 30)
                {
                    allClear = true;
                }
                SaveSystem.SaveGame(this);

                justOne = true;
            }
            if (!SoundCheak)
            {
                SoundManager.instance.GameClear();
                SoundCheak = true;
            }
        }
    }
    private void RestartUpdate()
    {
        switch (prevState)
        {
            case GameState.Play:
                if (finishDirecting)
                {
                    totalTime += Time.deltaTime;
                    if (totalTime > 2f)
                    {
                        state = prevState;
                    }
                }
                break;
            case GameState.Trace:
            case GameState.Boss:
                if (finishDirecting)
                {
                    state = prevState;
                }
                break;
            case GameState.BossRun:
                bossRun = true;
                SaveGame();
                break;
            default:
                break;
        }
        //if (finishDirecting)
        //{
        //    state = prevState;
        //}
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

    public void OnOffSpeedUpText(bool value)
    {
        InGameUI.GetComponent<InGameUI>().speedUpText.SetActive(value);
    }
    public void GameStart() // ???? ??????
    {
        state = GameState.Start;
    }
    public void GameOver() // ???? ????
    {
        state = GameState.GameOver;
    }
    public void GameReStart() // ???? ????????(???? ????)
    {
        GoogleMobileAdTest.OnClickRetry();
        
        //state = GameState.ReStart;
    }
    public void FinishDirecting() // ?????????? ????
    {
        Debug.Log("finishdirecting");
        finishDirecting = true;
    }
    public void SaveGame()
    {
        stageSave = sceanscri.CurIndex;
        SaveSystem.SaveGame(this);
    }
}