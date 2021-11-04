using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endSpace : MonoBehaviour, ICollisionAble
{
    private void Start()
    {
        var positioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
        positioner.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        positioner.SetPercent(1f);
    }
    public void nowCollision(GameObject go)
    {
        var enemylayer = LayerMask.NameToLayer("Enemy");
        if (go.layer == enemylayer)
        {
            //GameManager.instance.BossRun();
            Destroy(go);
        }
        else
        {
            GameManager.instance.endCollision();
            var horse = GameObject.FindGameObjectWithTag("Horse");
            horse.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
