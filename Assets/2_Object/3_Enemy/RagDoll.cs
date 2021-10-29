using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    GameObject particle;
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>().gameObject;
    }
    void Update()
    {
        if(GameManager.instance.state != GameManager.GameState.Play)
        {
            particle.SetActive(false);
        }
    }
}
