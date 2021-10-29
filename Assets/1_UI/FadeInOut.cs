using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour
{
    public void InOut()
    {
        var image = GetComponent<Image>();
        StartCoroutine(CoFadeInOut(image.color, Color.black, 2f));
    }
    
    public IEnumerator CoFadeIn(Image img, Color startColor, Color endColor, float duration)
    {
        var totalTime = 0f;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            img.color = Color.Lerp(startColor, endColor, totalTime / duration);
            yield return null;
        }
    }
    public IEnumerator CoFadeInOut(Color startColor, Color endColor, float duration)
    {
        var image = GetComponent<Image>();
        yield return StartCoroutine(CoFadeIn(image, startColor, endColor, duration / 2f));
        StartCoroutine(CoFadeIn(image, endColor, startColor, duration / 2f));
    }
}
