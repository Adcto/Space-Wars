using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	public static PlayerController current;
	public Transform posicionDisparo;
	//public Joystick movement;
	//public Joystick aim;
	public float maxHealth = 10;
	public float currentHealth;
	public bool puedeDisparar = false;
	public float cadenciaDisparo;
	//public Vector3 max,min;
	//skill1,skill2
	public float angle;
	public float speed = 2f;
	private Vector2 shootDirection;
	private Rigidbody2D rig;
	private float cooldownDisparo = 0;
	[HideInInspector]
	public Collider2D collider;
	public Vector2 direction;
	public ParticleSystem rastro;
	public Vector2 aim;
	[HideInInspector]
	public bool apuntando = false;

	// Use this for initialization
	void Awake(){
		current = this;

	}

	void Start () {
//		cadenciaDisparo = Pool.current.cadenciaDisparo; //Habra q actualizarla cuando se cambie de disparo
		currentHealth = maxHealth;
		rig = GetComponent<Rigidbody2D> ();
		collider = GetComponent<Collider2D> ();


	}

	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		if (puedeDisparar) {
			if(apuntando){					//Solo dispara cuando pulsas el joystick derecho!
				cooldownDisparo += Time.deltaTime;
				if(cooldownDisparo >= cadenciaDisparo){
					cooldownDisparo = 0;
					Disparar ();
				}
			}
		}
	}

	void Move(){
		//direction = movement.GetDirection ();
		//angle = aim.GetAngle ();
		direction = new Vector2(CrossPlatformInputManager.GetAxis("HorizontalMovement"), CrossPlatformInputManager.GetAxis("VerticalMovement"));
		aim = new Vector2(CrossPlatformInputManager.GetAxis("HorizontalAim"), CrossPlatformInputManager.GetAxis("VerticalAim"));
		aim = aim.normalized;
		if (aim != Vector2.zero) {
			angle = Mathf.Atan2 (aim.y, aim.x) * Mathf.Rad2Deg - 90;
			apuntando = true;
		}
		else 
			apuntando = false;
		rig.velocity = direction * speed;
		if (rig.velocity == Vector2.zero && rastro.isPlaying) 
			rastro.Stop ();
		else if (rig.velocity != Vector2.zero && rastro.isStopped)
			rastro.Play ();

		//float x = Mathf.Clamp (transform.position.x, min.x, max.x);
		//float y = Mathf.Clamp (transform.position.y, min.y,max.y);
		//transform.position = new Vector2 (x, y);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, angle), 1080 * Time.deltaTime); // Giro con delay
		//rig.MoveRotation(angle); //Giro Instantaneo
	}

	void Disparar(){
		GameObject shoot = Pool.current.Disparar ();
		shoot.transform.position = posicionDisparo.position;
		shoot.transform.rotation = transform.rotation;
//		float desviacion = Random.Range (-5f, 5f);
//
//
//		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90+desviacion)) *  new Vector3(1,0.0f,0.0f);
//		//float shootAngle = shoot.transform.rotation.eulerAngles.z;
//		shoot.GetComponent<Disparo> ().direction = (Vector2)dir.normalized;
		//shoot.GetComponent<Disparo> ().direction = new Vector2 (Mathf.Cos (shootAngle), Mathf.Sin (shootAngle));
//		curvo *= -1;
//		if (transform.childCount > 0) {
//			DisparoCurvo[] disps = shoot.GetComponentsInChildren<DisparoCurvo> ();
				for (int i = 0; i < shoot.transform.childCount; i++) {
//				if( disps[i] != null){
//					disps[i].sentido = curvo;
//				}
				shoot.transform.GetChild(i).gameObject.layer = gameObject.layer;
			}
//		} else {
//			DisparoCurvo disp = shoot.GetComponent<DisparoCurvo>();
//			if( disp != null){
//				disp.sentido = curvo;
//			}
//		}


		shoot.layer = gameObject.layer;
		shoot.SetActive (true);
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy" ||other.gameObject.tag == "Asteroide" ) {
			currentHealth--;
			//transform.position = Vector2.zero;

			other.gameObject.GetComponent<EnemyController>().QuitarVida(-1);
		}
		if (other.gameObject.tag == "PowerUp") {
			PowerUp p = other.gameObject.GetComponent<PowerUp>();
			int powerUp = p.tipo;
			p.QuitarVida(-1);
		}
	}
}
