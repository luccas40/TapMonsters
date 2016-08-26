using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour {


    public decimal gold = 10;
    bool ativado = false;


	void Start () {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GoldCoin"), LayerMask.NameToLayer("Enemy"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GoldCoin"), LayerMask.NameToLayer("GoldCoin"));
        float rand = UnityEngine.Random.Range(-.32f, .32f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, 1.3f) *7f, ForceMode2D.Impulse);
        StartCoroutine(getOuroSemClicar());
    }


    public void hitGold()
    {
        if (!ativado)
        {
            GetComponentInChildren<TextMesh>().text = "+"+ NumberFormat.getInstance().format(gold);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().earnGold(gold);
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

        yield return new WaitForSeconds(5f);
        hitGold();
    }

}
