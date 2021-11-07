using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] kk;
    public Button prevButton;
    public Button nextButton;
    int nowindex;

    private void Update()
    {

        if (nowindex == 0)
        {
            prevButton.interactable = false;
        }
        else
        {
            prevButton.interactable = true;
        }

    }
    public void OnNext()
    {
        if(nowindex == kk.Length-1)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        else
        {
            kk[nowindex].SetActive(false);
            ++nowindex;
            kk[nowindex].SetActive(true);
        }
    }
    public void OnPrev()
    {
        if (nowindex > 0)
        {
            kk[nowindex].SetActive(false);
            --nowindex;
            kk[nowindex].SetActive(true);
        }
    }

}
