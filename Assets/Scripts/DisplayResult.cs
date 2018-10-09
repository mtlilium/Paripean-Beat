using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayResult : MonoBehaviour {

	//Rank
	public GameObject rankText;
	//クリアかどうか
	public GameObject clearText;
	//score
	public GameObject scoreText;
	//ランキング
	public GameObject rankingText;


	private  string[] rank = {"SSS","SS","S","AAA","AA","A","B","C","D","E","F"};
	private  int[] rankBorder = { 990000, 975000, 950000, 900000, 850000, 800000, 750000, 700000, 650000, 600000, 0 };

	private string RANKING_PREF_KEY; 

	private int[] ranking = new int[5];//5位まで保存


	// Use this for initialization
	void OnEnable () {
		DisplayRank ();

		DisplayScore ();

		RANKING_PREF_KEY = ListUpManager.nextPlayMusic.name + SelectManager.difficulty.ToString(); //(曲名)+(譜面の種類 0 or 1)

		GetRanking();

		SaveRanking (NotesManagement.score);

		DisplayRanking ();
		
	}

	// Update is called once per frame
	void Update () {
		if (transform.localPosition.y > -50) {
			Vector3 tmp = transform.localPosition;
			tmp.y -= 800f;
			if (tmp.y <= -50) {
				tmp.y = -50;
			}
			transform.localPosition = Vector3.Lerp(transform.localPosition, tmp, Time.deltaTime * 2);
		} 

		//とりあえずF1キーでランキング削除
		if (Input.GetKeyDown (KeyCode.F1)) {
			PlayerPrefs.DeleteKey (RANKING_PREF_KEY);
		}
	}

	private void DisplayRank (){
		if (NotesManagement.clearStatus) {
			clearText.GetComponent<Text> ().text = "Clear";
		} else {
			clearText.GetComponent<Text> ().text = "Failed";
		}
		for (int i = 0; i < rankBorder.Length; i++) {
			if (rankBorder [i] <= NotesManagement.score) {
				rankText.GetComponent<TextMeshProUGUI> ().SetText(rank[i]);
				return;
			}
		}
	}

	private void DisplayScore(){
		scoreText.GetComponent<Text> ().text = "Score\n" + NotesManagement.score.ToString () + "\n\n" + "Perfect\n" + NotesManagement.perfectCount.ToString () + "\n" + "Good\n" + NotesManagement.goodCount.ToString () + "\n" + "Bad\n" + NotesManagement.badCount.ToString () + "\n" + "Miss\n" + NotesManagement.missCount.ToString ();
	}

	//ランキングの取得
	private void GetRanking(){
		var _ranking = PlayerPrefs.GetString (RANKING_PREF_KEY);
		if (_ranking.Length > 0) {
			var _score = _ranking.Split ("," [0]);
			ranking = new int[5];
			for (var i = 0; i < _score.Length && i < 5; i++) {
				ranking [i] = int.Parse (_score [i]);
			}
		}
	}

	//新たにランキングをセーブ
	private void SaveRanking(int newScore){
		if (ranking.Length > 0) {
			int tmp = 0;
			for (var i = 0; i < ranking.Length; i++) {
				if (ranking [i] < newScore) {
					tmp = ranking [i];
					ranking [i] = newScore;
					newScore = tmp;
				}
			}
		} else {
			ranking [0] = newScore;
		}

		//配列を文字列に変換してPlayerPrefsに格納
		string rankingString = ranking[0].ToString() + "," + ranking[1].ToString() + "," + ranking[2].ToString() + "," + ranking[3].ToString() + "," + ranking[4].ToString();
		PlayerPrefs.SetString (RANKING_PREF_KEY, rankingString);
	}

	private void DisplayRanking(){
		string rankingString = "";
		for (var i = 0; i < ranking.Length; i++) {
			rankingString = rankingString + (i + 1) + "位  " + ranking[i].ToString() + "\n";
		}
		rankingText.GetComponent<Text> ().text = rankingString;
	}
}
