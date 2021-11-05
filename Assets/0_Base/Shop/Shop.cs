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
    public Text totalCatchEnemy;
    public GameObject equipped;
    public GameObject needImage;
    public Text needEnemy;
    int skinNumber;
    void FixedUpdate()
    {
        totalCatchEnemy.text = $" : {GameManager.instance.totalEnemyCatch}";
        //스킨을 입을 수 있느냐
        
        needImage.SetActive(false);
        switch (skinNumber)
        {
            case 0:
                SettingHorseSkin(0);
                break;
            case 1:
                SettingHorseSkin(15);
                break;
            case 2:
                SettingHorseSkin(23);
                break;
            case 3:
                SettingHorseSkin(30);
                break;
            case 4:
                SettingHorseSkin(38);
                break;
            case 5:
                SettingHorseSkin(45);
                break;
            case 6:
                SettingHorseSkin(53);
                break;
            case 7:
                SettingHorseSkin(60);
                break;
            case 8:
                SettingHorseSkin(68);
                break;
            case 9:
                SettingHorseSkin(75);
                break;
            default:
                break;
        }
        shopCamera.transform.RotateAround(shopHorse.position, Vector3.up, Time.deltaTime * speed);

    }
    public void SettingHorseSkin(int value)
    {
        CanEquip(value);
        if (GameManager.instance.totalEnemyCatch < value)
        {
            needImage.SetActive(true);
            needEnemy.text = $" : {value}";
        }
    }
    public void CanEquip(int value)
    {
        if (GameManager.instance.HorseSkin == skinNumber)
        {
            equipButton.interactable = false;
            equipped.SetActive(true);
        }
        else
        {
            if (GameManager.instance.totalEnemyCatch >= value)
            {
                equipButton.interactable = true;
            }
            else
            {
                equipButton.interactable = false;
            }
            equipped.SetActive(false);
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
    public void OnAdButton()
    {
        GoogleMobileAdTest.OnClickGoods();

    }
}
