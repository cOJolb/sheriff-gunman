using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endSpace : MonoBehaviour, isCollision
{
    public void nowCollision()
    {
        GameManager.instance.endCollision();
        Destroy(gameObject);
    }
    private void Start()
    {
    }
}
