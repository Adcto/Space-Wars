using UnityEngine;
using System.Collections;

public class Asteroide : EnemyController {
	public int tipo = 1;
	public int hijos = 1;
	public int maxDivisiones = 3;
	public Vector2 direction;
	public Vector2 impacto;
	public Vector2 rebote;
	private Rigidbody2D rig;

	public override void Awake ()
	{
		rig = GetComponent<Rigidbody2D> ();
		base.Awake ();
	}

	public override void OnEnable(){
		base.OnEnable ();
		direction = direction.normalized;
		currentHealth/=tipo;
		speed += tipo;
		score += 10 * tipo;
		transform.localScale = Vector3.one * 12/tipo;
		if (tipo != 1) {
			anim.Play ("Idle");
		}
	}

	// Update is called once per frame
	public override void FixedUpdate () {
		if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Spawn")) {
			rig.velocity = direction * speed;
			Rotate ();
		}
	}

	
	public override void Rotate(){
		transform.Rotate (0, 0, rotationTime * Time.fixedDeltaTime);
	}

	void OnDisable(){
		//crear cantidad segun tipo, dar direcciones, dar calidad
		if (tipo != maxDivisiones) {
			GameObject go;
			for(int i = 0; i < hijos; i++){
				go = Pool.current.Crear_Asteroide(tipo+1,calidad,direction+impacto);
				if(go != null){
					go.transform.position = transform.position;
					if(!go.activeInHierarchy)
						go.SetActive(true);
			//	nuevo.hijos = hijos-1;
				}
				direction*=-1;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Finish") {
			direction = Vector2.Reflect(direction,other.contacts[0].normal);
		}
	}
}
