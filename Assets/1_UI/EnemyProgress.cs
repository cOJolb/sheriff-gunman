using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyProgress : MonoBehaviour
{
    public void SettingValue(float enemyPos, float width)
    {
        //GetComponent<Image>().rectTransform.sizeDelta = new Vector2(enemyPos * width, 0f);
        GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(enemyPos * width, 0f);
    }
}
