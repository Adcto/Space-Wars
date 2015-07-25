using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public float cd_enemigos = 2.5f;
	public Text score;
	public Text multiplicadorText;
	public Image multiplicadorBarra;
	private int mult = 1;
	public float resetMult = 0.5f;
	private float time = 0.5f;
	public static GameManager current;
	public List<Transform> SpawnPoints;

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
			if(time > 0){
				time -= Time.deltaTime;
				multiplicadorBarra.fillAmount = time/resetMult;
			}
			else {
				mult = 1;
				multiplicadorText.text = "x" + mult.ToString();
				multiplicadorBarra.fillAmount = 0;
			}
		}
	}

	void CrearEnemigos(){
		GameObject enemy = Pool.current.Crear_Enemigo ();
		if (enemy == null)
			return;
		enemy.transform.position = SpawnPoints[Random.Range (0, 8)].position;
		enemy.SetActive (true);
	}
	public void AddScore(int punt){
		punt *= mult;
		//Para mejorar el sistema de multiplicador, solo habria que sumar algo a tiempo, y cuando tiempo >=resetmult -> mult++
		//Quizas x punts son = cierta cantidad de tiempo? hay que pensarlo
		mult++;
		time = resetMult;
		multiplicadorText.text = "x" + mult.ToString();
		string[] split = score.text.Split(':');
		punt += int.Parse (split [1]);
		score.text = "Score: " + punt.ToString();
	}
}
