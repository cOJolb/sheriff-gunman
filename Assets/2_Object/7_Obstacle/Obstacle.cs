using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : isCollision, ICollisionAble
{
    public SetPos setPos;
    public void nowCollision(GameObject go)
    {
        GameManager.instance.HumanCollision();
        var horse = GameObject.FindGameObjectWithTag("Horse");
        var horseScript = horse.GetComponent<Horse>();
        horseScript.SpeedUpValue -= 0.2f;
        gameObject.layer = LayerMask.NameToLayer("Dummy");
        
        //Destroy(gameObject);
    }
    void Start()
    {
        PosSetting(setPos);
    }


    void Update()
    {
        
    }
}
