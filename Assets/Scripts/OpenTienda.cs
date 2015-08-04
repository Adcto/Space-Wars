using UnityEngine;
using System.Collections;

public class OpenTienda : MonoBehaviour {

	public MenuManager menu;
	public Menu tienda;
	public float tiempoEntrada = 2;
	private float time = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			time+=Time.fixedDeltaTime;
			if(time >= tiempoEntrada){
				time = 0;
				Debug.Log("Entrando en la tienda");
				gameObject.SetActive(false);
				menu.ShowMenu(tienda);
				Time.timeScale = 0;
			}
		}
	}

}

