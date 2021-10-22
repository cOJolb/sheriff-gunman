using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InGameUI InGameUI;
    public Slider progressbar;

    Vector3 playerPos;

    GameObject player;
    GameObject boss;
    GameObject[] enemys;
    GameObject[] items;
    GameObject[] humans;
    GameObject end;
    GameObject mainCamera;

    private List<GameObject> enemylist = new List<GameObject>();
    private List<Vector2> portratePos = new List<Vector2>();
    private List<Image> enemyProgress = new List<Image>();
    //private Image[] enemyProgresses;
    float totalTime;
    int startCount;
    

    //float bossTiming;         타이밍바
    //float bossTimingRange;    타이밍바
    //int life;                 타이밍바

    float Timing;
    bool TimingOn;

    Touch touch;

    public static GameManager instance;
    
    public enum GameState
    {
        Start,
        Play,
        Boss,
        finish,
        GameOver
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
            InGameUI.UiInit(value);
            
            switch (gameState)
            {
                case GameState.Start:
                    for (int i = 0; i < enemys.Length; i++)
                    {
                        enemylist.Add(enemys[i]);
                        enemyProgress.Add(InGameUI.EnemyProgress());
                    }
                    break;
                case GameState.Play:
                    for (int i = 0; i < enemys.Length; i++)
                    {
                        //현상금수배지
                        //var newGo = Instantiate(enemys[i].GetComponent<Enemy>().portrait, InGameUI.transform);
                        //var image = newGo.GetComponent<Image>();
                        //var width = image.rectTransform.sizeDelta.x;
                        //var height = image.rectTransform.sizeDelta.y;
                        //image.rectTransform.anchoredPosition = new Vector2(width / 2, -height / 2 - i * (height+10f));
                        //portratePos.Add(image.rectTransform.anchoredPosition);
                        
                    }
                    break;
                case GameState.Boss:
                    //bossTimingRange = 0.2f; //범위  
                    //bossTiming = Random.Range(bossTimingRange, 1- bossTimingRange);
                    //var bossTimingImage = InGameUI.bossBattle.GetComponentInChildren<test>();
                    //var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
                    //var sliderWidth = slider.gameObject.GetComponent<RectTransform>().sizeDelta.x;

                    //bossTimingImage.Setting(bossTiming, sliderWidth, bossTimingRange);
                    //타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바

                    Timing = Random.Range(1f, 5f); // 타이밍 범위 지정
                    
                    break;
                case GameState.finish:
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
        startCount = 3;
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Item");
        humans = GameObject.FindGameObjectsWithTag("Human");
        boss = GameObject.FindGameObjectWithTag("Boss");
        end = GameObject.FindGameObjectWithTag("end");
        playerPos = end.transform.position;
        state = GameState.Start;
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
            case GameState.Boss:
                BossUpdate();
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
        //현상금수배지
        //var checkPos = portratePos[index];    
        //var newGo = Instantiate(InGameUI.enemyCheck, InGameUI.transform);
        //newGo.rectTransform.anchoredPosition = checkPos;
        // life++;  목숨 추가
        return index;
    }
    public void EnemyRun(GameObject Enemy)
    {
        
    }
    public void HumanCollision()
    {
        player.transform.position = new Vector3(0, 5, 0);
        state = GameState.Start;
    }
    public void ItemCollision()
    {

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
    private void PlayUpdate()
    {
        float value = (float)player.GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
        InGameUI.GetComponentInChildren<ProgressBar>().SettingValue(value);
        for (int i = 0; i < enemyProgress.Count; i++)
        {
            if(enemyProgress[i] != null)
            {
                var enemyPos = (float)enemylist[i].GetComponent<Dreamteck.Splines.SplineFollower>().GetPercent();
                var sliderWidth = (float)progressbar.GetComponent<RectTransform>().sizeDelta.x;
                enemyProgress[i].GetComponent<EnemyProgress>().SettingValue(enemyPos, sliderWidth);
            }
        }
    }
    private void BossUpdate()
    {
        //        totalTime += Time.deltaTime;
        //        var timeSpeed = 2f;// 게이지 왔다갔다 속도 
        //        if (totalTime >= timeSpeed)
        //        {
        //            totalTime = 0f;
        //        }

        //        var nowTiming = totalTime / (timeSpeed/2f);
        //        if(nowTiming>= 1f)
        //        {
        //            nowTiming = 2f - nowTiming;
        //        }

        //        var slider = InGameUI.bossBattle.GetComponentInChildren<Slider>();
        //        slider.value = nowTiming;

        //#if UNITY_EDITOR
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //           if (slider.value > bossTiming - bossTimingRange && slider.value < bossTiming + bossTimingRange)
        //           {
        //               state = GameState.finish;
        //           }
        //           else
        //           {
        //               if (life > 0)
        //               {
        //                   life--;
        //               }
        //               else
        //               {
        //                   state = GameState.GameOver;
        //               }
        //           }
        //        }
        //#endif
        //#if UNITY_ANDROID

        //        //Debug.Log(bossTiming);
        //        if (Input.touchCount == 1)
        //        {
        //            touch = Input.GetTouch(0);
        //            if (touch.phase == TouchPhase.Began)
        //            {
        //                Debug.Log($"{slider.value} , {bossTiming}");

        //                if (slider.value > bossTiming - bossTimingRange && slider.value < bossTiming + bossTimingRange)
        //                {
        //                    state = GameState.finish;
        //                }
        //                else
        //                {
        //                    if (life > 0)
        //                    {
        //                        life--;
        //                    }
        //                    else
        //                    {
        //                        state = GameState.GameOver;
        //                    }
        //                }
        //            }
        //        }
        //#endif        타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바타이밍바


        totalTime += Time.deltaTime;
        
        if(totalTime >= Timing)
        {
            if(!TimingOn)
            {
                InGameUI.TimingOn();
                TimingOn = true;
            }
            TimingTouch();
        }
        if (totalTime >= Timing + boss.GetComponent<Boss>().Timing)
        {
            state = GameState.GameOver;
        }
    }
    private void BackWard()
    {
        boss.GetComponent<Rigidbody>().AddForce(player.transform.forward * 50f + Vector3.up * 10f, ForceMode.Impulse);
        iTween.MoveTo(player, iTween.Hash("position", playerPos, "speed", 5f, "easetype", iTween.EaseType.linear));
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
}
