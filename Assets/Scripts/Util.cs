using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

public class Util {

    string[] cast = new string[] { "", "K", "M",  "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh",
            "ii", "jj", "kk", "ll", "mm", "nn", "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "xx", "ww",
            "yy", "zz", "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK", "LLL",
            "MMM", "NNN", "OOO", "PPP", "QQQ", "RRR", "SSS", "TTT", "UUU", "VV", "XXX", "WWW", "YYY", "ZZZ",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE"
        };

	private string passwordSave = "#suamaequeroverquemvaidecryptografarisso#";

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



	public void saveGame(GameEngine ge)
	{
		Player p = ge.getPlayer ();

		string playerInfo = "{" + ge.getFaseNum() + "," + ge.getLevelNum() + "," + p.getGold() + "," + p.getLevel() + "}";
		string soldiersInfo = "{";
		foreach(Soldier s in p.getSoldiers())
		{
			if (s != null)
			{
				soldiersInfo += s.id + ";" + s.getLevel() + ",";
			}
		}
		soldiersInfo.Substring(0, soldiersInfo.Length - 1);
		soldiersInfo += "}";


		string passwordGame = "{" + Util.getInstance().encrypt(passwordSave) + "}";
		string data = playerInfo + soldiersInfo + passwordGame;
		string hashCode = "{" + Math.Abs(data.GetHashCode()*data.Length).ToString() + "}";

		data += hashCode;
		data = Util.getInstance().encrypt(data);

		if (!Directory.Exists(Application.persistentDataPath + "/data")) { Directory.CreateDirectory(Application.persistentDataPath + "/data"); }

		FileStream fs = File.Open(Application.persistentDataPath + "/data/01110011011000010111011001100101.ide", FileMode.OpenOrCreate);
		byte[] dataPersist = new UTF8Encoding(true).GetBytes(data);
		fs.Write(dataPersist, 0, dataPersist.Length);
		fs.Close();
	}

	public void loadGame(out int fase, out int level, out Player p)
	{
		fase = 1;
		level = 1;
		p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		p.setLevel (1);

		try
		{
			if (File.Exists(Application.persistentDataPath + "/data/01110011011000010111011001100101.ide"))
			{
				FileStream fs = File.Open(Application.persistentDataPath + "/data/01110011011000010111011001100101.ide", FileMode.Open);
				StreamReader sr = new StreamReader(fs);
				string data = sr.ReadLine();
				sr.Close();
				fs.Close();

				data = Util.getInstance().decrypt(data);
				string[] brute = data.Split('{');
				string[] soldiersInfo = brute[2].Split('}');
				string[] passwordGame = brute[3].Split('}');
				string[] hashCode = brute[4].Split('}');
				string[] playerInfo = brute[1].Split('}');


				string pass = Util.getInstance().encrypt(passwordSave);


				if (pass.Equals(passwordGame[0]))
				{
					string tmp = "{" + playerInfo[0] + "}{"+soldiersInfo[0]+"}{" + pass + "}";
					tmp = Math.Abs(tmp.GetHashCode() * tmp.Length).ToString();
					if (tmp.Equals(hashCode[0]))
					{
						playerInfo = playerInfo[0].Split(',');
						fase = System.Int32.Parse(playerInfo[0]);//2700limite
						level = System.Int32.Parse(playerInfo[1]);
						p.setLevel(System.Int32.Parse(playerInfo[3]));
						p.earnGold(double.Parse(playerInfo[2]));

						soldiersInfo = soldiersInfo[0].Split(',');
						foreach(string s in soldiersInfo)
						{
							if (!"".Equals(s))
							{
								string[] temporario = s.Split(';');
								p.setSoldier(int.Parse(temporario[0]), int.Parse(temporario[1]));
							}
						}
					}
					else
					{
						//new game hack noob
					}
				}
				else
				{
					//new game hack noob
				}
			}
		}
		catch (System.IndexOutOfRangeException e) { Debug.Log(e.Message); }
	}




}
