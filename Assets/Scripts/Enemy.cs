using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {

    public string nameMonster;
    public float gastoBase;
    double health;
    double maxHealth;
    double gold;

    float tempo;
    GameObject timer;

    public bool death = false;

    Animator anim;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        anim = this.gameObject.GetComponent<Animator>();
        timer = GameObject.Find("Canvas/HUD/MonsterHUD/Timer/Time");
    }

    public void setVida(double hp)
    {
        this.health = this.maxHealth = hp;
        updateHud();
    }

    public void setGold(double gold)
    {
        this.gold = gold;
    }

    public void hitMe(double damage)
    {
        if (!death)
        {
            health -= damage;
            if (health <= 0)
            {
                death = true;
                health = 0;
                updateHud();
                Death();
                return;
            }
            anim.SetTrigger("hit");
            updateHud();
        }
    }

    public void setBoss(float tempo)
    {
        this.tempo = tempo;
        StartCoroutine(bossTime());
    }

    IEnumerator bossTime()
    {

        while(tempo > 0)
        {
            yield return new WaitForSeconds(0.1f);
            tempo -= 0.1f;
            timer.GetComponent<Text>().text = tempo.ToString("0 0 . 0");
        }
        GameObject.Find("GameEngine").GetComponent<GameEngine>().cantKillBoss();
        timer.GetComponent<Text>().text = "";
        finishDeathAnimation();
    }

    private void Death()
    {
        anim.SetTrigger("death");

        if(tempo > 0) { StopAllCoroutines(); timer.GetComponent<Text>().text = ""; }

        UnityEngine.Object goldpref = Resources.Load("Prefabs/Coin", typeof(GameObject));

        int rand = UnityEngine.Random.Range(2, 8);
        GameObject coin;
        Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y, -1f);


        for(int i = 0; i<rand; i++)
        {
            coin = (GameObject)Instantiate(goldpref, coinPosition, new Quaternion());
            double bn = gold / rand;
            bn = Math.Ceiling(bn);
            coin.GetComponent<Gold>().gold = bn;
        }
    }

    private void  finishDeathAnimation() //Executar quando a animação de morte acabar
    {
        GameObject.FindGameObjectWithTag("GameEngine").GetComponent<GameEngine>().enemySpawn();
        Destroy(gameObject);
    }

    private void updateHud()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("HUD#HPBar");
        GameObject healthCanvas = GameObject.FindGameObjectWithTag("HUD#HPValue");
        GameObject nameCanvas = GameObject.FindGameObjectWithTag("HUD#MName");

        double division;
        if (health > 0  && maxHealth > 0)
        {
            division = health / maxHealth;
        }
        else { division = 0; }

        healthBar.GetComponent<RectTransform>().localScale = new Vector3( float.Parse(division.ToString()), 1, 1);
        healthCanvas.GetComponent<Text>().text = Util.getInstance().format(health.ToString("f0"), 0);
        nameCanvas.GetComponent<Text>().text = nameMonster;
    }



    



}
