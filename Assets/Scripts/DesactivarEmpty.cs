using UnityEngine;
using System.Collections;

public class DesactivarEmpty : MonoBehaviour {

	public int hijosDesactivados = 0;
	// Use this for initialization
	void Start () {

		//positions = transform.GetComponentsInChildren<Transform> (true);
	}
	void OnEnable(){
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive (true);
		}
		hijosDesactivados = 0;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void Desactivar(){
		hijosDesactivados++;
		if (hijosDesactivados == transform.childCount && gameObject.activeInHierarchy) {

			Debug.Log("Desactivando Disparo" + GetHashCode());
			gameObject.SetActive(false);
			//Da error, pero se supone q esta bien :S
		}
	}
}
