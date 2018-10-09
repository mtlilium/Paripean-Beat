using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SelectButtonOnClick : MonoBehaviour {

	Button selectButton;
	GameObject root;

	SelectManager selectManager;
	ListUpManager musicPlay;
	// Use this for initialization
	void Start () {
		selectManager = GameObject.Find ("SelectManager").GetComponent<SelectManager>();
		musicPlay = GameObject.Find ("ListUpManager").GetComponent<ListUpManager>();
		selectButton = GetComponent<Button> ();
		root = selectButton.transform.parent.gameObject;
		selectButton.onClick.AddListener (() => {
			GoToPlay();
		});
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void GoToPlay(){
		AudioManager.Instance.AttachBGMSource.Stop ();

		//f.nameを取ってくるのが課題>>いけた？でもstaticで管理つらい
		musicPlay.SetNextMusic (root.transform.FindChild("TextMusicTitle").GetComponent<Text>().text + ".json");

		selectManager.Push_SelectButton ();
	}
}
