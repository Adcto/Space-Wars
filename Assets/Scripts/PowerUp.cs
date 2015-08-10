using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
	
	public int maxHealth = 10;
	public int currentHealth;
	public float speed;
	public float rotationTime = 540;
	public int tipo = 0;
	public int maxTipos = 8;
	public Sprite[] sprites;
	private SpriteRenderer rend;

	void Awake(){
		rend = GetComponent<SpriteRenderer> ();
	}


	public  void OnEnable(){
		tipo = Random.Range (1, maxTipos+1);
		rend.sprite = sprites [tipo - 1];
		currentHealth = maxHealth;
	}
	// Update is called once per frame
	public  void FixedUpdate () {

			Move (NextPos ());
			Rotate ();

	}
	
	public void Move(Vector3 nextPosition){
		transform.position = Vector2.MoveTowards (transform.position,nextPosition ,speed* Time.deltaTime);
	}
	
	public  void Rotate(){
		Vector2 direction = PlayerController.current.transform.position - transform.position;
		direction = direction.normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, angle), rotationTime * Time.deltaTime);
	}
	
	public  Vector3 NextPos(){
		return (Vector2)PlayerController.current.transform.position;
	}
	
	public void QuitarVida(int vida){
		if (vida == -1)
			currentHealth = 0;
		else {
			currentHealth -= vida;
		}
		if (currentHealth <= 0) {
			GameObject go = Pool.current.Crear_Explosion_Enemigo(); //Por ahora usa la misma particula q los enemigos
			go.transform.position = transform.position;
			go.transform.rotation = transform.rotation;
			go.SetActive(true);
			
			gameObject.SetActive (false);
		}
	}
}
