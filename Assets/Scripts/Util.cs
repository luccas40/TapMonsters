using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Util {

    string[] cast = new string[] { "", "K", "M",  "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh",
            "ii", "jj", "kk", "ll", "mm", "nn", "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "xx", "ww",
            "yy", "zz", "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK", "LLL",
            "MMM", "NNN", "OOO", "PPP", "QQQ", "RRR", "SSS", "TTT", "UUU", "VV", "XXX", "WWW", "YYY", "ZZZ",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE"
        };

    private static Util instance;

    private Util() { }

    public static Util getInstance()
    {
        if (instance == null) { instance = new Util(); }
        return instance;
    }

    public string encrypt(string txt)
    {
        string newString = "";
        foreach (char c in txt)
        {
            newString += (int)c * 7;
        }

        return newString;
    }

    public string decrypt(string txt)
    {
        char[] c = txt.ToCharArray();
        string newString = "";
        for (int i = 0; i < c.Length; i = i + 3)
        {
            int d;
            Int32.TryParse(c[i] + "" + c[i + 1] + "" + c[i + 2], out d);
            newString += (char)(d / 7);
        }

        return newString;
    }


    public String format(string valor, int index)
    {
        bool negativa = valor.Contains("-");
        valor = Regex.Replace(valor, "[A-Za-z -]", ""); //tira todas as letras pois ja tenho o index

        int dotPosition = valor.IndexOf(".");
        if (dotPosition <= 0) { dotPosition = valor.Length; }
        string integerDoValor = valor.Substring(0, dotPosition);

        double indexCalc = (double)integerDoValor.Length / 3;
        indexCalc = indexCalc + index;
        valor.Replace(".", "");

        if (integerDoValor.Length == 4)
        {
            if (negativa) { return "-" + valor.Substring(0, 1) + "." + valor.Substring(1, 2) + cast[(int)indexCalc]; }
            else { return valor.Substring(0, 1) + "." + valor.Substring(1, 2) + cast[(int)indexCalc]; }
        }
        else if (integerDoValor.Length > 4)
        {
            int l = (integerDoValor.Length % 3 == 0) ? 3 : integerDoValor.Length % 3;
            return valor.Substring(0, l) + "." + valor.Substring(l, 2) + cast[(int)indexCalc - 1];
        }
        else
        {
            if (negativa) { return "-" + valor + cast[index]; }
            else { return valor + cast[index]; }
        }
    }





}
