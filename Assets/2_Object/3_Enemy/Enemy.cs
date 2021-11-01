using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : isCollision, ICollisionAble
{
    public Animator ani;

    Dreamteck.Splines.SplineFollower follow;
    public SetPos setPos;
    public float timeSpeed;
    public Image portrait;
    public GameObject ragDoll;
    public GameObject model;
    public Image progress;

    bool isCatch;

    void Start()
    {
        follow = GetComponentInParent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
        follow.followSpeed = timeSpeed;
        PosSetting(setPos);
    }

    void Update()
    {

    }
    public void StateInit(GameManager.GameState value)
    {
        if (!isCatch)
        {
            switch (value)
            {
                case GameManager.GameState.Start:
                    follow.follow = true;
                    break;
                case GameManager.GameState.Play:
                    follow.follow = true;
                    ani.SetInteger("State", 0);
                    break;
                case GameManager.GameState.Boss:
                    follow.follow = false;
                    break;
                case GameManager.GameState.finish:
                    follow.follow = false;
                    break;
                case GameManager.GameState.GameOver:
                    ani.SetInteger("State", 1);
                    follow.follow = false;
                    transform.LookAt(GameManager.instance.cowboy.transform.position);
                    break;
                default:
                    break;
            }
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
    public void nowCollision(GameObject go)
    {
        //¸ðµ¨Àº ²ô°í ·ºµ¹Àº ÄÑ¼­ ²ø·Á°¡´Â ¿¬Ãâ on
        GameManager.instance.EnemyCollision(gameObject);
        follow.follow = false;
        model.SetActive(false);
        ragDoll.SetActive(true);
        var joint = ragDoll.GetComponentInChildren<SpringJoint>();
        joint.connectedBody = go.GetComponent<Rigidbody>();
        isCatch = true; 
    }
}
