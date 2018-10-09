using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeRed : MonoBehaviour {

	NoteObject note;

	//ホイールの方向　巻き込み防止
	private float lastWheelSign;

	//musictime取得用  必要？？？？
	NotesManagement manager;



	//同一レーンで自身より前にノーツがあるかチェック
	//GameObject[] beforeNotes;

	//Player_Pos取得
	PlayerManagement player;

	private float player_PosX;
	private float deltaPlayer_PosX;

	//HPバー制御
	private HP_BarCtrl hp;

	// Use this for initialization
	void Start () {
		note = GetComponent<NoteObject> ();
		manager = GameObject.Find ("NotesManagement").GetComponent<NotesManagement> ();
		player = GameObject.Find ("PlayerManagement").GetComponent<PlayerManagement>();
		hp = GameObject.Find ("HP_BarCtrl").GetComponent<HP_BarCtrl>();
	}
	
	// Update is called once per frame
	void Update () {
		//見逃し判定
		if (transform.localPosition.y < -60f) {
			NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);

			PlayerManagement.inputTrigger = true;

			NotesManagement.missCount++;
			manager.ResetCombo ();
			hp.ChangeHP (-0.05f);
			ObjectPool.instance.ReleaseGameObject(gameObject);
		}else if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 && PlayerManagement.inputLimit){
			OnNoteJudge (NotesManagement.timer, player.nowLine);
			PlayerManagement.inputLimit = false;
		}
	}

	//同一レーンに未判定のノーツが存在するか tagを使って判定 //課題：：そいつらがmiss判定なら別に判定していいよ　と付け加えたい
	private bool CheckBeforeNotes(){
		return note.noteIndex == NotesManagement.noteList [int.Parse(note.gameObject.tag) - 1] [0];
	}

	private void OnNoteJudge(float timer, int playerLine){

		if (CheckBeforeNotes ()) {
			int deltaLine = player.nowLine;
			if (note.gameObject.tag ==  playerLine.ToString() || note.gameObject.tag == deltaLine.ToString()) {
				if (Mathf.Abs (note.timeDiff - timer) <= NotesManagement.perfectTime / 2 ) {
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
					Debug.Log ("badだよ");
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
