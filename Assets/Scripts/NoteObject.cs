using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour {
	
	//曲が始まって何秒後に判定ラインに来るか
	public float timeDiff;

	//Noteの種類
	public int kind;

	//NoteのIndex
	public int noteIndex;

	/*private float startTime;
	private Vector3 startPosition;

	void OnEnable(){
		startTime = Time.timeSinceLevelLoad;
		startPosition = this.transform.localPosition;
	}*/

	// Use this for initialization
	void Start () {

		//this.transform.localPosition = Vector3.Lerp (startPos, endPos, Mathf.Clamp01 (timeDiff));
	}

	// Update is called once per frame
	void Update () {

		DrawNote (NotesManagement.timer); 


	}

	//ノーツの再描画
	private void DrawNote(float timer){
		//y座標しか変わらないので
		Vector3 tmp = this.transform.localPosition;
		tmp.y = (timeDiff - timer) * NotesManagement.laneLength * SelectManager.highSpeed;
		this.transform.localPosition = tmp;

		/*var diff = Time.timeSinceLevelLoad - startTime;
		var rate = diff / ((NotesManagement.createNotes_PosY + 100) / (NotesManagement.laneLength * SelectManager.highSpeed));
		this.transform.localPosition = Vector3.Lerp (startPosition, new Vector3 (this.transform.localPosition.x,-100, this.transform.localPosition.z), rate);*/

	}

	//ノートの初期化　種類は何？　何秒後にちょうどラインに落ちてくるの？   Slideは判定仕様が違うのでリストに加えない
	public void NoteInit(int _kind, float _timeDiff, int _index){
		if (_kind == 0 || _kind == 3 || _kind == 6) {
			this.gameObject.tag = "1";
			if (_kind != 6) {
				NotesManagement.noteList [0].Add (_index);
			}
		} else if (_kind == 1 || _kind == 4 || _kind == 7 || _kind == 8) {
			this.gameObject.tag = "2";
			if (_kind < 7) {
				NotesManagement.noteList [1].Add (_index);
			}
		} else {
			this.gameObject.tag = "3";
			if (_kind != 9) {
				NotesManagement.noteList [2].Add (_index);
			}
		}
		this.noteIndex = _index;
		this.kind = _kind;
		this.timeDiff = _timeDiff;
	}
}
