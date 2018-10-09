using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeSlide : MonoBehaviour { //missカウントがおかしい。なぜか増える


	NoteObject note;

	//musictime取得用  必要？？？？
	NotesManagement manager;

	//Player_Pos取得
	PlayerManagement player; //player.player_Pos.transform.localPosition.xがプレイヤーの位置



	//HPバー制御
	private HP_BarCtrl hp;

	//okCountとjudgeCount

	private int okCount;
	private int judgeCount;

	//最終判定済みか
	private bool isJudged;
	private bool isSE;



	private bool flag;
	void OnEnable(){
		okCount = 0;
		judgeCount = 0;
		isJudged = false;
		isSE = false;
		flag = false;

		Invoke ("StartJudge", NotesManagement.createNotes_PosY / (NotesManagement.laneLength * SelectManager.highSpeed) - NotesManagement.perfectTime/2 );  
	}

	// Use this for initialization
	void Start () {
		note = GetComponent<NoteObject> ();
		manager = GameObject.Find ("NotesManagement").GetComponent<NotesManagement> ();
		player = GameObject.Find ("PlayerManagement").GetComponent<PlayerManagement>(); //戻すならPlayerPosition
		hp = GameObject.Find ("HP_BarCtrl").GetComponent<HP_BarCtrl>();

	}
	
	// Update is called once per frame
	void Update () {
		/*if (!isSE && judgeCount == 3) {
			isSE = true;
			AudioManager.Instance.PlaySE ("SE_Slide");
		}

		if (judgeCount == 5 && okCount < 3) {
			isJudged = true;
			//NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
			NotesManagement.missCount++;
			manager.ResetCombo ();
			hp.ChangeHP (-0.05f);
			ObjectPool.instance.ReleaseGameObject(gameObject);
		}*/

		/*if (okCount >= 3 && !isJudged && judgeCount == 5) {
			isJudged = true;
			//NotesManagement.noteList [int.Parse (note.gameObject.tag) - 1].RemoveAt (0);
			manager.ExplosionSlide();
			NotesManagement.perfectCount++;
			manager.AddScore (NotesManagement.perfectScore);
			manager.AddCombo ();
			hp.ChangeHP (0.02000f);
			ObjectPool.instance.ReleaseGameObject (gameObject);
		}*/

		if (okCount >= 1 && !isJudged) {
			isJudged = true;
			manager.ExplosionSlide();
			AudioManager.Instance.PlaySE ("SE_Slide");
			NotesManagement.perfectCount++;
			manager.AddScore (NotesManagement.perfectScore);
			manager.AddCombo ();
			hp.ChangeHP (0.02000f);
			ObjectPool.instance.ReleaseGameObject (gameObject);
		} else if (note.timeDiff - NotesManagement.timer <= - NotesManagement.goodTime && !isJudged){
			isJudged = true;
			NotesManagement.missCount++;
			manager.ResetCombo ();
			hp.ChangeHP (-0.05f);
			ObjectPool.instance.ReleaseGameObject(gameObject);
		}

		if (flag) {
			OnNoteJudge (player.player_Pos.transform.localPosition.x,Input.GetAxis("Mouse X"));
		}
	}

	/*private bool OnNoteJudge(int playerNowLine, float mouseAxis_X){ //なんかおおきく思いっきりやったら判定されない
		bool success = false;
		if (judgeCount <= 1) {
			startPos [judgeCount] = player.transform.localPosition.x;
			if (note.kind % 2 == 0) {
				if (playerNowLine <= int.Parse (note.gameObject.tag)) {
					success = true;
				}
			} else if (note.kind % 2 != 0) {
				if (playerNowLine >= int.Parse (note.gameObject.tag)) {
					success = true;
				}
			}
		} else if (judgeCount == 2) {
			if (note.kind % 2 == 0) {
				if (mouseAxis_X >= 0.05f && (playerNowLine == int.Parse (note.gameObject.tag) || playerNowLine == int.Parse (note.gameObject.tag) + 1)) {
					success = true;
				}
			} else if (note.kind % 2 != 0 && (playerNowLine == int.Parse (note.gameObject.tag) || playerNowLine == int.Parse (note.gameObject.tag) - 1)) {
				if (mouseAxis_X <= -0.05f) {
					success = true;
				}
			}
		} else if (judgeCount >= 3) {
			endPos [judgeCount - 3] = player.transform.localPosition.x;
			if (note.kind % 2 == 0) {
				if (playerNowLine > int.Parse (note.gameObject.tag) || endPos [judgeCount - 3] - startPos [judgeCount - 3] >= 50) {
					success = true;
				}
			} else if (note.kind % 2 != 0) {
				if (playerNowLine < int.Parse (note.gameObject.tag) || endPos [judgeCount - 3] - startPos [judgeCount - 3] <= -50) {
					success = true;
				}
			}
		}
		return success == true;
	} */

	private void OnNoteJudge(float playerPosX, float mouseAxisX){
		if (Mathf.Abs (playerPosX - this.transform.localPosition.x) <= 150f) {
			//kindが偶数のとき、右に行くスライドなのでベクトルが”＋”ならよし.逆もまた然り
			if (Mathf.Sign (mouseAxisX) == Mathf.Pow (-1, note.kind % 2) && Mathf.Abs(mouseAxisX) >= 0.5f) { 
				okCount++;
				return;
			}
		}	
	}


	private void StartJudge(){
		
		/*if (OnNoteJudge (player.nowLine, Input.GetAxis ("Mouse X"))) {
			judgeCount++;
			okCount++;

		} else {
			
			judgeCount++;
		}*/
		flag = true;
	}

	private bool CheckBeforeNotes(){
		/*beforeNotes = GameObject.FindGameObjectsWithTag (this.gameObject.tag);
		int count = 0;
		foreach(GameObject note in beforeNotes){
			if (note.transform.localPosition.y < this.transform.localPosition.y && note.transform.localPosition.y >= -25f) {
				count++;
			}
		}
		return count == 0;*/
		return note.noteIndex == NotesManagement.noteList [int.Parse(note.gameObject.tag) - 1] [0];
	}
}
