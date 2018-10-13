using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TitleManager : MonoBehaviour {

	public VideoPlayer mPlayer;

	private bool toLoadFlag;

	//　非同期動作で使用するAsyncOperation
	private AsyncOperation async;
	//　シーンロード中に表示するUI画面
	public GameObject loadUI;
	//　読み込み率を表示するスライダー
	public Slider slider;

	// Use this for initialization
	void Start () {
		toLoadFlag = false;
		AudioManager.Instance.PlayBGM("TitleMovie");
	}
	
	// Update is called once per frame
	void Update () {
		if (AudioManager.Instance.AttachBGMSource.isPlaying == false && toLoadFlag == false) {
			AudioManager.Instance.PlayBGM ("TitleMovie");
		}
	}

	public void NextScene() {
		//Titleのムービー停止
		toLoadFlag = true;
		AudioManager.Instance.AttachBGMSource.Stop ();
		AudioManager.Instance.PlaySE ("TitleCall");
		mPlayer.gameObject.SetActive (false);

		//　ロード画面UIをアクティブにする
		loadUI.SetActive(true);

		//　コルーチンを開始
		StartCoroutine(LoadData(2.0f));

	}

	IEnumerator LoadData(float delay) {
		yield return null;
		yield return new WaitForSeconds (delay);
		// シーンの読み込みをする
		async = SceneManager.LoadSceneAsync("SelectScene");

		//　読み込みが終わるまで進捗状況をスライダーの値に反映させる
		while(!async.isDone) {
			var progressVal = Mathf.Clamp01(async.progress / 0.9f);
			slider.value = progressVal;
			yield return null;
		}
	}
}
