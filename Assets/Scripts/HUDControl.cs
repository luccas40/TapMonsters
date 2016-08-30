using UnityEngine;
using System.Collections;

public class HUDControl : MonoBehaviour {


    public GameObject HeroPane;


    public void HeroPaneOn()
    {
        if (!HeroPane.activeSelf)
        {
            HeroPane.SetActive(true);
        }else
        {
            HeroPaneOff();
        }
    }

    public void HeroPaneOff()
    {
        HeroPane.SetActive(false);
    }


}
