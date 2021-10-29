using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endSpace : MonoBehaviour, ICollisionAble
{
    public void nowCollision(GameObject go)
    {
        var enemylayer = LayerMask.NameToLayer("Enemy");
        if (go.layer == enemylayer)
        {
            GameManager.instance.BossRun();
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
