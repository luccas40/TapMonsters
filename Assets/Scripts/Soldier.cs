using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Soldier : MonoBehaviour {

    public int id;


    public string Nome;
    public double BaseGold;
    public float BaseAtkSpeed;
    public double BaseDamage;

    private GameObject button;

    int level = 1;
    float atkSpeed = 2.5f;
    double damage = 10d;

    Animator anim;


    public void setButton(GameObject obj)
    {
        this.button = obj;
    }

	void Start () {
        anim = GetComponent<Animator>();
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
            e.GetComponent<Enemy>().hitMe(damage);
        }
    }

    public double getDamage() { return damage; }

    IEnumerator atkCD()
    {
        yield return new WaitForSeconds(atkSpeed);
        anim.SetTrigger("attack");
        StartCoroutine(atkCD());
    }

    public void damageCalculation()
    {
        damage = level * BaseDamage;
    }

    public void setLevel(int level)
    {
        this.level = level;
        damageCalculation();
        updateHUD();
    }
    public int getLevel() { return level; }

    public void levelUp()
    {
        level += 1;
        damageCalculation();
        updateHUD();
    }

    public void updateHUD()
    {
        Text[] t = button.GetComponentsInChildren<Text>();
        t[0].text = Nome;
        t[1].text = ""+level;
        t[2].text = ""+BaseGold;
    }

}
