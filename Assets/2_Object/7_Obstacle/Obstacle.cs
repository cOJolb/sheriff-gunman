using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : isCollision, ICollisionAble
{
    public SetPos setPos;
    public float distance;
    public float downSpeed = 1f;
    Dreamteck.Splines.SplinePositioner positioner;
    public void nowCollision(GameObject go)
    {
        GameManager.instance.HumanCollision();
        var horse = GameObject.FindGameObjectWithTag("Horse");
        var horseScript = horse.GetComponent<Horse>();
        horseScript.SpeedUpValue -= downSpeed;
        gameObject.layer = LayerMask.NameToLayer("Dummy");
        positioner.enabled = false;
        //Destroy(gameObject);
    }
    void Start()
    {
        positioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
        positioner.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        positioner.SetDistance(distance);
        PosSetting(setPos);
    }
    void Update()
    {
        
    }
}
