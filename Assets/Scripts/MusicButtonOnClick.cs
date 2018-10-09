using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class MusicButtonOnClick : MonoBehaviour {

	private ListUpManager manager;

	Button musicButton;

	GameObject informationPanel;

	private string key,name;
	private int level;

	private int selfDifficulty; //指しているのはNormalかAnotherか

	public bool previewFlag;

	// Use this for initialization
	void Start () {
		previewFlag = false;
		manager = GameObject.Find("ListUpManager").GetComponent<ListUpManager> ();
		informationPanel = GameObject.Find ("InformationPanel");
		informationPanel.GetComponent<CanvasGroup> ().alpha = 0;
		musicButton = GetComponent<Button> ();
		musicButton.onClick.AddListener (() => {
				UpdateInformation();
			});
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void init(string _Key, string Name, int Level){
		key = _Key;
		this.transform.FindChild ("TextMusicTitle").GetComponent<Text> ().text = Name;
		this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().color = new Color (127 / 255, 255 / 255, 212 / 255);
		this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().text = Level.ToString ();
	}

	private void UpdateInformation(){
		informationPanel.GetComponent<CanvasGroup> ().alpha = 1;
		manager.SetMusicText (key);
		PreviewMusic (this.transform.FindChild ("TextMusicTitle").GetComponent<Text> ().text);
	}

	private void PreviewMusic(string BGM){
		/*if (AudioManager.Instance.AttachBGMSource.isPlaying == true) {
			AudioManager.Instance.FadeOutBGM (0.8f);
		}*/
		previewFlag = true;

		AudioManager.Instance.PlayBGM (BGM);

		AudioManager.Instance.PlaySE ("SELECT(Music)");
	}

	//難易度変更
	public void ChangeDifficulty(){
		MusicInformation music = manager.musicsDic [key];
		if (SelectManager.difficulty == 1) {
			if (music.levelAnother != 0) { //Another譜面が存在すれば
				this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().color = new Color (255 / 255, 48 / 255, 48 / 255); //Another譜面の色
				this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().text = music.levelAnother.ToString ();
			}
		} else {
			
			this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().color = new Color (127 / 255, 255 / 255, 212 / 255); //Normal譜面の色
			this.transform.FindChild ("Level").FindChild ("TextLevel").GetComponent<Text> ().text = music.level.ToString ();
		}

	}
}
