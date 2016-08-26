using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour {

    public string nameMonster;
    decimal health;
    decimal maxHealth;
    decimal gold;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
    }

    public void setVida(decimal hp)
    {
        this.health = this.maxHealth = hp;
        updateHud();
    }

    public void setGold(decimal gold)
    {
        this.gold = gold;
    }

    public void hitMe(decimal damage)
    {
        
        health = Decimal.Subtract(health, damage);
        if (health <= Decimal.Zero) { health = 0; updateHud(); Death(); return; }
        updateHud();
    }

    private void Death()
    {
        UnityEngine.Object goldpref = Resources.Load("Prefabs/Coin", typeof(GameObject));

        int rand = UnityEngine.Random.Range(2, 8);
        GameObject[] coins = new GameObject[rand];

        Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y, -1f);


        for(int i = 0; i<rand; i++)
        {
            coins[i] = (GameObject)Instantiate(goldpref, coinPosition, new Quaternion());
            coins[i].GetComponent<Gold>().gold = Math.Ceiling(gold / rand);
        }


        GameObject.FindGameObjectWithTag("GameEngine").GetComponent<GameEngine>().enemySpawn();
        Destroy(gameObject);
    }

    private void updateHud()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("HUD#HPBar");
        GameObject healthCanvas = GameObject.FindGameObjectWithTag("HUD#HPValue");
        GameObject nameCanvas = GameObject.FindGameObjectWithTag("HUD#MName");

        decimal division;
        if (health != 0 && maxHealth != 0)
        {
            division = Decimal.Divide(health, maxHealth);
        }
        else { division = 0; }

        healthBar.GetComponent<RectTransform>().localScale = new Vector3( (float)division, 1, 1);
        healthCanvas.GetComponent<Text>().text = NumberFormat.getInstance().format(health);
        nameCanvas.GetComponent<Text>().text = nameMonster;
    }



    



}
