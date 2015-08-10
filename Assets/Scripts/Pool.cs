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
	public List<GameObject> AgujerosNegros;
	public List<GameObject> EnemigosRecto;
	public List<GameObject> Asteroides;
	public List<GameObject> ExplosionesDisparos;
	public List<GameObject> ExplosionesEnemigos;
	public GameObject disparo;
	public GameObject enemigo_basico;
	public GameObject enemigo_2;
	public GameObject enemigo_3;
	public GameObject enemigo_4;
	public GameObject agujero;
	public GameObject recto;
	public GameObject asteroide;
	public GameObject hit_disparo;
	public GameObject explosion_enemigo;
	public int tiposEnemigos = 5;
	public int numDisparos = 10;
	public int numEnemigos = 10;
	public bool aumentar_enemigos = false;	
	public GameObject powerUp;

	void Awake(){
		current = this;
		Disparos = new List<GameObject> ();
		GameObject go;
		Enemigos1 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (enemigo_basico);
			go.SetActive (false);
			Enemigos1.Add (go);
		}
		Enemigos2 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (enemigo_2);
			go.SetActive (false);
			Enemigos2.Add (go);
		}
		Enemigos3 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (enemigo_3);
			go.SetActive (false);
			Enemigos3.Add (go);
		}
		Enemigos4 = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (enemigo_4);
			go.SetActive (false);
			Enemigos4.Add (go);
		}
		EnemigosRecto = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (recto);
			go.SetActive (false);
			EnemigosRecto.Add (go);
		}
		Asteroides = new List<GameObject> ();
		for (int i = 0; i < numEnemigos; i++) {
			go = (GameObject)Instantiate (asteroide);
			go.SetActive (false);
			Asteroides.Add (go);
		}
		AgujerosNegros = new List<GameObject> ();
		for (int i = 0; i < 4; i++) {
			go = (GameObject)Instantiate (agujero);
			go.SetActive (false);
			AgujerosNegros.Add (go);
		}
		ExplosionesDisparos = new List<GameObject>();
		for (int i = 0; i < 5; i++) {
			go = (GameObject)Instantiate (hit_disparo);
			go.SetActive (false);
			ExplosionesDisparos.Add (go);
		}
		ExplosionesEnemigos = new List<GameObject>();
		for (int i = 0; i < 8; i++) {
			go = (GameObject)Instantiate (explosion_enemigo);
			go.SetActive (false);
			ExplosionesEnemigos.Add (go);
		}

	}

	// Use this for initialization
	void Start () {
		EquiparDisparos ();
	}

	public void EquiparDisparos(){
		for (int i = 0; i<Disparos.Count; i++) {
			Destroy(Disparos[i]);
		}
		Disparos.Clear ();
		for (int i = 0; i < numDisparos; i++) {
			GameObject go = (GameObject) Instantiate(disparo);
			go.SetActive(false);
			Disparos.Add(go);
		}

		float cadencia;
		if(Disparos[0].transform.childCount > 0)
			cadencia =Disparos [0].GetComponent<DesactivarEmpty> ().cadencia;
		else
			cadencia =Disparos [0].GetComponent<Disparo> ().cadencia;

//		if (Disparos [0].transform.childCount > 0)
//			cadencia = Disparos [0].GetComponentInChildren<Disparo> ().cadencia;
//		else 
//			cadencia = Disparos [0].GetComponent<Disparo> ().cadencia;
		PlayerController.current.cadenciaDisparo =cadencia;
	}

	public GameObject Disparar(){
		for (int i = 0; i<Disparos.Count; i++) {
			if(!Disparos[i].activeInHierarchy)
				return Disparos[i];
		}
		//si no existe, se crea otro
		GameObject go = (GameObject) Instantiate(disparo);
		go.SetActive (false);
		Disparos.Add(go);
		//numDisparos++;
		return go;
	}

	public GameObject Crear_Hit_Disparo(){
		for (int i = 0; i<ExplosionesDisparos.Count; i++) {
			if(!ExplosionesDisparos[i].activeInHierarchy)
				return ExplosionesDisparos[i];
		}
		//si no existe, se crea otro
		GameObject go = (GameObject) Instantiate(hit_disparo);
		go.SetActive (false);
		ExplosionesDisparos.Add(go);
		//numDisparos++;
		return go;
	}
	public GameObject Crear_Explosion_Enemigo(){
		for (int i = 0; i<ExplosionesEnemigos.Count; i++) {
			if(!ExplosionesEnemigos[i].activeInHierarchy)
				return ExplosionesEnemigos[i];
		}
		//si no existe, se crea otro
		GameObject go = (GameObject) Instantiate(explosion_enemigo);
		go.SetActive (false);
		ExplosionesEnemigos.Add(go);
		//numDisparos++;
		return go;
	}

	public GameObject Crear_PowerUp(){
		if (powerUp.activeInHierarchy)
			return null;
		return powerUp;
	}


	public GameObject Crear_Enemigo(int tipo){
		int calidad = tipo / tiposEnemigos;
		if (tipo % tiposEnemigos != 0)
			calidad++;
		//Si la calidad es > 1 hay que aumentar las estadisticas el enemigo a spawnear 
		tipo %= tiposEnemigos;
		switch (tipo) {
		case 0: 
			for (int i = 0; i<AgujerosNegros.Count; i++) {
				if (!AgujerosNegros [i].activeInHierarchy) {
					AgujerosNegros [i].GetComponent<EnemyController> ().calidad = calidad;
					return AgujerosNegros [i];
				}
			}
			break;
		case 1:
			for (int i = 0; i<Enemigos1.Count; i++) {
				if (!Enemigos1 [i].activeInHierarchy) { 
					Enemigos1 [i].GetComponent<EnemyController> ().calidad = calidad;
					return Enemigos1 [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_basico);
				go.GetComponent<EnemyController> ().calidad = calidad;
				Enemigos1.Add (go);
				return go;
			}

			break;
		case 2:
			for (int i = 0; i<Enemigos2.Count; i++) {
				if (!Enemigos2 [i].activeInHierarchy) {
					Enemigos2 [i].GetComponent<EnemyController> ().calidad = calidad;
					return Enemigos2 [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_2);
				go.GetComponent<EnemyController> ().calidad = calidad;
				Enemigos2.Add (go);
				return go;
			}

			break;
		case 3:
			for (int i = 0; i<Enemigos3.Count; i++) {
				if (!Enemigos3 [i].activeInHierarchy) {
					Enemigos3 [i].GetComponent<EnemyController> ().calidad = calidad;
					return Enemigos3 [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_3);
				go.GetComponent<EnemyController> ().calidad = calidad;
				Enemigos3.Add (go);
				return go;
			}

			break;
		case 4:
			for (int i = 0; i<Enemigos4.Count; i++) {
				if (!Enemigos4 [i].activeInHierarchy) {
					Enemigos4 [i].GetComponent<EnemyController> ().calidad = calidad;
					return Enemigos4 [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) {
				GameObject go = (GameObject)Instantiate (enemigo_4);
				go.GetComponent<EnemyController> ().calidad = calidad;
				Enemigos4.Add (go);
				return go;
			}
			
			break;
		case 5:			//EnemigoRecto
			for (int i = 0; i<EnemigosRecto.Count; i++) {
				if (!EnemigosRecto [i].activeInHierarchy) {
					EnemigosRecto [i].GetComponent<EnemyController> ().calidad = calidad;
					return EnemigosRecto [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos && EnemigosRecto.Count < 30) { //aprox
				GameObject go = (GameObject)Instantiate (recto);
				go.GetComponent<EnemyController> ().calidad = calidad;
				EnemigosRecto.Add (go);
				return go;
			}
		
			break;
		case 6:		//Asteroide
			for (int i = 0; i<Asteroides.Count; i++) {
				if (!Asteroides [i].activeInHierarchy) {
					Asteroide nuevo = Asteroides [i].GetComponent<Asteroide> ();
					nuevo.tipo = 1;
					nuevo.direction = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f));
					nuevo.calidad = calidad;
					return  Asteroides [i];
				}
			}
			//si no existe , compueba si se puede crear otro
			if (aumentar_enemigos) { //aprox
				GameObject go = (GameObject)Instantiate (asteroide);
				Asteroide nuevo = go.GetComponent<Asteroide> ();
				nuevo.tipo = 1;
				nuevo.direction = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f));
				nuevo.calidad = calidad;
				Asteroides.Add (go);
				return go;
			}
			
			break;
		}


		return null;



	}

}
