using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopCamera;
    public float speed = 3f;
    public Transform shopHorse;
    public GameObject shopHorseModel;
    public Material[] skin;
    public Button equipButton;

    int skinNumber;
    void FixedUpdate()
    {
        shopCamera.transform.RotateAround(shopHorse.position, Vector3.up, Time.deltaTime * speed);
    }

    public void OnEquip()
    {
        GameManager.instance.HorseSkin = skinNumber;
    }
    public void nextSkin()
    {
        skinNumber++;
        if(skinNumber >= skin.Length)
        {
            skinNumber = 0;
        }
        SetSkin(skinNumber);
    }
    public void prevSkin()
    {
        skinNumber--;
        if (skinNumber < 0)
        {
            skinNumber = skin.Length-1;
        }
        SetSkin(skinNumber);
    }
    public void SetSkin(int value)
    {
        //말 스킨 적용
        var meshrender = shopHorseModel.GetComponent<SkinnedMeshRenderer>().materials;
        meshrender[0] = skin[value];
        shopHorseModel.GetComponent<SkinnedMeshRenderer>().materials = meshrender;
    }
}
