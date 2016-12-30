using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Cryptography;

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

	public string encrypt(string toEncrypt, bool useHashing = true)
    {
        
		byte[] keyArray;
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (toEncrypt);
		string key = "#jogoDoPanda";

		if (useHashing)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
			hashmd5.Clear();
		}
		else
			keyArray = UTF8Encoding.UTF8.GetBytes(key);

		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;

		ICryptoTransform cTransform = tdes.CreateEncryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		tdes.Clear();

		return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

	public string decrypt(string toDecrypt, bool useHashing = true)
    {
		byte[] keyArray;
		byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
		string key = "#jogoDoPanda";

		if (useHashing)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

			hashmd5.Clear();
		}
		else
		{
			keyArray = UTF8Encoding.UTF8.GetBytes(key);
		}

		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;

		ICryptoTransform cTransform = tdes.CreateDecryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);       
		tdes.Clear();

		return UTF8Encoding.UTF8.GetString(resultArray);
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
