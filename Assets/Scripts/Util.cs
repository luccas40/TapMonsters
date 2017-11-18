using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using PwndaGames.TapMonsters;
using System.Runtime.Serialization.Formatters.Binary;

public class Util {

    private static Util instance;

    private Util() { }

    public static Util getInstance()
    {
        if (instance == null) { instance = new Util(); }
        return instance;
    }
    
    public Pitolar getEnemyHP(int fase, int level)
    {
        Pitolar calculoHP = new Pitolar(fase * 18, 0);
        calculoHP *= Math.Pow(1.39f, Math.Min(fase, 115));
        calculoHP *= Math.Pow(1.13f, Math.Max(fase - 115, 0));
        calculoHP *= Math.Pow(1.39f, Math.Min(fase, 115));
        calculoHP *= Math.Pow(1.13f, Math.Max(fase - 115, 0));
        calculoHP /= (double)5000 / Math.Abs(5000 - fase * 10);
        calculoHP /= fase; 
        if (level == 10) { calculoHP *= 2.5; }

        return calculoHP;
    }

    public double getEnemyGold(int fase, int level)
    {
        double calculoGold = Math.Pow(1.9, fase);
        calculoGold /= (fase * 1.5f);
        calculoGold /= 2;
        return calculoGold;
    }

	public void saveGame(GameData gd)
	{
        try
        {
            if (!Directory.Exists(Application.persistentDataPath + "/save"))
                Directory.CreateDirectory(Application.persistentDataPath + "/save");

            using (Stream stream = File.Open(Application.persistentDataPath + "/save/01110011011000010111011001100101.ide", FileMode.OpenOrCreate))
            {
                MemoryStream streamMemory = new MemoryStream();
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(streamMemory, gd);
                byte[] serialEncoded = streamMemory.GetBuffer();
                stream.Write(serialEncoded, 0, serialEncoded.Length);
                streamMemory.Close();
            }
        }
        catch (IOException e) { Debug.Log("Erro ao salvar o game -=   " +e); }
	}

	public void loadGame(out GameData gd)
	{
        try
        {
            using (Stream stream = File.Open(Application.persistentDataPath + "/save/01110011011000010111011001100101.ide", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                gd = (GameData)bin.Deserialize(stream);
            }
        }
        catch (IOException) { Debug.Log("Erro ao carregar game"); gd = null; }

    }




}
