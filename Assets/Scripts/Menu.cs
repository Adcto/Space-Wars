using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	private Animator  anim;
	private CanvasGroup canvas;

	public bool IsOpen{
		get { return anim.GetBool("IsOpen");}
		set { anim.SetBool("IsOpen", value);}
	}
	

	public void Awake(){
		anim = GetComponent<Animator> ();
		canvas = GetComponent<CanvasGroup> ();
		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);

	}

	public void Update(){
		if (!anim.GetBool ("IsOpen")) {
			canvas.blocksRaycasts = canvas.interactable = false;
		}
		else 
			canvas.blocksRaycasts = canvas.interactable = true;
	}
}
