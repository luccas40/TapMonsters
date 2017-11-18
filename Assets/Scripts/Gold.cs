using UnityEngine;
using System.Collections;
using PwndaGames.TapMonsters;

public class Gold : MonoBehaviour {


    public Pitolar gold = Pitolar.ZERO;
    bool ativado = false;


	void Start () {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GoldCoin"), LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GoldCoin"), LayerMask.NameToLayer("GoldCoin"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GoldCoin"), LayerMask.NameToLayer("Soldier"));
        float rand = Random.Range(-.32f, .32f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, 1.3f) *7f, ForceMode2D.Impulse);
        StartCoroutine(getOuroSemClicar());
    }


    public void hitGold()
    {
        if (!ativado)
        {
            gold.forceCeiling();
            GetComponentInChildren<TextMesh>().text = "+"+ gold.ToString();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().earnGold(gold);
            ativado = true;            
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
            StartCoroutine(destroyCoutdown());
        }
    }


    IEnumerator destroyCoutdown()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(this.gameObject);
    }

    IEnumerator getOuroSemClicar()
    {
        yield return new WaitForSeconds(2.5f);
        hitGold();
    }

}
