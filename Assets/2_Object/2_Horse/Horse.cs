using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;


public class Horse : MonoBehaviour
{
    public GameObject SpeedUpParticle;

    Animator ani;

    float speedUpTime;
    float totalspeedUpTime;
    float speedUp;
    bool isSpeedDown;
    bool isSpeedZero = true;

    Vector3 SleepPos;

    private Vector3 prevPos;

    public float speedDown = 0.5f;

    public MalbersAnimations.Controller.MAnimal animal;
    public bool SpeedZero
    {
        get 
        {
            return isSpeedZero;
        }
    }
    public float SpeedUpValue
    {
        get
        {
            return speedUp;
        }
        set
        {
            speedUp = value;
        }
    }
    private ItemList playerGetItem;
    public ItemList playerItem
    {
        get
        {
            return playerGetItem;
        }
    }
    //public enum HorseState
    //{
    //    Idle,
    //    Run,
    //    Sleep,
    //    Death
    //}
    //private HorseState horseState;
    //public HorseState hstate
    //{
    //    get
    //    {
    //        return horseState;
    //    }
    //    set
    //    {
    //        horseState = value;
    //        GameManager.instance.HorseStateInit(value);
    //        switch (value)
    //        {
    //            case HorseState.Run:
    //                break;
    //            case HorseState.Sleep:
    //                follow.follow = false;
    //                ani.SetInteger("State", 0);
    //                break;
    //            case HorseState.Death:
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}
    Touch touch;
    public float speed = 5f;
    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
        ani = GetComponent<Animator>();
        follow = GetComponent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
        follow.followSpeed = speed;
        //horseState = HorseState.Idle;
        //StartCoroutine(CoSpeedDown());
    }
    void Update()
    {
        transform.rotation = follow.transform.rotation;

        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                //이펙트 재생을 위한 현재 아이템 보유 상태 확인
                if (CheckItem(ItemList.SpeedUp))
                {
                    SpeedUpUpdate();
                }

                // 아이템에 따른 스피드 설정
                follow.followSpeed = speed + (int)speedUp;

                // 애니메이션 설정
                animal.SetFloatParameter(animal.hash_Vertical, 1f + speedUp);
                animal.SetFloatParameter(animal.hash_Horizontal, 0f);

                //좌우 이동
#if UNITY_EDITOR
                var h = Input.GetAxis("Horizontal");
                if (h != 0)
                {
                    follow.motion.offset = new Vector2(follow.motion.offset.x + h * 10f * Time.deltaTime, follow.motion.offset.y);
                }
                //transform.position += transform.right * h * 10f * Time.deltaTime;
#endif
#if UNITY_ANDROID
                if (Input.touchCount == 1)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        follow.motion.offset = new Vector2(follow.motion.offset.x + touch.deltaPosition.x * 0.01f, follow.motion.offset.y);
                        //transform.position += transform.right * touch.deltaPosition.x * 0.01f;
                    }
                }
#endif
                //게이지 감소 
                if(speedUp>0)
                {
                    speedUp -= (Time.deltaTime * speedDown)/4f;
                    isSpeedZero = false;
                }
                if(speedUp <= 0)
                {
                    speedUp = 0;
                    isSpeedZero = true;
                }
                break;
            case GameManager.GameState.Boss:
                break;
            default:
                break;
        }


    }
    private void OnCollisionEnter(Collision other)
    {
        var collisionable = other.gameObject.GetComponentsInChildren<ICollisionAble>();
        foreach (var collision in collisionable)
        {
            collision.nowCollision(gameObject);
        }
    }
    public void GetItem(ItemList type, float itemValue, float itemValue2)
    {
        switch (type)
        {
            case ItemList.SpeedUp:
                totalspeedUpTime = 0f;
                speedUpTime = itemValue2;
                //follow.followSpeed += itemValue;
                if (!SpeedUpParticle.activeSelf)
                {
                    SpeedUpParticle.SetActive(true);
                }
                speedUp += itemValue;
                if(speedUp >= 4f)
                {
                    speedUp = 4f;
                }
                break;
            case ItemList.None:
                break;
            default:
                break;
        }
        playerGetItem |= type;
    }

    private void SpeedUpUpdate()
    {
        totalspeedUpTime += Time.deltaTime;
        if (totalspeedUpTime >= speedUpTime)
        {
            totalspeedUpTime = 0f;
            SpeedUpParticle.SetActive(false);
            playerGetItem &= (~ItemList.SpeedUp);
        }
    }
    public bool CheckItem(ItemList type)
    {
        return ((playerItem & type) != 0);
    }
    public void StateInit(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                //horseState = HorseState.Run;
                animal.AlwaysForward = true;
                follow.follow = true;
                break;
            case GameManager.GameState.Trace:
                //이동하세요
                animal.AlwaysForward = true;
                follow.follow = true;

                //장애물에 부딪혀도 종료안되게
                isSpeedZero = false;
                //스피드업 이펙트 끄기
                SpeedUpParticle.SetActive(false);

                // 보스의 이동속도랑 동일하게 추격
                var bossHorse = GameObject.FindGameObjectWithTag("BossHorse");
                var bossSpeed = bossHorse.GetComponent<BossHorse>().speed;
                follow.followSpeed = bossSpeed;

                // 애니메이션 걷기로 설정 (뛰면 눈이 아래로 향해서 너무 어지러움)
                animal.SetFloatParameter(animal.hash_Vertical, 2f);
                animal.SetFloatParameter(animal.hash_Horizontal, 0f);
                break;
            case GameManager.GameState.RunOver:
                animal.AlwaysForward = false;
                follow.follow = false;
                break;
            case GameManager.GameState.Boss:
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                follow.follow = false;
                ani.SetInteger("State", 0);
                break;
            case GameManager.GameState.ReStart:
                switch (GameManager.instance.PrevState)
                {
                    case GameManager.GameState.Play:
                        animal.Mode_Stop();
                        StartCoroutine(CoMoveToTarget(1f,0.8f, SleepPos));
                        break;
                    case GameManager.GameState.Trace:
                        FinishDirecting();
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

    public void disMount()
    {
        SleepPos = transform.position;
        StartCoroutine(CoDisMount());
        //animal.ForceAction(4, 6);
        animal.Mode_TryActivate(4, 6);
    }
    IEnumerator CoDisMount()
    {
        var targetPos = transform.position - transform.forward * 2f - transform.right * 2f;
        yield return StartCoroutine(CoMoveToTarget(0.5f,0.6f,targetPos));
        yield return new WaitForSeconds(0.6f);  
        //ani.SetInteger("ModeStatus", 2);
        var cowboy = GameManager.instance.cowboy;
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        if (GameManager.instance.PrevState == GameManager.GameState.Play)
        {
            cowboyScript.SetAnimation(Cowboy.PlayerAnimation.Sad);
        }
    }
    IEnumerator CoMoveToTarget(float delay, float duration, Vector3 targetPos)
    {
        yield return new WaitForSeconds(delay);
        var totalTime = 0f;
        while (totalTime <= duration)
        {
            Debug.Log(targetPos);
            totalTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, totalTime / duration);
            yield return null;
        }
        //animal.PlayAction(4,6);
        //animal.Mode_TryActivate(4, 6);

        //ani.SetInteger("ModeStatus", 0);
        //ani.SetInteger("Mode", 4006);
        //ani.SetInteger("State", 5);
        
        var cowboy = GameObject.FindGameObjectWithTag("Player");
        var boss = GameObject.FindGameObjectWithTag("Boss");
        cowboy.transform.LookAt(boss.transform.position);
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}
