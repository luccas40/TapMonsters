using UnityEngine;
using System.Collections;

public class HUDControl : MonoBehaviour {


    public GameObject[] Paineis;


    public void PaneOn(int id)
    {
        id--;
        if (Paineis[id].activeSelf)
        {
            PaneOff();
        }else
        {
            PaneOff();
            Paineis[id].SetActive(true);
        }
    }


    void PaneOff()
    {
        foreach(GameObject o in Paineis)
        {
            o.SetActive(false);
        }
    }





}
