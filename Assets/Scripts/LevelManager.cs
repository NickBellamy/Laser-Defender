using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		Application.LoadLevel(name);
	}
	
	public void LoadLevel(string name, float wait){
		Debug.Log ("New Level load: " + name);
		StartCoroutine (Wait(name, wait));
	}
	
	IEnumerator Wait(string name, float wait) {
		yield return new WaitForSeconds(wait);
		Application.LoadLevel(name);
	}
	
	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

}
