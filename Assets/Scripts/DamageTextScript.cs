using UnityEngine;
using System.Collections;

public class DamageTextScript : MonoBehaviour {

	void Start () {
        StartCoroutine(desapear());
	}
	
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime*3.5f);
	}

    IEnumerator desapear()
    {
        yield return new WaitForSeconds(.55f);
        Destroy(this.gameObject);
    }
}
