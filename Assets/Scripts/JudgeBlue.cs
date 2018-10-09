using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeBlue : MonoBehaviour {

	NoteObject note;

	//musictime取得用  必要？？？？
	NotesManagement manager;


	//Player_Pos取得
	PlayerManagement player;

	private float player_PosX;
	private float deltaPlayer_PosX; //使わないかも

	//HPバー制御
	private HP_BarCtrl hp;

	/*private int judgeStatus; // 0:何もしない 1:Miss 2:GOOD 3:PERFECT 4: GOOD 5:Miss

	private bool isJudged;

	private float t;

	void OnEnable(){
		judgeStatus = 0;
		t = 0;
		isJudged = false;
	}*/

	// Use this for initialization
	void Start () {
		note = GetComponent<NoteObject> ();
		manager = GameObject.Find ("NotesManagement").GetComponent<NotesManagement> ();
		player = GameObject.Find ("PlayerManagement").GetComponent<PlayerManagement>(); //戻すならPlayerPosition
		hp = GameObject.Find ("HP_BarCtrl").GetComponent<HP_BarCtrl>();

	}
	
	// Update is called once per frame
	void Update () {

		if ((Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.J)) && PlayerManagement.inputLimit) {
			OnNoteJudge (NotesManagement.timer, player.nowLine);
			PlayerManagement.inputLimit = false;
			return;
		} else if (note.timeDiff - NotesManagement.timer <= -NotesManagement.goodTime/2 && CheckBeforeNotes() ){
			Debug.Log ("akanyan");
			NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
			PlayerManagement.inputTrigger = true;
			NotesManagement.missCount++;
			manager.ResetCombo ();
			hp.ChangeHP (-0.05f);
			ObjectPool.instance.ReleaseGameObject (gameObject);
		}


		/*if (judgeStatus == 5 && !isJudged) {
			Debug.Log ("あほ");
			isJudged = true;
			NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
			NotesManagement.missCount++;
			manager.ResetCombo ();
			hp.ChangeHP (-0.05f);
			ObjectPool.instance.ReleaseGameObject(gameObject);
		} 
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.J)){
			//OnNoteJudge (NotesManagement.timer, player.nowLine);
			if (!isJudged) {
				OnNoteJudge (player.nowLine);
			}
		} */

	}
		

	//同一レーンに未判定のノーツが存在するか 
	private bool CheckBeforeNotes(){
		return note.noteIndex == NotesManagement.noteList [int.Parse(note.gameObject.tag) - 1] [0];
	}

	private void OnNoteJudge(float timer, int playerLine){  
		if (CheckBeforeNotes ()) {
			int deltaLine = player.nowLine;
			if (note.gameObject.tag == playerLine.ToString () || note.gameObject.tag == deltaLine.ToString ()) {
				if (Mathf.Abs (note.timeDiff - timer) <= NotesManagement.perfectTime / 2) {
					NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
					PlayerManagement.inputTrigger = true;
					NotesManagement.perfectCount++;
					manager.AddScore (NotesManagement.perfectScore);
					manager.AddCombo ();
					manager.ExplosionBlue (this.gameObject.tag);
					hp.ChangeHP (0.02000f);
					ObjectPool.instance.ReleaseGameObject (gameObject);
					return;
				} else if (Mathf.Abs (note.timeDiff - timer) <= NotesManagement.goodTime / 2) {
					NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
					PlayerManagement.inputTrigger = true;
					NotesManagement.goodCount++;
					manager.AddScore (NotesManagement.goodScore);
					manager.AddCombo ();
					manager.ExplosionBlue (this.gameObject.tag);
					hp.ChangeHP (0.01000f);
					ObjectPool.instance.ReleaseGameObject (gameObject);
					return;
				} else if (Mathf.Abs (note.timeDiff - timer) <= NotesManagement.badTime / 2) { 
					NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
					PlayerManagement.inputTrigger = true;
					NotesManagement.badCount++;
					manager.ResetCombo ();
					hp.ChangeHP (-0.05000f);
					ObjectPool.instance.ReleaseGameObject (gameObject);

					return;
				} else {
					//判定外なので何もしない//
					return;
				}
			}
		}
	}


}
