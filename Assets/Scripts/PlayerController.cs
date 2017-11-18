using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PwndaGames.TapMonsters
{
    class PlayerController : MonoBehaviour
    {
        public GameObject heroPainel;
        private Text HeroLevelCost;
        private Text HeroLevel;
        private Text ToqueDano;
        private Text CriticalChance;
        private Text CriticalDamage;
        private Text GoldText;



        private Player player;
        public Player Jogador { get { return player; } }

        void Start()
        {
            player = new Player();

            HeroLevelCost = heroPainel.transform.Find("UpgradeDamageBtn/HeroLevelCost").GetComponent<Text>();
            HeroLevel = heroPainel.transform.Find("HeroLevelTXT/HeroLevel").GetComponent<Text>();
            ToqueDano = heroPainel.transform.Find("ToqueDanoTXT/ToqueDano").GetComponent<Text>();
            CriticalChance = heroPainel.transform.Find("CriticalChanceTXT/CriticalChance").GetComponent<Text>();
            CriticalDamage = heroPainel.transform.Find("CriticalDamageTXT/CriticalDamage").GetComponent<Text>();
            GoldText = GameObject.FindGameObjectWithTag("HUD#Gold").GetComponent<Text>();
            updateHud();
        }


        void Update()
        { //Controller

            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null)
                    {
                        if ("GoldCoin".Equals(hit.collider.tag))
                        {
                            hit.collider.gameObject.GetComponent<Gold>().hitGold();
                        }
                        else if ("Enemy".Equals(hit.collider.tag))
                        {
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

                        if (hit.collider != null)
                        {
                            if ("GoldCoin".Equals(hit.collider.tag))
                            {
                                hit.collider.gameObject.GetComponent<Gold>().hitGold();
                            }
                            else if ("Enemy".Equals(hit.collider.tag))
                            {
                                attack();
                            }
                        }
                        else
                        {
                            attack();
                        }
                    }
                }
            }
#endif

        }


        private void updateHud()
        {
            GoldText.text = player.Gold.ToString();
            HeroLevelCost.text = player.cost2LevelUp().ToString();
            HeroLevel.text = player.Level.ToString();
            ToqueDano.text = player.Damage.ToString();
            CriticalChance.text = player.CriticalRate + "%";
            CriticalDamage.text = ((player.CriticalDamage - 1) * 100) + "%";

        }


        private void updateSoldierTotalDamage()
        {    
            if(player.Soldados.Count > 0)
            GameObject.Find("Canvas/HUD/SoldierPanel/Scroll View/Viewport/Content/TotalDamageTXT/DanoTotal")
                .GetComponent<Text>().text = player.Soldados.Select(d => d.Value.Damage).Aggregate((a, b) => a + b).ToString();
        }

        private void attack()
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                Enemy e = enemy.GetComponent<Enemy>();
                if (e.State == EnemyState.Alive)
                {
                    GameObject dmgObj;
                    float rand = Random.Range(1, 100);
                    Pitolar danoCalc;
                    if (rand > 0 && rand <= player.CriticalRate) //critico
                    {
                        Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxtCritical", typeof(GameObject));
                        danoCalc = player.Damage * player.CriticalDamage;
                        dmgObj = (GameObject)Instantiate(dmgTxt);

                    }
                    else //dano normal
                    {
                        danoCalc = player.Damage;
                        Object dmgTxt = Resources.Load("Prefabs/Damage/DamageTxt", typeof(GameObject));
                        dmgObj = (GameObject)Instantiate(dmgTxt);
                    }
                    dmgObj.GetComponent<TextMesh>().text = danoCalc.ToString();
                    enemy.GetComponent<Enemy>().hitMe(danoCalc);
                }
            }
        }

        public void earnGold(Pitolar valor)
        {
            player.Gold += valor;
            updateHud();
        }

        private bool loseGold(Pitolar valor)
        {
            if (player.Gold < valor) { return false; }
            player.Gold -= valor;
            updateHud();
            return true;
        }

        public void levelUp(int levelPlus)
        {
            if (player.Level+levelPlus <= 5000)
                if (loseGold(player.cost2LevelUp()))
                {
                    player.Level += levelPlus;
                }
            updateHud();
        }

        void instantiateSoldier(Soldier soldier)
        {
            GameObject t = (GameObject)Instantiate(Resources.Load("Prefabs/Soldiers/Soldier" + soldier.ID, typeof(GameObject)));
            GameObject btn = GameObject.Find("Canvas/HUD/SoldierPanel/Scroll View/Viewport/Content/Soldier" + soldier.ID + "Btn");
            SoldierController s = t.GetComponent<SoldierController>();
            s.setButton(btn);
            s.Soldado = soldier;
            player.Soldados[soldier.ID] = soldier;
        }

        void instantiateSoldier(int id)
        {
            GameObject t = (GameObject)Instantiate(Resources.Load("Prefabs/Soldiers/Soldier" + id, typeof(GameObject)));
            GameObject btn = GameObject.Find("Canvas/HUD/SoldierPanel/Scroll View/Viewport/Content/Soldier" + id + "Btn");
            SoldierController s = t.GetComponent<SoldierController>();
            Soldier soldier = new Soldier(s.id, s.BaseGold, s.BaseAtkSpeed, s.BaseDamage);
            s.setButton(btn);
            s.Soldado = soldier;
            player.Soldados[soldier.ID] = soldier;
        }


        public void levelUpSoldier(int id)
        {
            if (player.Soldados[id] == null)
            {
                instantiateSoldier(id);
            }
            else
            {
                player.Soldados[id].levelUp();
            }

            updateSoldierTotalDamage();
        }

        public void setPlayer(Player p)
        {
            this.player = p;
            player.Level = p.Level;
            p.Soldados.ToList().ForEach(s => instantiateSoldier(s.Value));
            updateHud();
            updateSoldierTotalDamage();
        }

    }
}
