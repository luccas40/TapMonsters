using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {


    public GameObject PLevel;
    public GameObject PDamage;
    public GameObject PCritC;
    public GameObject PCritD;
    public GameObject PLCost;



    float gastoBase = 10;


    private decimal gold = 0;


    private int level = 1;
    private float criticalRate = 1f;
    private float criticalDamage = 1.5f;
    private decimal damage = 1m;



	void Start () {
        updateHud();
	}
	
	
	void Update () { //Controller

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if ("GoldCoin".Equals(hit.collider.tag)) {
                        hit.collider.gameObject.GetComponent<Gold>().hitGold();
                    }
                    else if ("Enemy".Equals(hit.collider.tag)){
                        attack();
                    }
                }
                else
                {
                    attack();
                }
            }
        }

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            attack();

        }

#else //Android IOS Windows Phone     
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(touch.rawPosition);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                    if(hit.collider != null)
                    {
                        if ("GoldCoin".Equals(hit.collider.tag))
                        {
                            hit.collider.gameObject.GetComponent<Gold>().hitGold();
                        }else if("Enemy".Equals(hit.collider.tag)){
                            attack();
                        }
                    }else
                    {
                        attack();
                    }
                }
            }
        }
#endif

    }


    public void levelUp(int levelPlus)
    {
        if (loseGold(cost2LevelUp()))
        {
            this.level += levelPlus;
            damageCalculator();
            updateHud();
        }
    }

    decimal cost2LevelUp()
    {
        decimal cost = (decimal)gastoBase;
        cost *= (decimal)Mathf.Pow(1.05f, (level - 1));
        cost += (decimal)Mathf.Pow(1.05f, (level));
        //cost *= level^level;
        //cost *= (decimal)Mathf.Pow(0.904f, (level - 1));
        cost = System.Math.Ceiling(cost);
        Debug.Log(cost);
        return cost;
    }


    public void earnGold(decimal valor)
    {
        gold += valor;
        updateHud();
    }

    public bool loseGold(decimal valor)
    {
        if(gold < valor){ return false; }
        gold -= valor;
        updateHud();
        return true;
    }

    public void setLevel(int level)
    {
        this.level = level;
        damageCalculator();
    }

    public decimal getGold() { return this.gold;  }
    public int getLevel() { return this.level; }


    private void attack()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null && !enemy.GetComponent<Enemy>().death)
        {

            float rand = Random.Range(1, 100);
            decimal danoCalc;
            if (rand > 0 && rand <= criticalRate) //critico
            {
                danoCalc = damage * decimal.Parse("" + criticalDamage);
                danoCalc = System.Math.Ceiling(danoCalc);
                Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxtCritical", typeof(GameObject));
                GameObject dmgObj = (GameObject)Instantiate(dmgTxt);
                dmgObj.GetComponent<TextMesh>().text = NumberFormat.getInstance().format(danoCalc);
            }
            else //dano normal
            {
                danoCalc = damage;
                danoCalc = System.Math.Ceiling(danoCalc);
                Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxt", typeof(GameObject));
                GameObject dmgObj = (GameObject)Instantiate(dmgTxt);
                dmgObj.GetComponent<TextMesh>().text = NumberFormat.getInstance().format(danoCalc);
            }
            enemy.GetComponent<Enemy>().hitMe(danoCalc);
        }
    }



    void damageCalculator()
    {
        damage = (decimal)(level * Mathf.Pow(1.01f, level));
        damage = System.Math.Ceiling(damage);
    }


    private void updateHud()
    {

        GameObject.FindGameObjectWithTag("HUD#Gold").GetComponent<Text>().text = ""+NumberFormat.getInstance().format(gold);
        PLevel.GetComponent<Text>().text = ""+level;
        PDamage.GetComponent<Text>().text = NumberFormat.getInstance().format(damage);
        PCritC.GetComponent<Text>().text = criticalRate+"%";
        PCritD.GetComponent<Text>().text = ((criticalDamage-1)*100)+"%";
        PLCost.GetComponent<Text>().text = NumberFormat.getInstance().format(cost2LevelUp());

    }


}
