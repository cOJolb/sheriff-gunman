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
        //스킨을 입을 수 있느냐
        equipButton.interactable = false;
        switch (skinNumber)
        {
            case 0:
                equipButton.interactable = true;
                break;
            case 1:
                CanEquip(10);
                break;
            case 2:
                CanEquip(20);
                break;
            case 3:
                CanEquip(30);
                break;
            case 4:
                CanEquip(40);
                break;
            case 5:
                CanEquip(50);
                break;
            case 6:
                CanEquip(60);
                break;
            case 7:
                CanEquip(70);
                break;
            case 8:
                break;
            case 9:
                break;
            default:
                break;
        }
        shopCamera.transform.RotateAround(shopHorse.position, Vector3.up, Time.deltaTime * speed);
    }
    public void CanEquip(int value)
    {
        if (GameManager.instance.totalEnemyCatch >= value)
        {
            equipButton.interactable = true;
        }
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
