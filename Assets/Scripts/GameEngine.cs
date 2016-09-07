using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class GameEngine : MonoBehaviour {


    private string passwordSave = "#suamaequeroverquemvaidecryptografarisso#";

    public GameObject[] Enemies;
    private int fase=1;
    private int level=1;
    bool start;
    bool loaded = false;
    GameObject[] FaseTxt = new GameObject[2];



    void OnApplicationQuit()
    {
        if(loaded)
        save();
    }


    void OnApplicationPause()
    {
        if (loaded)
            save();
    }



    void Start () {
        
         start = true;
         FaseTxt[0] = GameObject.FindGameObjectWithTag("HUD#Fase");
         FaseTxt[1] = GameObject.FindGameObjectWithTag("HUD#LevelFase");
        if (!load())
        {
            setUpNewGame();
        }
         loaded = true;
         enemySpawn();
         
       

    }



    public void enemySpawn()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            if (!start) { level++; }
            if (level > 10) { level = 1; fase++; }

            updateHUD();

            int rand = UnityEngine.Random.Range(0, Enemies.Length - 1);
            GameObject enemy = Enemies[rand];
            enemy = (GameObject)Instantiate(enemy);

            double calculoHP = fase * 29;
            calculoHP *= Math.Pow(1.23f ,(fase + 1)) ;

            calculoHP *= Math.Pow(10, (fase/50));
            if (fase >= 250 && fase < 1000) { calculoHP *= Math.Pow(fase, fase / 100); }
            if (fase >= 1000 && fase <= 2000) { calculoHP *= Math.Pow(fase, fase / 1000); }
            if (fase < 100) { calculoHP /= 2; }
            if (level == 10) { calculoHP *= 2.5; }

            double calculoGold = Math.Pow(1.9, fase);
            calculoGold /= (fase * 1.5f);
            calculoGold /= 2 ;


            calculoGold = Math.Ceiling(calculoGold);
            calculoHP = Math.Ceiling(calculoHP);



            enemy.GetComponent<Enemy>().setVida(calculoHP);
            enemy.GetComponent<Enemy>().setGold(calculoGold);
        }
        else
        {
            StartCoroutine(CDEnemy());
        }
        start = false;
    }


    IEnumerator CDEnemy()
    {
        yield return new WaitForSeconds(.15f);
        enemySpawn();
    }

    void updateHUD()
    {
        FaseTxt[0].GetComponent<Text>().text = "Fase " + fase;
        FaseTxt[1].GetComponent<Text>().text = level + "/10";
    }


    public void save()
    {
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        string playerInfo = "{" + fase + "," + level + "," + p.getGold() + "," + p.getLevel() + "}";
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

    public bool load()
    {
        try
        {
            Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
                        

                        updateHUD();
                        return true;

                    }
                    else
                    {
                        //new game hack noob
                        return false;
                    }
                }
                else
                {
                    //new game hack noob
                    return false;
                }


            }

            return false;
        }
        catch (System.IndexOutOfRangeException e) { Debug.Log(e.Message); return false;  }
    }



    private void setUpNewGame()
    {
        fase = 1;
        level = 1;
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        p.setLevel(1);
    }

}
