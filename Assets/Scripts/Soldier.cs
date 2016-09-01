using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {

    public string Nome;
    public float BaseAtkSpeed;
    public double BaseDamage;


    float atkSpeed = 2.5f;
    double damage = 1d;

    Animator anim;



	void Start () {
        anim = GetComponent<Animator>();
	}
	

    private void spawn()
    {
        StartCoroutine(atkCD());
    }

    private void attack()
    {
        Enemy enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.hitMe(damage);
        }
    }

    IEnumerator atkCD()
    {
        yield return new WaitForSeconds(atkSpeed);
        anim.SetTrigger("attack");
        StartCoroutine(atkCD());
    }


}
