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
    float speedAni;
    bool isSpeedDown;
    bool isSpeedZero = true;

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
            return speedAni;
        }
        set
        {
            speedAni = value;
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
    public enum HorseState
    {
        Idle,
        Run,
        Sleep,
        Death
    }
    private HorseState horseState;
    public HorseState hstate
    {
        get
        {
            return horseState;
        }
        set
        {
            horseState = value;
            GameManager.instance.HorseStateInit(value);
            switch (value)
            {
                case HorseState.Run:
                    break;
                case HorseState.Sleep:
                    follow.follow = false;
                    ani.SetInteger("State", 0);
                    break;
                case HorseState.Death:
                    break;
                default:
                    break;
            }
        }
    }
    Touch touch;
    public float speed = 5f;
    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
        ani = GetComponent<Animator>();
        follow = GetComponent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
        follow.followSpeed = speed;
        horseState = HorseState.Idle;
        //StartCoroutine(CoSpeedDown());
    }
    void Update()
    {
        transform.rotation = follow.transform.rotation;
        //follow.followSpeed = speed + (int)(speedAni / 0.5f);

        if (speedAni > 0f && !isSpeedDown && isSpeedZero)
        {
            isSpeedDown = true;
        }
        if(isSpeedDown && isSpeedZero)
        {
            StartCoroutine(CoSpeedDown());
            isSpeedDown = false;
            isSpeedZero = false;
        }
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                follow.followSpeed = speed + (int)(speedAni / 0.5f);
                animal.SetFloatParameter(animal.hash_Vertical, 2f + speedAni);
                animal.SetFloatParameter(animal.hash_Horizontal, 0f);
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
                break;
            case GameManager.GameState.Boss:
                break;
            default:
                break;
        }

        if (CheckItem(ItemList.SpeedUp))
        {
            SpeedUpUpdate();
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
                speedAni += 0.5f;
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
            follow.followSpeed = speed;
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
                horseState = HorseState.Run;
                animal.AlwaysForward = true;
                follow.follow = true;
                break;
            case GameManager.GameState.Trace:
                var bossHorse = GameObject.FindGameObjectWithTag("BossHorse");
                var bossSpeed = bossHorse.GetComponent<BossHorse>().speed;
                follow.followSpeed = bossSpeed;
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
            default:
                break;
        }
    }

    public void disMount()
    {
        StartCoroutine(CoDisMount());
    }
    IEnumerator CoSpeedDown()
    {
        while (speedAni>0f)
        {
            speedAni -= 0.05f;
            yield return new WaitForSeconds(0.5f);
        }
        isSpeedZero = true;
    }
    IEnumerator CoDisMount()
    {
        var targetPos = transform.position - transform.forward * 2f - transform.right * 2f;
        yield return StartCoroutine(CoMoveToTarget(0.6f,targetPos));
        yield return new WaitForSeconds(0.6f);  
        ani.SetInteger("ModeStatus", 2);
        var cowboy = GameObject.FindGameObjectWithTag("Player");
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        if (horseState == HorseState.Sleep)
        {
            cowboyScript.SetAnimation(Cowboy.PlayerAnimation.Sad);
        }
    }
    IEnumerator CoMoveToTarget(float duration, Vector3 targetPos)
    {
        var totalTime = 0f;
        while (totalTime <= duration)
        {
            Debug.Log(targetPos);
            totalTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, totalTime / duration);
            yield return null;
        }

        ani.SetInteger("ModeStatus", 0);
        ani.SetInteger("Mode", 4006);
        ani.SetInteger("State", 5);
        
        var cowboy = GameObject.FindGameObjectWithTag("Player");
        var boss = GameObject.FindGameObjectWithTag("Boss");
        cowboy.transform.LookAt(boss.transform.position);
    }
}
