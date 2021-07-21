using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject shopButton;
    public GameObject destoryButton;
    public GameObject installButton;
    public GameObject hpBer;

    public bool istimeCheck = true;

    public void Breaktime()
    {
        attackButton.SetActive(false);
        shopButton.SetActive(true);
        destoryButton.SetActive(true);
        installButton.SetActive(true);
        hpBer.SetActive(false);
    }
    public void GamePlayTime()
    {
        installButton.SetActive(false);
        destoryButton.SetActive(false);
        shopButton.SetActive(false);
        attackButton.SetActive(true);
        hpBer.SetActive(true);
    }
    public void Update()
    {
        if (istimeCheck == true)
        {
            Breaktime();
        }
        else
        {
            GamePlayTime();
        }
    }

}
