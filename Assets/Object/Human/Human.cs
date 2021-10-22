using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, ICollisionAble
{
    public void nowCollision(GameObject go)
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
