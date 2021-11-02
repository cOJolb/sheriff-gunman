using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public void Setting(float delta, float width , float gauge)
    {
        var rectTF = gameObject.GetComponent<Image>().rectTransform;
        rectTF.anchoredPosition = new Vector2(width * delta, 0);
        rectTF.sizeDelta = new Vector2((gauge * 2f) * width, 0);
        //rectTF.offsetMin = new Vector2(rectTF.offsetMin.x, 2.7f);
        //rectTF.offsetMax = new Vector2(rectTF.offsetMax.x, -2.7f);
    }
}
