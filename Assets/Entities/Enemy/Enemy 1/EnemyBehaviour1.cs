using UnityEngine;
using System.Collections;

public class EnemyBehaviour1 : MonoBehaviour {
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float projectileDamage = 60f;
	public float health = 500f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 300;
	public GameObject explosion;
	public GameObject smokey;
	public float smokeThreshold = 200f;
	public Animator anim;
	
	private GameObject smoke;
	private ScoreKeeper scoreKeeper;
	private bool shipHit = false;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		anim = GetComponent<Animator>();
	}
	
	void Update() {
		
		float probability = shotsPerSecond * Time.deltaTime;
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("In Formation")) {
			if (probability > Random.value) {
				Fire();
			}
		}

		
		if (shipHit) {
			Debug.Log(smoke.GetComponent<ParticleSystem>().loop);
			smoke.transform.position =  this.transform.position;
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
			if (health <= smokeThreshold && shipHit == false) {
				smoke = Instantiate(smokey, transform.position, Quaternion.identity) as GameObject;
				shipHit = true;
			}
			if (health <= 0) {
				Die ();
			}
		}
	}
	
	void Die() {
		AudioSource.PlayClipAtPoint(deathSound, transform.position,0.08f);
		smoke.GetComponent<ParticleSystem>().loop = false;
		Destroy (gameObject);
		GameObject smokePuff = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		smokePuff.particleSystem.particleSystem.startColor = gameObject.GetComponent<SpriteRenderer>().color;
		Destroy (smokePuff, 2f);
		Destroy (smoke, 2f);
		scoreKeeper.Score(scoreValue);
	}	
}
