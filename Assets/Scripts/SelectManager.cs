using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour {

	//ハイスピオプション
	public static float highSpeed;

	//譜面難易度　0:Normal 1:Another
	public static int difficulty;

	public GameObject highSpeedText;

	// Use this for initialization
	void Start () {
		highSpeed = 1.0f;
		difficulty = 0;
		//BGM流す
		AudioManager.Instance.PlayBGM ("BGM(Select)");

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (AudioManager.Instance.AttachBGMSource.isPlaying);
		if (AudioManager.Instance.AttachBGMSource.isPlaying == false) {
			AudioManager.Instance.PlayBGM ("BGM(Select)", 1.0f);
		}

		DisplayHighSpeed (highSpeed);
	}

	public void Push_SelectButton(){
		SceneManager.LoadScene ("PlayingScene"); 
	}

	public void Push_HighSpeedUpButton(){
		highSpeed += 0.25f;
		if (highSpeed >= 3.0f) {
			highSpeed = 3.0f;
		}
		AudioManager.Instance.PlaySE ("SELECT");
	}
	public void Push_HighSpeedDownButton(){
		highSpeed -= 0.25f;
		if (highSpeed <= 0.25f) {
			highSpeed = 0.25f;
		}
		AudioManager.Instance.PlaySE ("SELECT");
	}

	public void Push_ChangeDifficultyButton(){
		difficulty++;
		if (difficulty > 1) {
			difficulty = 0;
		}
		AudioManager.Instance.PlaySE ("CHANGE_DIFFICULTY");
		//レベルの表示を変える
		MusicButtonOnClick[] objs = GameObject.FindObjectsOfType<MusicButtonOnClick>();
		foreach (MusicButtonOnClick obj in objs) {
			obj.ChangeDifficulty ();
		}
	}
	public void DisplayHighSpeed(float speed){
		string num = ((float)speed).ToString ();
		highSpeedText.GetComponent<Text>().text = "×" + num;
	}
}
