using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	public float cd_enemigos = 2.5f;
	public Text score;
	public Text multiplicadorText;
	public Image multiplicadorBarra;
	private int mult = 1;
	public float resetMult = 0.5f;
	private float time = 0.5f;
	public static GameManager current;
	public List<Transform> spawnPoints;
	public GameObject spawnPref;
	//public List<Vector3> spawnPoints;
	public bool useRandomSeed;
	public string seed;
	private System.Random randomNumber;
	private int minNumberSpawns,maxNumberSpawns;
	public float tiempoNuevaRonda;
	private float tiempo;
	public int currentRound;
	private int enemigosRestantes;
	public int enemigosTotales;
	public int valorEnemigos;
	public int enemigosEliminados = 0;
	public List<int> tipoEnemigos;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {
		if (useRandomSeed)
			seed = Time.time.ToString ();
		minNumberSpawns = 0;
		maxNumberSpawns = 3;
		randomNumber = new System.Random (seed.GetHashCode());
		tipoEnemigos = new List<int>();

		newRound ();
	}

	void newRound(){
		currentRound++;
		enemigosEliminados = 0;
		ClearSpawnPoints ();
		minNumberSpawns = currentRound / 20;
		maxNumberSpawns = currentRound / 15 + 3;

		int newSpawns = randomNumber.Next (minNumberSpawns, maxNumberSpawns);

		for(int i = 0; i< newSpawns;i++)
			CreateNewSpawnPoints ();

		valorEnemigos += randomNumber.Next (1, 4);
		enemigosTotales += randomNumber.Next (0, 3);
		if (currentRound == 1) {
			for(int i = 0; i< enemigosTotales; i++){
				tipoEnemigos.Add(1);
			}
		}

		//Calcular que tipos de enemigos hay que spawnear en esta ronda, y ajustar el numero de enemigos si es necesario!
		CalcularEnemigos ();

		enemigosRestantes = enemigosTotales;
		InvokeRepeating ("CrearEnemigos", cd_enemigos, cd_enemigos);
	}

	void CalcularEnemigos(){
		if (tipoEnemigos.Count < enemigosTotales) {
			for(int i = tipoEnemigos.Count; i< enemigosTotales; i++){
				tipoEnemigos.Add(1);
			}
		}
		int sum = 0;
		foreach (int x in tipoEnemigos) {
			sum +=x;
		}
		if (sum == valorEnemigos) {
			return;
		} 
		else if (sum < valorEnemigos) {
			for(int i = sum; i < valorEnemigos; i++){
				int pos = randomNumber.Next (0, tipoEnemigos.Count);
				tipoEnemigos [pos] ++;
			}
		}
		else if (sum > valorEnemigos) {
			for(int i = sum; i > valorEnemigos; i--){
				int pos = randomNumber.Next (0, tipoEnemigos.Count);
				while(tipoEnemigos [pos] == 1)
					pos = randomNumber.Next (0, tipoEnemigos.Count);
				tipoEnemigos [pos] ++;
			}
		}
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

		if (enemigosEliminados == enemigosTotales) {
			newRound ();
		}


	}

	void CrearEnemigos(){
		if (enemigosRestantes == 0) {
			CancelInvoke();
			return;
		}
		GameObject enemy = Pool.current.Crear_Enemigo (tipoEnemigos[enemigosRestantes-1]);
		if (enemy == null)
			return;

		//0-3, posiciones referentes a la camara
		//4-7, posiciones esquinas del mapa
		//8+, posiciones random extra
		enemy.transform.position = spawnPoints[randomNumber.Next(0,spawnPoints.Count)].position;
		enemy.SetActive (true);
		enemigosRestantes--;
	}

	void ClearSpawnPoints(){
		for (int i = 8; i < spawnPoints.Count; i++)
			Destroy (spawnPoints [i].gameObject);

		spawnPoints.RemoveRange (8, spawnPoints.Count - 8);
	}

	void CreateNewSpawnPoints(){

		Vector2 pos = new Vector2(randomNumber.Next ((int)PlayerController.current.min.x,(int) PlayerController.current.max.x),
		                          randomNumber.Next ((int)PlayerController.current.min.y, (int) PlayerController.current.max.y) );
		GameObject spawn = (GameObject) Instantiate (spawnPref,pos,Quaternion.identity);
		//spawn.position = pos;


		spawnPoints.Add (spawn.transform);
	
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
