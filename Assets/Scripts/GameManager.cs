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
	public int maxEnemigosSimultaneos = 5;
	public int valorEnemigos;
	public int enemigosEliminados = 0;
	public List<int> tipoEnemigos;
	public List<int> enemigosSpawneados;
	public GameObject Tienda;
	public Vector2 max, min;
	private int spawnAgujero = 4;
	private int puntuacionMaxima = 10;
	private int puntAcumulada = 0;


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

	public void newRound(){
		currentRound++;
		if (currentRound % 3 == 0) {
			maxEnemigosSimultaneos++;
		}

		ClearSpawnPoints ();
		minNumberSpawns = currentRound / 20;
		maxNumberSpawns = currentRound / 15 + 3;

		int newSpawns = randomNumber.Next (minNumberSpawns, maxNumberSpawns);

		for(int i = 0; i< newSpawns;i++)
			CreateNewSpawnPoints ();

		valorEnemigos += randomNumber.Next (2, 5) * valorEnemigos/enemigosTotales;
		enemigosTotales += randomNumber.Next (0, 2);
		if (currentRound == 1) {
			for(int i = 0; i< enemigosTotales; i++){
				enemigosSpawneados.Add(1);
			}
		}

		//Calcular que tipos de enemigos hay que spawnear en esta ronda, y ajustar el numero de enemigos si es necesario!
		CalcularEnemigos ();

		enemigosRestantes = enemigosTotales;
		cd_enemigos =  0.2f + 2 / ((float)currentRound +1); // yo k se teteeee
		InvokeRepeating ("CrearEnemigos", cd_enemigos, cd_enemigos);
	}

	void CalcularEnemigos(){
		//Quizas habria que cambiarlo de modo que valorEnemigo/enemigosTotales, y hacer que el entero resultante sea el valor 
		//del mayor numero de enemigos esa ronda, para asi diferenciar mas las rondas entre si y las oleadas

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

	void CrearEnemigos(){
		if ( tipoEnemigos.Count == 0) {
			CancelInvoke("CrearEnemigos");
			return;
		}

		//No sobrecargar la pantalla de enemigos!
		if (enemigosSpawneados.Count - enemigosEliminados >= maxEnemigosSimultaneos)
			return;


		int pos = randomNumber.Next (0, tipoEnemigos.Count);
		int spawnPos = randomNumber.Next (0, spawnPoints.Count);

		if (tipoEnemigos [pos] % Pool.current.tiposEnemigos == 0) {

			GameObject agujero = Pool.current.Crear_Enemigo (tipoEnemigos[pos]);
			if(agujero != null){
				enemigosSpawneados.Add (tipoEnemigos [pos]);
				tipoEnemigos.RemoveAt (pos);
				agujero.transform.position = spawnPoints[spawnAgujero].position;
				agujero.SetActive (true);
				enemigosRestantes--;
				spawnAgujero++;
				if(spawnAgujero == 8)
					spawnAgujero = 4;
			}
			return;
			
		}



		if (enemigosRestantes >= 5 && tipoEnemigos.Count - pos >=5 && currentRound > 5) {						//Prueba con oleadas fijas, de 5 naves
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
		if (enemy == null)
			return;
		enemigosSpawneados.Add (tipoEnemigos [pos]);
		tipoEnemigos.RemoveAt (pos);


		if (enemigosSpawneados[enemigosSpawneados.Count-1] % Pool.current.tiposEnemigos == 5) {

			
			Vector2 dir ;
			Vector2 posInicial;

			if (randomNumber.Next (0, 2) == 1) {
				dir = Vector2.up;
				posInicial = new Vector2 (PlayerController.current.transform.position.x , min.y -1);
			} else {
				dir = Vector2.right;
				posInicial = new Vector2 (min.x-1, PlayerController.current.transform.position.y);
			}
			enemy.transform.position =  posInicial;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90;
			enemy.transform.rotation = Quaternion.Euler(0,0,angle) ;
			enemy.SetActive (true);
			return;
		}

		//0-3, posiciones referentes a la camara
		//4-7, posiciones esquinas del mapa
		//8+, posiciones random extra


		while (spawnPos < 4 && !ComprobarSpawn(spawnPos)) {			//Si la posicion de spawn es una de las que depende de la camara, esta podria salirse del mapa
			spawnPos = randomNumber.Next (0, spawnPoints.Count);
		}

		enemy.transform.position = spawnPoints[spawnPos].position;
		enemy.SetActive (true);
		enemigosRestantes--;


		//Crear uno cada cierto tiempo, y solo uno al mismo tiempo
		//Quizas cada x puntos crear un powerup??
		if (randomNumber.Next (0, 100) == 50) {
			GameObject powerup = Pool.current.Crear_PowerUp ();
			if(powerup != null){
				spawnPos = randomNumber.Next (0, spawnPoints.Count);
				
				while (spawnPos < 4 && !ComprobarSpawn(spawnPos)) {
					spawnPos = randomNumber.Next (0, spawnPoints.Count);
				}
				
				powerup.transform.position = spawnPoints [spawnPos].position;
				PowerUp pu = powerup.GetComponent<PowerUp>();
				pu.tipo = randomNumber.Next(1,pu.maxTipos);
				powerup.SetActive(true);
			}
		}
		

	}

	void SpawnearOleada(int pos){
		enemigosRestantes -= 5;
		
		Vector3 nextPos = Vector3.zero;
		if (tipoEnemigos [pos] % Pool.current.tiposEnemigos == 5) {



			Vector2 dir;
			Vector2 posInicial;

			if(randomNumber.Next(0,1) == 1){
				dir = Vector2.up;
				nextPos = Vector2.right;
				posInicial = new Vector2(PlayerController.current.transform.position.x -2, min.y-1);
			}
			else {
				dir = Vector2.right;
				nextPos = Vector2.up;
				posInicial = new Vector2( min.x-1,PlayerController.current.transform.position.y -2);
			}



			for (int i = 0; i < 5; i++) {
				GameObject enemy = Pool.current.Crear_Enemigo (tipoEnemigos [pos]);	//da igual incrementarlo, son el mismo
				enemy.transform.position =  (Vector3)posInicial + (nextPos * i);
				enemy.GetComponent<EnemyRecto>().direction = dir;
				enemy.SetActive (true);
				
			}
		}
		else {
			int spawnPos = randomNumber.Next (0, spawnPoints.Count);
			
			while (spawnPos < 4 && !ComprobarSpawn(spawnPos)) {
				spawnPos = randomNumber.Next (0, spawnPoints.Count);
			}
			for (int i = 0; i < 5; i++) {
				GameObject enemy = Pool.current.Crear_Enemigo (tipoEnemigos [pos]);	//da igual incrementarlo, son el mismo
				nextPos *= -1;
				if (i == 1)
					nextPos = Vector3.right;
				else if (i == 3)
					nextPos = Vector3.up;
				
				enemy.transform.position = spawnPoints [spawnPos].position + nextPos;
				enemy.SetActive (true);
				
			}
		}
	}

	bool ComprobarSpawn(int i){
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

		Vector2 pos = new Vector2(randomNumber.Next ((int)min.x,(int) max.x),randomNumber.Next ((int)min.y, (int) max.y) );
		GameObject spawn = (GameObject) Instantiate (spawnPref,pos,Quaternion.identity);
		//spawn.position = pos;


		spawnPoints.Add (spawn.transform);
	
	}

	// Update is called once per frame
	void Update () {
		
		if (mult > 1) {
			if(time > 0){
				time -= Time.deltaTime;
				puntAcumulada -=(int)Time.deltaTime*10;
				multiplicadorBarra.fillAmount = time/resetMult;
			}
			else {
				mult = 1;
				multiplicadorText.text = "x" + mult.ToString();
				multiplicadorBarra.fillAmount = 0;
				puntAcumulada = 0;
				puntuacionMaxima =10;
			}
		}
		
		if ( enemigosEliminados == enemigosTotales ) {
			enemigosEliminados = 0;
			Tienda.SetActive(true);
			Invoke("newRound", 5);
		}
	}
	public void AddScore(int punt){
		punt *= mult;
		//Para mejorar el sistema de multiplicador, solo habria que sumar algo a tiempo, y cuando tiempo >=resetmult -> mult++
		//Quizas x punts son = cierta cantidad de tiempo? hay que pensarlo -> Igual que Slayin
		puntAcumulada += punt;
		time =resetMult* (float)puntAcumulada / (float)puntuacionMaxima;

		if (time >= resetMult) {
			puntAcumulada = 0;
			puntuacionMaxima+=50;
			mult++;
			time = resetMult;
		}
		multiplicadorText.text = "x" + mult.ToString();
		string[] split = score.text.Split(':');
		punt += int.Parse (split [1]);
		score.text = "Score: " + punt.ToString();
	}
}
