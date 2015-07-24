using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {
	public static Pool current;

	public List<GameObject> Disparos;
	public List<GameObject> Enemigos;
	public List<GameObject> Explosiones;
	public GameObject disparo_basico;
	public GameObject enemigo_basico;
	public GameObject enemigo_2;
	public GameObject enemigo_3;
	public GameObject enemigo_4;
	public int numDisparos = 10;
	[HideInInspector]public float cadenciaDisparo;
	public int numEnemigos = 10;
	public bool aumentar_enemigos = false;	

	void Awake(){
		current = this;
		Disparos = new List<GameObject> ();
		for (int i = 0; i < numDisparos; i++) {
			GameObject go = (GameObject) Instantiate(disparo_basico);
			go.SetActive(false);
			Disparos.Add(go);
		}
		cadenciaDisparo = Disparos[0].GetComponent<Disparo>().cadencia;

		Enemigos = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			GameObject go = null;
			switch(Random.Range(0,4)){ //[0,3]
			case 0:
				go = (GameObject) Instantiate(enemigo_basico);
				break;
			case 1:
				go = (GameObject) Instantiate(enemigo_2);
				break;
			case 2:
				go = (GameObject) Instantiate(enemigo_3);
				break;
			case 3:
				go = (GameObject) Instantiate(enemigo_4);
				break;
			}
			go.SetActive(false);
			Enemigos.Add(go);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	public GameObject Disparar(){
		for (int i = 0; i<Disparos.Count; i++) {
			if(!Disparos[i].activeInHierarchy)
				return Disparos[i];
		}
		//si no existe, se crea otro
		GameObject go = (GameObject) Instantiate(disparo_basico);
		Disparos.Add(go);
		//numDisparos++;
		return go;
	}

	public GameObject Crear_Enemigo(){
		for (int i = 0; i<numEnemigos; i++) {
			if(!Enemigos[i].activeInHierarchy)
				return Enemigos[i];
		}
		//si no existe , compueba si se puede crear otro
		if (aumentar_enemigos) {
			GameObject go = (GameObject)Instantiate (enemigo_basico);
			Enemigos.Add (go);
			return go;
		}
		return null;
	}

}
