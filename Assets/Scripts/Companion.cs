using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Companion : MonoBehaviour {
	public float radio = 2;
	public List<GameObject> Disparos;
	public GameObject disparo;
	public float cadencia;
	private float cooldownDisparo=0;
	public Transform posicionDisparo;

	void Awake(){
		transform.position +=  Vector3.right* radio;


		for (int i = 0; i < 10; i++) {
			GameObject go = (GameObject) Instantiate(disparo);
			go.SetActive(false);
			Disparos.Add(go);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerController.current.puedeDisparar) {
			if(PlayerController.current.apuntando){					//Solo dispara cuando pulsas el joystick derecho!
				cooldownDisparo += Time.deltaTime;
				if(cooldownDisparo >= cadencia){
					cooldownDisparo = 0;
					Disparar ();
				}
			}
		}
	}


	void Disparar(){
		GameObject shoot = Crear_Disparo ();
		shoot.transform.position = posicionDisparo.position;
		shoot.transform.rotation = transform.rotation;
		shoot.layer = gameObject.layer;
		shoot.SetActive (true);
	}


	GameObject Crear_Disparo(){
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
}
