using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int healthBase = 20;
	public int maxHealth;
	public int currentHealth;
	public float baseSpeed;
	protected float speed = 0;
	public float rotationTime = 540;
	public int baseScore = 10;
	protected int score = 0;
	public int calidad = 1;
	protected Animator anim;
	protected bool hit = false;

	// Use this for initialization
	public virtual void Start () {
		anim = GetComponent<Animator> ();

	}

	public virtual void OnEnable(){
		hit = false;
		maxHealth = healthBase * calidad;
		currentHealth = maxHealth;
		speed = baseSpeed+calidad*0.5f;
		score = baseScore * calidad;
	}
	// Update is called once per frame
	public virtual void FixedUpdate () {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn")) {
			Move (NextPos ());
			Rotate ();
		}
	}

	public void Move(Vector3 nextPosition){
		transform.position = Vector2.MoveTowards (transform.position,nextPosition ,speed* Time.deltaTime);
	}

	public virtual void Rotate(){
		Vector2 direction = PlayerController.current.transform.position - transform.position;
		direction = direction.normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, 0, angle), rotationTime * Time.deltaTime);
	}

	public virtual Vector3 NextPos(){
		return (Vector2)PlayerController.current.transform.position;
	}

	public void QuitarVida(int vida){
		if (vida == -1)
			currentHealth = 0;
		else {
			currentHealth -= vida;
			hit = true;
		}
		if (currentHealth <= 0) {
			GameManager.current.AddScore(score);
			GameManager.current.enemigosEliminados++;
			GameObject go = Pool.current.Crear_Explosion_Enemigo();
			go.transform.position = transform.position;
			go.transform.rotation = transform.rotation;
			go.SetActive(true);

			gameObject.SetActive (false);
		}
	}


	

}
