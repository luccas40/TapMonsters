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


    public String format(decimal value)
    {
        String temp = value.ToString("");

        double i = (double)temp.Length / 3;
        i = Math.Ceiling(i);
        String[] cast = new String[22];
        cast[0] = "";
        cast[1] = "K";
        cast[2] = "M";
        cast[3] = "B";
        cast[4] = "T";
        cast[5] = "aa";
        cast[6] = "bb";
        cast[7] = "cc";
        cast[8] = "dd";
        cast[9] = "ee";
        cast[10] = "ff";
        cast[11] = "gg";
        cast[12] = "hh";
        cast[13] = "ii";
        cast[14] = "jj";
        cast[15] = "kk";
        cast[16] = "ll";
        cast[17] = "mm";
        cast[18] = "nn";
        cast[19] = "oo";
        cast[20] = "pp";
        cast[21] = "qq";


        if (temp.Length % 3 == 0 && temp.Length > 3) // 000,00
        {
            return temp.Substring(0, 3) + "." + temp.Substring(3, 2) + cast[(int)i - 1];
        }
        else if ((temp.Length - 1) % 3 == 0 && temp.Length > 3)// 0,00
        {
            return temp.Substring(0, 1) + "." + temp.Substring(1, 2) + cast[(int)i - 1];
        }
        else if ((temp.Length - 2) % 3 == 0 && temp.Length > 3)// 00,00
        {
            return temp.Substring(0, 2) + "." + temp.Substring(2, 2) + cast[(int)i - 1];
        }
        else {
            return temp + cast[(int)i - 1];
        }




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
            Debug.Log(newString);
        }

        return newString;
    }


}
