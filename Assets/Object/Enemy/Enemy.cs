using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, isCollision
{
    public void nowCollision()
    {
        GameManager.instance.EnemyCollision(gameObject);
        //Destroy(gameObject);
    }
    void Start()
    {
    }

    void Update()
    {
    }
}
