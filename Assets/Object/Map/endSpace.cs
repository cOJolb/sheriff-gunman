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
            GameManager.instance.EnemyRun(go);
            Destroy(go);
        }
        else
        {
            GameManager.instance.endCollision();
            Destroy(gameObject);
        }
    }
    private void Start()
    {
    }
}
