using UnityEngine;
using System.Collections;

public class Disparo : MonoBehaviour {

	public int invertido =1;
	public float speed;
	public float cadencia = 0.08f;
	public int damage = 10;
	public Vector2 direction;
	public float dispersion;
	public DesactivarEmpty padre;
	protected Vector3 startPos;
	protected Quaternion startRot;
	protected float desviacion;
	private Rigidbody2D rig;
	public float TimeToDestroy = 1;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();


	}

	public virtual void OnEnable(){
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		Invoke ("Desactivate", TimeToDestroy);
		desviacion = 0;
		if (dispersion >0) {
			desviacion = Random.Range (-dispersion, dispersion);
		}
		Vector3 dir = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,90+desviacion)) *  new Vector3(invertido,0.0f,0.0f);
		direction = (Vector2) dir.normalized;
	}

	public void Desactivate(){
		CancelInvoke ();
		if (gameObject.activeInHierarchy) {
			if (padre != null) {
				padre.Desactivar ();
				transform.localPosition = startPos;
				transform.localRotation = startRot;
			}
			gameObject.SetActive (false);
		}
	}

	// Update is called once per frame
	public virtual void FixedUpdate(){
		rig.velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Finish" || other.gameObject.tag == "PowerUp") {
			GameObject go = Pool.current.Crear_Hit_Disparo();
			go.transform.position = transform.position;
			go.transform.rotation = transform.rotation;
			go.SetActive(true);
			if(other.gameObject.tag == "Enemy"){
				if(other.GetType() == typeof(Asteroide)){
					other.gameObject.GetComponent<Asteroide>().impacto = direction;
				}
				other.gameObject.GetComponent<EnemyController>().QuitarVida(damage);
			}
			else if(other.gameObject.tag == "PowerUp")
				other.gameObject.GetComponent<PowerUp>().QuitarVida(damage);
			
			Desactivate();
		}
	}

	void OnBecameInvisible(){
		Desactivate ();
	}

	void OnDisable(){
		CancelInvoke ();
	}
	
}
