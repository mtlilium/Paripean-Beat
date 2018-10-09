using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ListUpManager : MonoBehaviour {


	//MusicNodeプレハブ
	public GameObject musicNode;
	//MusicNodeをContentに並べるための親オブジェクトとobj
	private GameObject content;
	private GameObject obj;

	//曲情報
	public GameObject info_MusicName;
	public GameObject info_MusicNamePanel;
	public GameObject info_MusicArtist;
	public GameObject info_MusicBPM;
	public GameObject info_MusicLevel;

	//Musicの辞書
	public Dictionary<string,MusicInformation> musicsDic;

	//Gameシーンにわたす曲

	public static MusicInformation nextPlayMusic;

	// Use this for initialization
	void Start () {
		musicsDic = new Dictionary<string,MusicInformation> ();

		//曲数をjsonファイル数分読み込む
		DirectoryInfo dir = new DirectoryInfo(@"Assets\Resources\Sounds\Notes");
		FileInfo[] info = dir.GetFiles("*.json");

		//親参照させる
		content = GameObject.Find ("Content");

		//曲数分MusicNodeを生成する。
		foreach (FileInfo f in info) {
			string key = f.Name;

			obj = (GameObject)Instantiate (musicNode);
			MusicButtonOnClick button = obj.GetComponent<MusicButtonOnClick> ();
			obj.transform.SetParent(content.transform,false);

			LoadMusicFromJSON (key);
			Debug.Log (key);
			button.init (key, musicsDic [key].name, musicsDic [key].level); //initをElectの中に入れてしまいたい
			ElectMusic (0, key, obj); //第一引数はステージ
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMusicText(string key){
		MusicInformation music = musicsDic [key];
		info_MusicNamePanel.GetComponent<Text> ().text = music.name;
		info_MusicArtist.GetComponent<Text> ().text = music.artist;
		info_MusicBPM.GetComponent<Text> ().text = "BPM " + music.BPM.ToString();
	}
		

	public MusicInformation LoadMusicFromJSON(string filename){
		string json = File.ReadAllText(@"Assets\Resources\Sounds\Notes\" + filename);
		MusicInformation music = new MusicInformation();

		//複数譜面対応させるなら変更しないと
		Notes note = new Notes();
		JsonUtility.FromJsonOverwrite (json, note);

		/*MusicScore musicScore = new MusicScore ();
		JsonUtility.FromJsonOverwrite (json, musicScore); */

		JsonUtility.FromJsonOverwrite (json, music);
		musicsDic.Add (filename,music);

		return music;
	}

	public void SetNextMusic(string key){
		nextPlayMusic = musicsDic [key];
	}

	//ストーリーモード用 課題曲のみをActiveにする
	public void ElectMusic(int stage, string key, GameObject obj){
		if (stage == 0) {
			return;
		}
		if (musicsDic [key].stage != stage) {
			obj.SetActive (false);
		}
	}
		
}

[Serializable]
public class MusicInformation{
	public string name;
	public string artist;
	public int BPM;
	public int level;
	public int levelAnother;
	public int stage; //ストーリーモード用
	public Notes[] notes;
	public Notes[] notesAnother;
	public MusicScore[] musicScore;//Notes[] notes;

}
//これで複数譜面に対応できた
[Serializable]
public class MusicScore{
	public Notes[] notes;
	public int stage;
}

[Serializable]
public class Notes{
	public int LPB;
	public int num;
	public int block;
	public int type;
}

	
