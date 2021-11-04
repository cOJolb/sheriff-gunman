using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : isCollision, ICollisionAble
{
    public SetPos setPos;
    public float distance;
    public float downSpeed = 1f;

    public void nowCollision(GameObject go)
    {
        GameManager.instance.HumanCollision();
        var horse = GameObject.FindGameObjectWithTag("Horse");
        var horseScript = horse.GetComponent<Horse>();
        horseScript.SpeedUpValue -= downSpeed;
        gameObject.layer = LayerMask.NameToLayer("Dummy");
        //Destroy(gameObject);
    }
    void Start()
    {
        var positioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
        positioner.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        positioner.SetDistance(distance);
        PosSetting(setPos);
    }
    void Update()
    {
        
    }
}
