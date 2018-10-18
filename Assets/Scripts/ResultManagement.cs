using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ResultManagement : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PushBackSelectButton(){
		//Destroy (AudioManager.Instance.gameObject);
		SceneManager.LoadScene ("SelectScene");
	}

	public void PushBackTitleButton(){
		Destroy (AudioManager.Instance.gameObject);
		SceneManager.LoadScene ("TitleScene");
	}

	public void PushRetryButton(){
		SceneManager.LoadScene ("PlayingScene");
	}


}
