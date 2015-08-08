using UnityEngine;
using System.Collections;

public class DesactivarEmpty : MonoBehaviour {
	public int hijosDesactivados = 0;
	public float dispersion = 0;
	public float cadencia = 1;
	public float TimeToDestroy = 2.5f;
	// Use this for initialization
	void Start () {
		//positions = transform.GetComponentsInChildren<Transform> (true);
	}
	void OnEnable(){
		Invoke ("Disable", TimeToDestroy);
		float desviacion = 0;
		if (dispersion >0) {
			desviacion = Random.Range (-dispersion, dispersion);
		}
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,desviacion)); 

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive (true);
		}

		hijosDesactivados = 0;
		
	}
	

	
	void Disable(){
	
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Disparo>().Desactivate();
		}
		gameObject.SetActive (false);
	}
	
	void OnDisable(){
		CancelInvoke ();
	}
	
	public void Desactivar(){
		hijosDesactivados++;
		if (hijosDesactivados == transform.childCount && gameObject.activeInHierarchy) {
			gameObject.SetActive(false);
		}
	}
}
