using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileDamage = 20f;
	public float health = 150f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public GameObject explosion;
	public Animator anim;

	
	private ScoreKeeper scoreKeeper;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		anim = GetComponent<Animator>();
	}
	
	void Update() {
			float probability = shotsPerSecond * Time.deltaTime;
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("In Formation 1")) {
				if (probability > Random.value) {
					Fire();
			}
		}
	}
	
	void Fire() {
		GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		missile.rigidbody2D.velocity = new Vector2(0 , -projectileSpeed);
		missile.GetComponent<Projectile>().SetDamage(projectileDamage);
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 0.1f);
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit ();
			if (health <= 0) {
				Die ();
			}
		}
	}
	
	void Die() {
		AudioSource.PlayClipAtPoint(deathSound, transform.position,0.08f);
		Destroy (gameObject);
		GameObject smokePuff = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		smokePuff.particleSystem.particleSystem.startColor = gameObject.GetComponent<SpriteRenderer>().color;
		Destroy (smokePuff, 2f);
		scoreKeeper.Score(scoreValue);
	}
	
	
}
