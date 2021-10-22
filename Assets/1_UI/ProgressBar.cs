using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public void SettingValue(float value)
    {
        GetComponent<Slider>().value = value;
    }
}
