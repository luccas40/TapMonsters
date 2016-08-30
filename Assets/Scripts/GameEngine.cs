using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

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
            fase = 1;
            level = 1;
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

            int rand = Random.Range(0, Enemies.Length - 1);
            GameObject enemy = Enemies[rand];
            enemy = (GameObject)Instantiate(enemy);

            decimal calculoHP = (decimal)(fase * 29 + System.Math.Pow(1.6, (fase + 1)));
            if (level == 10) { calculoHP *= 2.5m; }
            calculoHP = System.Math.Ceiling(calculoHP);

            decimal calculoGold = (decimal)(System.Math.Pow(2, fase) / (fase*1.5f))/2;
            calculoGold = System.Math.Ceiling(calculoGold);

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
        string data = "{"+fase+","+level+","+p.getGold()+","+p.getLevel()+"}{"+ NumberFormat.getInstance().encrypt(passwordSave) +"}";
        data += "{"+(data.GetHashCode()*data.Length)+"}";
        data = NumberFormat.getInstance().encrypt(data);

        if (!Directory.Exists(Application.persistentDataPath + "/data")) { Directory.CreateDirectory(Application.persistentDataPath + "/data"); }
        
        FileStream fs = File.Open(Application.persistentDataPath + "/data/01110011011000010111011001100101.ide", FileMode.OpenOrCreate);
        byte[] dataPersist = new UTF8Encoding(true).GetBytes(data);
        fs.Write(dataPersist, 0, dataPersist.Length);
        fs.Flush();
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

                data = NumberFormat.getInstance().decrypt(data);
                string[] firstPart = data.Split('{');
                string[] secondPart = firstPart[2].Split('}');
                string[] thirdPart = firstPart[3].Split('}');
                firstPart = firstPart[1].Split('}');


                string pass = NumberFormat.getInstance().encrypt(passwordSave);


                if (pass.Equals(secondPart[0]))
                {
                    string tmp = "{" + firstPart[0] + "}{" + pass + "}";
                    tmp = "" + (tmp.GetHashCode() * tmp.Length);
                    if (tmp.Equals(thirdPart[0]))
                    {
                        firstPart = firstPart[0].Split(',');
                        fase = System.Int32.Parse(firstPart[0]);
                        level = System.Int32.Parse(firstPart[1]);
                        p.setLevel(1600/*System.Int32.Parse(firstPart[3])*/);
                        p.earnGold(decimal.Parse(firstPart[2]));
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
        catch (System.IndexOutOfRangeException e) { return false;  }
    }



}
