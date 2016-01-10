using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float padding = 1f;
	public GameObject projectile;
	public float projectileSpeed;
	public float projectileDamage = 100f;
	public float firingRate = 0.15f;
	public float maxHealth = 250f;
	private float health;
	public AudioClip fireSound;
	public AudioClip deathSound;
	private GameObject smoke;
	public GameObject smokey;
	private float smokeThreshold = 61f;
	private bool shipHit = false;
	public AudioClip thud;
	public GameObject explosion;
	public Text myText;
	
	float xMin;
	float xMax;
	
	void Start () {
		health = maxHealth;
		myText = GameObject.Find ("Health").GetComponent<Text>();
		myText.text = health.ToString();
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
		
	}
	
	void Fire() {
		GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
		beam.GetComponent<Projectile>().SetDamage(projectileDamage);
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 0.05f);
	}

	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}
		
		if (Input.GetKey("left")) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		if (Input.GetKey("right")) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		
		// Restrict the player ship to the game space
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		
		if (shipHit) {
			Debug.Log(smoke.GetComponent<ParticleSystem>().loop);
			smoke.transform.position =  this.transform.position;
		}
		
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
			health = 0;
			Die ();
			} else {
				AudioSource.PlayClipAtPoint(thud, transform.position, 0.1f);	
			}
			myText.text = health.ToString();
		}
	}
	
	void Die() {
		AudioSource.PlayClipAtPoint(deathSound, transform.position,0.08f);
		smoke.GetComponent<ParticleSystem>().loop = false;
		Destroy (gameObject);
		GameObject smokePuff = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		smokePuff.particleSystem.particleSystem.startColor = gameObject.GetComponent<SpriteRenderer>().color;
		LevelManager man = FindObjectOfType<LevelManager>();
		man.LoadLevel("Win Screen", 2f);
	}
}
