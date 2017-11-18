using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PwndaGames.TapMonsters;

public class SoldierController : MonoBehaviour {

    public int id;
    public string Nome;
    public double BaseGold;
    public float BaseAtkSpeed;
    public double BaseDamage;

    private GameObject button;

    private Soldier soldier;
    public Soldier Soldado { get { return soldier; } set { soldier = value; } }


    Animator anim;


    public void setButton(GameObject obj)
    {
        this.button = obj;
    }

	void Start () {
        anim = GetComponent<Animator>();
	}

    void Update()
    {
        Text[] t = button.GetComponentsInChildren<Text>();
        t[0].text = Nome;
        t[1].text = "" + soldier.Level;
        t[2].text = "" + BaseGold;
    }


    private void spawn()
    {
        StartCoroutine(atkCD());
    }

    private void attack()
    {
        GameObject e = GameObject.FindGameObjectWithTag("Enemy");
        if(e != null)
        {
            e.GetComponent<Enemy>().hitMe(soldier.Damage);
        }
    }

    

    IEnumerator atkCD()
    {
        yield return new WaitForSeconds(soldier.AttackSpeed);
        anim.SetTrigger("attack");
        StartCoroutine(atkCD());
    }

 



}
