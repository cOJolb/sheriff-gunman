using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, isCollision
{
    public void nowCollision()
    {
        GameManager.instance.ItemCollision(1f);
        Destroy(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
    
}
