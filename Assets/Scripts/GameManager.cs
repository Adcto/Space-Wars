using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public float cd_enemigos = 2.5f;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("CrearEnemigos", cd_enemigos, cd_enemigos);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void CrearEnemigos(){
		GameObject enemy = Pool.current.Crear_Enemigo ();
		if (enemy == null)
			return;
		enemy.transform.position = Vector2.one * Random.Range (-4, 4);
		enemy.SetActive (true);
	}
}
