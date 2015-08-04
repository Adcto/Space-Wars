using UnityEngine;
using System.Collections;

public class DesactivarEmpty : MonoBehaviour {

	public int hijosDesactivados = 0;
	public float dispersion = 0;
	// Use this for initialization
	void Start () {
		//positions = transform.GetComponentsInChildren<Transform> (true);
	}
	void OnEnable(){
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
	// Update is called once per frame
	void Update () {
	
	}

	public void Desactivar(){
		hijosDesactivados++;
		if (hijosDesactivados == transform.childCount && gameObject.activeInHierarchy) {
			gameObject.SetActive(false);
		}
	}
}
