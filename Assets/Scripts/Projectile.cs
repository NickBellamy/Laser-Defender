using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float thisDamage;
	public void SetDamage(float damage) {
		thisDamage = damage;
	}
	
	public float GetDamage() {
		return thisDamage;	
	}
	
	public void Hit() {
		Destroy (gameObject);
	}

}
