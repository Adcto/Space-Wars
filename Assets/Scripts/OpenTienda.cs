using UnityEngine;
using System.Collections;

public class OpenTienda : MonoBehaviour {

	public MenuManager menu;
	public Menu tienda;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			Debug.Log("Entrando en la tienda");
			gameObject.SetActive(false);
			menu.ShowMenu(tienda);
			Time.timeScale = 0;
		}
	}
}

