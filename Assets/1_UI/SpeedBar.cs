using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public void SettingValue(float value)
    {
        GetComponent<Slider>().value = value;
    }
}
