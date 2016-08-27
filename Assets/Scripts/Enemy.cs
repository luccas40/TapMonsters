using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {

    public string nameMonster;
    public float gastoBase;
    decimal health;
    decimal maxHealth;
    decimal gold;

    public bool death = false;

    Animator anim;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        anim = this.gameObject.GetComponent<Animator>();
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
        if (!death)
        {
            health = Decimal.Subtract(health, damage);
            if (health <= Decimal.Zero)
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

    private void Death()
    {
        anim.SetTrigger("death");
        UnityEngine.Object goldpref = Resources.Load("Prefabs/Coin", typeof(GameObject));

        int rand = UnityEngine.Random.Range(2, 8);
        GameObject coin;
        Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y, -1f);


        for(int i = 0; i<rand; i++)
        {
            coin = (GameObject)Instantiate(goldpref, coinPosition, new Quaternion());
            coin.GetComponent<Gold>().gold = Math.Ceiling(gold / rand);
        }
        StartCoroutine(waitFinishAnimation());
    }

    IEnumerator waitFinishAnimation()
    {

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("AfterDeath"))
        {
            yield return null;
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
