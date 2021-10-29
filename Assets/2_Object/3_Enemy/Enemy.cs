using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : isCollision, ICollisionAble
{
    Dreamteck.Splines.SplineFollower follow;
    public SetPos setPos;
    public float timeSpeed;
    public Image portrait;
    public GameObject ragDoll;
    public Image progress;

    void Start()
    {
        follow = GetComponentInParent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
        follow.followSpeed = timeSpeed;
        PosSetting(setPos);
    }

    void Update()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                follow.follow = true;
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.Boss:
                follow.follow = false;
                break;
            case GameManager.GameState.finish:
                follow.follow = false;
                break;
            case GameManager.GameState.GameOver:
                follow.follow = false;
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
    public void nowCollision(GameObject go)
    {
        var posIndex = GameManager.instance.EnemyCollision(gameObject);

        var newGo = Instantiate(ragDoll);
        switch (posIndex)
        {
            case 0:
                newGo.transform.position = go.transform.position;
                break;
            default:
                newGo.transform.position = go.transform.position;
                break;
        }
        var joint = newGo.GetComponentInChildren<SpringJoint>();
        joint.connectedBody = go.GetComponent<Rigidbody>();
        Destroy(gameObject);
    }
}
