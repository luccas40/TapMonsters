using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class NumberFormat {

    private static NumberFormat instance;

    private NumberFormat(){}

    public static NumberFormat getInstance()
    {
        if (instance == null) { instance = new NumberFormat(); }
        return instance;
    }

    public string encrypt(string txt)
    {
        string newString = "";
        foreach(char c in txt)
        {
            newString += (int)c*7;
        }

        return newString;
    }

    public string decrypt(string txt)
    {
       char[] c = txt.ToCharArray();
        string newString = "";
        for (int i = 0; i < c.Length; i=i+3){
            int d;
            Int32.TryParse(c[i] + "" + c[i + 1] + "" + c[i + 2], out d);
            newString += (char)(d / 7);
        }

        return newString;
    }


}
