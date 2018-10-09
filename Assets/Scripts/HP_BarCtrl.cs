using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HP_BarCtrl : MonoBehaviour {

	public Slider _slider;

	public GameObject hpMessageText;

	//Result画面呼ぶ用
	public GameObject resultBack;

	private float _hp;

	// Use this for initialization
	void Start () {
		// スライダーを取得する
		_slider = GameObject.Find("HP_Bar").GetComponent<Slider>();
		_hp = 1;
	}

	// Update is called once per frame
	void Update () {
		//_hp -= 0.005f;
		if (_hp <= 0) {
			_hp = 0;
			//とりあえず
			AudioManager.Instance.AttachBGMSource.Stop();
			Destroy (ObjectPool.instance.gameObject);
			if (resultBack.gameObject.activeSelf == false) {
				resultBack.gameObject.SetActive (true);
				if (NotesManagement.timer >= AudioManager.Instance.AttachBGMSource.clip.length + 6.0f) {
					NotesManagement.timer = AudioManager.Instance.AttachBGMSource.clip.length + 6.0f;
				}
			}
		}
		if (_hp > 1) {
			_hp = 1;
		}

		// HPゲージに値を設定
		_slider.value = _hp;
		DisplayHpMessage (_slider.value);
	}

	public void DisplayHpMessage(float _hp){
		string num = ((Mathf.CeilToInt(100 * _hp))).ToString ();
		hpMessageText.GetComponent<Text>().text = num + " %";
	}

	public void ChangeHP(float value){
		if (_hp <= 0.2f) {
			value /= 2f;
		}
		_hp += value;
	}
}
