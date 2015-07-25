using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public float cd_enemigos = 2.5f;
	public Text score;
	public Text multiplicador;
	private int mult = 1;
	public float resetMult = 0.5f;
	private float time = 0.5f;
	public static GameManager current;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {

		InvokeRepeating ("CrearEnemigos", cd_enemigos, cd_enemigos);
	}
	
	// Update is called once per frame
	void Update () {
		if (mult > 1) {
			if(time > 0)
				time -= Time.deltaTime;
			else {
				mult = 1;
				multiplicador.text = "x" + mult.ToString();
			}
		}

	}

	void CrearEnemigos(){
		GameObject enemy = Pool.current.Crear_Enemigo ();
		if (enemy == null)
			return;
		enemy.transform.position = Vector2.one * Random.Range (-4, 4);
		enemy.SetActive (true);
	}
	public void AddScore(int punt){
		punt *= mult;
		mult++;
		time = resetMult;
		multiplicador.text = "x" + mult.ToString();
		string[] split = score.text.Split(':');
		punt += int.Parse (split [1]);
		score.text = "Score: " + punt.ToString();
	}
}
