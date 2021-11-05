using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using MalbersAnimations.Events;
using MalbersAnimations.Scriptables;


public class BossHorse : MonoBehaviour
{ 
    Animator ani;
    public MalbersAnimations.Controller.MAnimal animal;
    float totalTime = 0f;
    bool startDash;
    public float speed = 5f;
    public Image progress;
    public GameObject damageParticle;
    public GameObject failParticle;
    public float distance;
    private double prevPercentage;
    public GameObject[] doors;

    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
        ani = GetComponent<Animator>();
        follow = GetComponentInParent<Dreamteck.Splines.SplineFollower>();
        //follow.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        follow.follow = false;
        follow.followSpeed = speed;
        follow.SetDistance(distance);

    }
    void Update()
    {
        //Debug.Log(follow.GetPercent());
        transform.rotation = follow.transform.rotation;
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                if(startDash)
                {
                    totalTime += Time.deltaTime;
                    if (totalTime >= 0.7f)
                    {
                        animal.AlwaysForward = true;
                        follow.follow = true;
                        animal.SetFloatParameter(animal.hash_Vertical, 5f);
                        animal.SetFloatParameter(animal.hash_Horizontal, 0f);
                    }
                }
                break;
            case GameManager.GameState.Play:
                follow.followSpeed = speed;
                animal.SetFloatParameter(animal.hash_Vertical, 5f);
                animal.SetFloatParameter(animal.hash_Horizontal, 0f);
                break;
            case GameManager.GameState.Boss:
                break;
            default:
                break;
        }
    }
    public void StartDash()
    {
        startDash = true;
    }
    public void StateInit(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Start:
                
                break;
            case GameManager.GameState.Play:
                animal.AlwaysForward = true;
                follow.follow = true;
                break;
            case GameManager.GameState.Trace:
                //animal.AlwaysForward = false;
                //follow.follow = false;
                break;
            case GameManager.GameState.RunOver:
                animal.AlwaysForward = false;
                follow.follow = false;
                animal.State_Activate(10);
                break;
            case GameManager.GameState.Boss:
                //ani.SetInteger("State", 0);
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.BossRun:
                animal.AlwaysForward = true;
                animal.SetFloatParameter(animal.hash_Vertical, 5f);
                break;
            case GameManager.GameState.GameOver:
                switch (GameManager.instance.PrevState)
                {
                    case GameManager.GameState.Play:
                        prevPercentage = follow.GetPercent();
                        break;
                    case GameManager.GameState.Trace:
                        prevPercentage = follow.GetPercent();
                        FinishDirecting();
                        break;
                    case GameManager.GameState.Boss:
                        animal.AlwaysForward = false;
                        follow.follow = false;
                        break;
                    default:
                        break;
                }

                //ani.SetInteger("State", 0);
                break;
            case GameManager.GameState.ReStart:
                //재시작 초기화
                switch (GameManager.instance.PrevState)
                {
                    case GameManager.GameState.Play:
                        follow.SetPercent(prevPercentage);
                        break;
                    case GameManager.GameState.Trace:
                        follow.SetPercent(prevPercentage);
                        break;
                    
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "end" && GameManager.instance.state == GameManager.GameState.Play)
        {
            GameManager.instance.state = GameManager.GameState.BossRun;

        }
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}
