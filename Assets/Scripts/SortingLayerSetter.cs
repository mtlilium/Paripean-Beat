using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SortingLayerSetter : MonoBehaviour {

	public string _sortingLayerName;
	public int sortingOrder;

	// Use this for initialization
	void Start () {
		CanvasRenderer renderer = gameObject.GetComponent<CanvasRenderer> ();
		if (renderer == null) {
			return;
		}
		//renderer.sortingLayerName = _sortingLayerName;
		//renderer.sortingOrder = 4;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
