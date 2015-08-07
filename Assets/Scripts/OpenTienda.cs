using UnityEngine;
using System.Collections;

public class OpenTienda : MonoBehaviour {

	public MenuManager menu;
	public Menu tienda;
	public float tiempoEntrada = 1;
	private float time = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnEnable(){
		Invoke ("Desactivate", 4);
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
				Time.timeScale = 0;
				//OnTriggerExit2D -> time = 0 ??; asi creo q ya esta bien

			}
		}
	}

}

