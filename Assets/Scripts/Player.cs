using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {


    public GameObject heroPainel;



    private Soldier[] soldiers = new Soldier[10];


    double gastoBase = 10;

    private int level = 1;
    private double gold = 0;
    private float criticalRate = 1f;
    private float criticalDamage = 1.5f;
    private double damage = 1d;



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


    public void setSoldier(int id, int level)
    {
        if (soldiers[id] == null)
        {
            instantiateSoldier(id, level);
        }
        else
        {
            soldiers[id].setLevel(level);
        }

        updateSoldierTotalDamage();
    }

    public void levelUpSoldier(int id)
    {
        if (soldiers[id] == null)
        {
            instantiateSoldier(id, 1);
        }
        else
        {
            soldiers[id].levelUp();
        }

        updateSoldierTotalDamage();
    }

    void instantiateSoldier(int id, int level)
    {
        GameObject t = (GameObject)Instantiate(Resources.Load("Prefabs/Soldiers/Soldier" + id, typeof(GameObject)));
        GameObject btn = GameObject.Find("Canvas/HUD/SoldierPanel/Scroll View/Viewport/Content/Soldier" + id + "Btn");
        Soldier s = t.GetComponent<Soldier>();
        s.setButton(btn);
        s.setLevel(level);
        soldiers[id] = s;
    }

    public Soldier[] getSoldiers() { return soldiers; }

    public void levelUp(int levelPlus)
    {
        if(level < 5000)
            if (loseGold(cost2LevelUp()))
            {
                this.level += levelPlus;
                damageCalculator();
                updateHud();
            }
    }

    double cost2LevelUp()
    {
        double cost = gastoBase * level / 2;
        cost += System.Math.Pow(1.03d, (level - 1));
        cost *= System.Math.Pow(1.047d, level);
        cost *= System.Math.Pow(0.997f, (level - 1));
        return cost;
    }


    public void earnGold(double valor)
    {
        gold += valor;
        updateHud();
    }

    public bool loseGold(double valor)
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

    public double getGold() { return this.gold;  }
    public int getLevel() { return this.level; }

    private void attack()
    {
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null && !enemy.GetComponent<Enemy>().death)
        {
            GameObject dmgObj;
            float rand = Random.Range(1, 100);
            double danoCalc;
            if (rand > 0 && rand <= criticalRate) //critico
            {
                danoCalc = damage * criticalDamage;
                Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxtCritical", typeof(GameObject));
                dmgObj = (GameObject)Instantiate(dmgTxt);
                
            }
            else //dano normal
            {
                danoCalc = damage;
                Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxt", typeof(GameObject));
                dmgObj = (GameObject)Instantiate(dmgTxt);
            }
            dmgObj.GetComponent<TextMesh>().text = Util.getInstance().format(danoCalc.ToString("f0"), 0);
            enemy.GetComponent<Enemy>().hitMe(danoCalc);
        }
        
    }


    void damageCalculator()
    {
        damage = level * (System.Math.Pow(1.03d, level));
    }

    private void updateSoldierTotalDamage()
    {
        double sDamageTotal = 0;
        foreach (Soldier s in soldiers)
        {
            if (s != null) { sDamageTotal += s.getDamage(); }
        }

        GameObject.Find("Canvas/HUD/SoldierPanel/Scroll View/Viewport/Content/TotalDamageTXT/DanoTotal").GetComponent<Text>().text = "" + sDamageTotal;
    }

    private void updateHud()
    {

        GameObject.FindGameObjectWithTag("HUD#Gold").GetComponent<Text>().text = Util.getInstance().format(gold.ToString("f0"), 0);
        Text[] t = heroPainel.GetComponentsInChildren<Text>();
        t[3].GetComponentInChildren<Text>().text = ""+level;
        t[5].GetComponentInChildren<Text>().text = Util.getInstance().format(damage.ToString("f0"), 0);
        t[7].GetComponentInChildren<Text>().text = criticalRate+"%";
        t[9].GetComponentInChildren<Text>().text = ((criticalDamage-1)*100)+"%";
        t[1].text = Util.getInstance().format(cost2LevelUp().ToString("f0"), 0);

        }


}
