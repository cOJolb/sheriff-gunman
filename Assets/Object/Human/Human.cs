using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, isCollision
{
    public void nowCollision()
    {
        GameManager.instance.HumanCollision();
        //Destroy(gameObject);
    }
    void Start()
    {
    }

    void Update()
    {
        
    }
}
