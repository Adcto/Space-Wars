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
	public List<int> enemigosSpawneados;
	public GameObject Tienda;

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
		enemigosSpawneados = new List<int>();

		newRound ();
	}

	void newRound(){
		currentRound++;
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
		tipoEnemigos.Clear ();
		tipoEnemigos.AddRange(enemigosSpawneados);
		enemigosSpawneados.Clear ();
		if (tipoEnemigos.Count < enemigosTotales) {
			for(int i = tipoEnemigos.Count; i< enemigosTotales; i++){
				tipoEnemigos.Add(1);
			}
		}
		int sum = 0;
		foreach (int x in tipoEnemigos) {
			sum +=x;
		}
		if (sum < valorEnemigos) {
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
				tipoEnemigos [pos] --;
			}
		}
		tipoEnemigos.Sort ();
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

		if ( enemigosEliminados == enemigosTotales ) {
			enemigosEliminados = 0;
			Tienda.SetActive(true);
			Invoke("newRound", 5);
			//newRound ();
		}
	}

	void CrearEnemigos(){
		if ( tipoEnemigos.Count == 0) {
			CancelInvoke();
			return;
		}
		int pos = randomNumber.Next (0, tipoEnemigos.Count);
		if (enemigosRestantes >= 5 && tipoEnemigos.Count - pos >=5) {						//Prueba con oleadas fijas, de 5 naves
			bool spawnOleada = true;
			for (int i = 1; i < 5 && spawnOleada; i++) {	//Comprobar si los siguientes enemigos son del mismo tipo que el actual
				if (tipoEnemigos [pos] != tipoEnemigos [pos + i])
					spawnOleada = false;
			}
			if (spawnOleada) {
				SpawnearOleada(pos);
				for (int i = 0; i < 5 ; i++)
					enemigosSpawneados.Add(tipoEnemigos [pos]);
				tipoEnemigos.RemoveRange(pos,5);
				return;
			}
		}

		GameObject enemy = Pool.current.Crear_Enemigo (tipoEnemigos[pos]);
		enemigosSpawneados.Add (tipoEnemigos [pos]);
		tipoEnemigos.RemoveAt (pos);
		if (enemy == null)
			return;

		//0-3, posiciones referentes a la camara
		//4-7, posiciones esquinas del mapa
		//8+, posiciones random extra
		int spawnPos = randomNumber.Next (0, spawnPoints.Count);

		while (spawnPos < 4 && !ComprobarSpawn(spawnPos)) {			//Si la posicion de spawn es una de las que depende de la camara, esta podria salirse del mapa
			spawnPos = randomNumber.Next (0, spawnPoints.Count);
		}

		enemy.transform.position = spawnPoints[spawnPos].position;
		enemy.SetActive (true);
		enemigosRestantes--;
	}

	void SpawnearOleada(int pos){
		enemigosRestantes -= 5;
		int spawnPos = randomNumber.Next (0, spawnPoints.Count);

		while (spawnPos < 4 && !ComprobarSpawn(spawnPos)) {
			spawnPos = randomNumber.Next (0, spawnPoints.Count);
		}
		
		Vector3 nextPos = Vector3.zero;
		for (int i = 0; i < 5; i++){
			GameObject enemy = Pool.current.Crear_Enemigo (tipoEnemigos[pos]);	//da igual incrementarlo, son el mismo
			nextPos*=-1;
			if(i == 1)
				nextPos = Vector3.right;
			else if(i==3)
				nextPos = Vector3.up;
			enemy.transform.position = spawnPoints[spawnPos].position + nextPos;
			enemy.SetActive (true);
			//yield return new WaitForSeconds(0.1f);
		}
	}

	bool ComprobarSpawn(int i){
		Vector2 max = PlayerController.current.max;
		Vector2 min = PlayerController.current.min;
		Vector2 comprobarPos = spawnPoints [i].position;
		if (comprobarPos.x >= min.x && comprobarPos.y >= min.y && comprobarPos.x <= max.x && comprobarPos.y <= max.y) {
			return true;
		}
		return false;
	
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
