using UnityEngine;
using System.Collections;

public class OpenTienda : MonoBehaviour {

	public MenuManager menu;
	public Menu tienda;
	public float tiempoEntrada = 1;
	public float tiempoDesactivacion = 4;
	private float time = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnEnable(){
		Invoke ("Desactivate", tiempoDesactivacion);
	}

	void Desactivate(){
		time = 0;
		gameObject.SetActive(false);
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			time+=Time.fixedDeltaTime;
			if(time >= tiempoEntrada){
				Debug.Log("Entrando en la tienda");
				menu.ShowMenu(tienda);
				CancelInvoke();
				Desactivate();
				GameManager.current.CancelInvoke();
				//OnTriggerExit2D -> time = 0 ??; asi creo q ya esta bien

			}
		}
	}

}

