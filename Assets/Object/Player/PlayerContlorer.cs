using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerContlorer : MonoBehaviour
{
    //private int touchId = int.MinValue;
    Touch touch;
    public float speed = 5f;
    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
        follow = GetComponentInParent<Dreamteck.Splines.SplineFollower>();
        follow.follow = false;
    }
    void Update()
    {
        transform.rotation = follow.transform.rotation;
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                follow.follow = true;
                //transform.position += Time.deltaTime * transform.forward * speed;
                if (Input.touchCount == 1)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        transform.position += transform.right * touch.deltaPosition.x * 0.01f;
                    }
                }
                break;
            case GameManager.GameState.Boss:
                follow.follow = false;
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        var collisionable = other.gameObject.GetComponentsInChildren<isCollision>();
        foreach (var collision in collisionable)
        {
            collision.nowCollision();
        }
    }
}
