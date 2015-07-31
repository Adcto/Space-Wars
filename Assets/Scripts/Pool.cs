using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {
	public static Pool current;

	public List<GameObject> Disparos;
	public List<GameObject> Enemigos1;
	public List<GameObject> Enemigos2;
	public List<GameObject> Enemigos3;
	public List<GameObject> Enemigos4;
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

		Enemigos1 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			GameObject go = (GameObject)Instantiate (enemigo_basico);
			go.SetActive (false);
			Enemigos1.Add (go);
		}
		Enemigos2 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			GameObject go = (GameObject)Instantiate (enemigo_2);
			go.SetActive (false);
			Enemigos2.Add (go);
		}
		Enemigos3 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			GameObject go = (GameObject)Instantiate (enemigo_3);
			go.SetActive (false);
			Enemigos3.Add (go);
		}
		Enemigos4 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			GameObject go = (GameObject)Instantiate (enemigo_4);
			go.SetActive (false);
			Enemigos4.Add (go);
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

	public GameObject Crear_Enemigo(int tipo){
		int calidad = tipo / 4;
		if (tipo % 4 != 0)
			calidad++;
		//Si la calidad es > 1 hay que aumentar las estadisticas el enemigo a spawnear 
		tipo %= 4;
		switch (tipo) {
		case 0:
			for (int i = 0; i<Enemigos4.Count; i++) {
				if(!Enemigos4[i].activeInHierarchy) 
					return Enemigos4[i];
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_4);
				Enemigos4.Add (go);
				return go;
			}

			break;
		case 1:
			for (int i = 0; i<Enemigos1.Count; i++) {
				if(!Enemigos1[i].activeInHierarchy) 
					return Enemigos1[i];
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_basico);
				Enemigos1.Add (go);
				return go;
			}

			break;
		case 2:
			for (int i = 0; i<Enemigos2.Count; i++) {
				if(!Enemigos2[i].activeInHierarchy) 
					return Enemigos2[i];
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_2);
				Enemigos2.Add (go);
				return go;
			}

			break;
		case 3:
			for (int i = 0; i<Enemigos3.Count; i++) {
				if(!Enemigos3[i].activeInHierarchy) 
					return Enemigos3[i];
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_3);
				Enemigos3.Add (go);
				return go;
			}

			break;
		}
		return null;



	}

}
