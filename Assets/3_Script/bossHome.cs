using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHome : MonoBehaviour, ICollisionAble
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float closeTime = 3f;

    bool isClosed;
    float totalTime;

    private void Start()
    {
        var positioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
        positioner.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        positioner.SetPercent(1f);
    }
    private void Update()
    {
        if(isClosed)
        {
            totalTime = Time.deltaTime;
            var lerp = totalTime / closeTime;
            leftDoor.transform.rotation = Quaternion.Lerp(leftDoor.transform.rotation, Quaternion.identity, lerp);
            rightDoor.transform.rotation = Quaternion.Lerp(rightDoor.transform.rotation, Quaternion.identity, lerp);
        }
    }
    public void nowCollision(GameObject go)
    {
        isClosed = true;
        
        GameManager.instance.GameOver();
        GameManager.instance.FinishDirecting();
    }
}
