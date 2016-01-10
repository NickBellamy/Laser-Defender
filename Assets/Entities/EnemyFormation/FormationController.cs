using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {
	
	public Object enemyPrefab;
	public Object enemyPrefab1;
	public float width = 12f;
	public float height = 4f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;

	private float modifier = 0;	
	private Object shipType;
	private bool movingRight = true;
	private float xMax;
	private float xMin;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
		xMax = rightBoundary.x;
		xMin = leftBoundary.x;
		
		SpawnUntilFull();
	}
	
	/*void SpawnEnemies() {
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}*/
	
	void SpawnUntilFull() {
		Transform freePosition = NextFreePosition();
		if (Random.value > modifier) {
			shipType = enemyPrefab;
		} else {
			shipType = enemyPrefab1;
		}
		if (freePosition) {
			GameObject enemy = Instantiate(shipType, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
		modifier += 0.005f;
	}
	
	public void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if(movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
		// Check if the formation is going outside of the play space
		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		if (leftEdgeOfFormation < xMin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > xMax) {
			movingRight = false;
		}
		
		if(AllMembersDead()) {
			Debug.Log("Empty formation");
			SpawnUntilFull();
		}
	}
	
	Transform NextFreePosition() {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}
	
	bool AllMembersDead() {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}

}
