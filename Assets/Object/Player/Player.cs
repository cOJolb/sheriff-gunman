using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    float speedUpTime;
    float totalspeedUpTime;

    private ItemList playerGetItem;
    public ItemList playerItem
    {
        get
        {
            return playerGetItem;
        }
    }

    public enum PlayerState
    {
        normal,
        speedUp,
        fallDown,
        Death
    }
    private PlayerState playerState;
    public PlayerState state
    {
        get
        {
            return playerState;
        }
        set
        {
            playerState = value;
        }
    }

    Touch touch;
    public float speed = 5f;
    Dreamteck.Splines.SplineFollower follow;
    private void Start()
    {
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
                break;
            case GameManager.GameState.Play:
                follow.follow = true;
                
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
#endif
                }
                break;
            case GameManager.GameState.Boss:
                follow.follow = false;
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
                follow.followSpeed += itemValue;
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
        if(totalspeedUpTime >= speedUpTime)
        {
            totalspeedUpTime = 0f;
            follow.followSpeed = speed;
            playerGetItem &= (~ItemList.SpeedUp);
        }
    }
    public bool CheckItem(ItemList type)
    {
        return ((playerItem & type) != 0);
    }
}
