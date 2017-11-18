using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace PwndaGames.TapMonsters {

    public class Enemy : MonoBehaviour {

        public string nameMonster;
        Pitolar health;
        Pitolar maxHealth;
        double gold;

        float tempo;
        public float BossTimer { get { return tempo; } }
        GameObject timer;

        private EnemyState state;
        public EnemyState State { get { return state; } }

        public EnemyType Tipo;

        Animator animator;

        void Start()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
            animator = this.gameObject.GetComponent<Animator>();
            timer = GameObject.Find("Canvas/HUD/MonsterHUD/Timer/Time");
            state = EnemyState.Alive;
        }

        public void setVida(Pitolar hp)
        {
            this.health = this.maxHealth = hp;
            updateHud();
        }

        public void setGold(double gold)
        {
            this.gold = gold;
        }

        public void hitMe(Pitolar damage)
        {
            if (state == EnemyState.Alive)
            {
                health -= damage;
                if (health <= 0d)
                {
                    state = EnemyState.Dying;
                    health = Pitolar.ZERO;
                    updateHud();
                    Death();
                    return;
                }
                animator.SetTrigger("hit");
                updateHud();
            }
        }

        public void setBoss(float tempo)
        {
            this.tempo = tempo;
            Tipo = EnemyType.Boss;
            StartCoroutine(bossTime());
        }

        IEnumerator bossTime()
        {
            while (tempo > 0)
            {
                yield return new WaitForSeconds(0.1f);
                tempo -= 0.1f;
                timer.GetComponent<Text>().text = tempo.ToString("0 0 . 0");
            }
            timer.GetComponent<Text>().text = "";
        }

        private void Death()
        {
            animator.SetTrigger("death");

            if (tempo > 0) { StopAllCoroutines(); timer.GetComponent<Text>().text = ""; }

            UnityEngine.Object goldpref = Resources.Load("Prefabs/Coin", typeof(GameObject));

            int rand = UnityEngine.Random.Range(2, 8);
            GameObject coin;
            Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y, -2f);


            for (int i = 0; i < rand; i++)
            {
                coin = (GameObject)Instantiate(goldpref, coinPosition, new Quaternion());
                Pitolar bn = new Pitolar(gold, 0) / rand;
                coin.GetComponent<Gold>().gold = bn;
            }
        }

        private void finishDeathAnimation() //Executar quando a animação de morte acabar
        {
            state = EnemyState.Dead;            
        }

        private void updateHud()
        {
            GameObject healthBar = GameObject.FindGameObjectWithTag("HUD#HPBar");
            GameObject healthCanvas = GameObject.FindGameObjectWithTag("HUD#HPValue");
            GameObject nameCanvas = GameObject.FindGameObjectWithTag("HUD#MName");

            float division;
            if (health > 0 && maxHealth > 0)
            {
                division = (float)(health / maxHealth).valor;
            }
            else { division = 0; }
            healthBar.GetComponent<RectTransform>().localScale = new Vector3(division, 1, 1);
            healthCanvas.GetComponent<Text>().text = health.ToString();
            nameCanvas.GetComponent<Text>().text = nameMonster;
        }





    }

}
