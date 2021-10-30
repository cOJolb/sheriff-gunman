using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
        ani = GetComponent<Animator>();
        follow = GetComponent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
        follow.followSpeed = speed;

    }
    void Update()
    {
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
                //animal.AlwaysForward = true;
                //follow.follow = true;
                break;
            case GameManager.GameState.RunOver:
                animal.AlwaysForward = false;
                follow.follow = false;
                animal.State_Activate(10);
                break;
            case GameManager.GameState.Boss:
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                //follow.follow = false;
                //ani.SetInteger("State", 0);
                break;
            default:
                break;
        }
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}
