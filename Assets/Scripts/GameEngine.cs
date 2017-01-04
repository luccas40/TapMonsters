using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class GameEngine : MonoBehaviour {
	
	private Player player;
	public GameObject[] Enemies;
    private int fase=1;
    private int level=1;
    bool start;
    bool loaded = false;
    GameObject[] FaseTxt = new GameObject[2];



    void OnApplicationQuit()
    {
        if(loaded)
			Util.getInstance().saveGame(this);
    }


    void OnApplicationPause()
    {
        if (loaded)
			Util.getInstance().saveGame(this);
    }



    void Start () {
        start = true;
        FaseTxt[0] = GameObject.FindGameObjectWithTag("HUD#Fase");
        FaseTxt[1] = GameObject.FindGameObjectWithTag("HUD#LevelFase");
		Util.getInstance ().loadGame (out fase, out level, out player);
	    loaded = true;
	    enemySpawn();
    }


	public Player getPlayer(){
		return player;
	}

	public int getFaseNum(){
		return fase;
	}

	public int getLevelNum(){
		return level;
	}

    public void cantKillBoss()
    {
        fase--;
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
            calculoHP *= Math.Pow(1.23f, (fase + 1));

            calculoHP *= Math.Pow(10, (fase / 50));
            if (fase >= 250 && fase < 1000) { calculoHP *= Math.Pow(fase, fase / 100); }
            if (fase >= 1000 && fase <= 2000) { calculoHP *= Math.Pow(fase, fase / 1000); }
            if (fase < 100) { calculoHP /= 2; }
            if (level == 10) { calculoHP *= 2.5; }

            double calculoGold = Math.Pow(1.9, fase);
            calculoGold /= (fase * 1.5f);
            calculoGold /= 2;


            calculoGold = Math.Ceiling(calculoGold);
            calculoHP = Math.Ceiling(calculoHP);

            enemy.GetComponent<Enemy>().setVida(calculoHP);
            enemy.GetComponent<Enemy>().setGold(calculoGold);

            if (level == 10) { enemy.GetComponent<Enemy>().setBoss(30f); }

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

}
