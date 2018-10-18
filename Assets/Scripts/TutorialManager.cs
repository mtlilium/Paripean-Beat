using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	public Sprite[] SlidePicture = new Sprite[6];
	private int nowNo;

	public Button backSelect;
	public Button goTutorial;
	private bool key; //上２つのボタンをactiveにしたらfalseになる.

	ListUpManager musicPlay;
	SelectManager selectManager;

	void OnEnable(){
		this.gameObject.GetComponent<Image> ().sprite = SlidePicture [0];
		nowNo = 0;
		backSelect.gameObject.SetActive (false);
		goTutorial.gameObject.SetActive (false);
		key = true;

		musicPlay = GameObject.Find ("ListUpManager").GetComponent<ListUpManager>();
		selectManager = GameObject.Find ("SelectManager").GetComponent<SelectManager>();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (nowNo == 5 && key) {
			backSelect.gameObject.SetActive (true);
			goTutorial.gameObject.SetActive (true);
			key = false;

		} else if(nowNo != 5 && !key){
			backSelect.gameObject.SetActive (false);
			goTutorial.gameObject.SetActive (false);
			key = true;
		}
	}

	public void PushNextButton(){
		AudioManager.Instance.PlaySE ("SELECT");
		nowNo++;
		if (nowNo >= 5) {
			nowNo = 5;
		}
		this.gameObject.GetComponent<Image> ().sprite = SlidePicture [nowNo];
	}

	public void PushBackButton(){
		AudioManager.Instance.PlaySE ("SELECT");
		nowNo--;
		if (nowNo <= 0) {
			nowNo = 0;
		}
		this.gameObject.GetComponent<Image> ().sprite = SlidePicture [nowNo];
	}


	public void PushBackSelect(){
		AudioManager.Instance.PlaySE ("SELECT");

		this.gameObject.transform.parent.gameObject.SetActive (false);
	}

	public void PushGoTutorial(){
		musicPlay.SetNextMusic ("Tutorial.json");
		selectManager.Push_SelectButton ();
	}
}
