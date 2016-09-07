using UnityEngine;
using System.Collections;

public class HUDControl : MonoBehaviour {


    public GameObject HeroPane;
    public GameObject SoldierPane;



    public void HeroPaneOn()
    {
        if (!HeroPane.activeSelf)
        {
            HeroPane.SetActive(true);
        }else
        {
            HeroPane.SetActive(false);
        }
    }


    public void SoldierPaneOn()
    {
        if (!SoldierPane.activeSelf)
        {
            SoldierPane.SetActive(true);
        }
        else
        {
            SoldierPane.SetActive(false);
        }
    }


    public void EssencePaneOn()
    {
        if (!HeroPane.activeSelf)
        {
            HeroPane.SetActive(true);
        }
        else
        {
            HeroPane.SetActive(false);
        }
    }


    public void ScrollPaneOn()
    {
        if (!HeroPane.activeSelf)
        {
            HeroPane.SetActive(true);
        }
        else
        {
            HeroPane.SetActive(false);
        }
    }



}
