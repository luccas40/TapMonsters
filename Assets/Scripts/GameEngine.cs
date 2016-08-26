using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameEngine : MonoBehaviour {


    public GameObject[] Enemies;
    private int fase = 1;
    private int level = 0;

    GameObject[] FaseTxt = new GameObject[2];
    


    void Start () {

        FaseTxt[0] = GameObject.FindGameObjectWithTag("HUD#Fase");
        FaseTxt[1] = GameObject.FindGameObjectWithTag("HUD#LevelFase");
        enemySpawn();

        
    }
	
	void Update () {
	    
	}


    public void enemySpawn()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            level++;
            if (level > 10) { level = 1; fase++; }

            FaseTxt[0].GetComponent<Text>().text = "Fase " + fase;
            FaseTxt[1].GetComponent<Text>().text = level + "/10";

            int rand = Random.Range(0, Enemies.Length - 1);
            GameObject enemy = Enemies[rand];
            enemy = (GameObject)Instantiate(enemy);

            decimal calculoHP = (decimal)(fase * 29 + System.Math.Pow(1.6, (fase + 1)));
            if (level == 10) { calculoHP *= 2.5m; }
            calculoHP = System.Math.Ceiling(calculoHP);

            decimal calculoGold = (decimal)(System.Math.Pow(2, fase) / fase);
            calculoGold = System.Math.Ceiling(calculoGold);

            enemy.GetComponent<Enemy>().setVida(calculoHP);
            enemy.GetComponent<Enemy>().setGold(calculoGold);
        }
        else
        {
            StartCoroutine(CDEnemy());
        }
    }


    IEnumerator CDEnemy()
    {
        yield return new WaitForSeconds(.15f);
        enemySpawn();
    }



}
