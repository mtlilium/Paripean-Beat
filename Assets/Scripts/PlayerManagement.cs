using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour {

	//ゲーム上のPosition
	public GameObject player_Pos;

	private float[] posX = {-300, 0, 300};

	//マウス位置座標
	private Vector3 mouse_Pos;

	//マウス座標をゲーム内の座標に変換
	private Vector3 screenTOWorldPoint_Pos;

	//マウスの移動量
	private float mouse_X_Delta;
	private float mouse_Y_Delta;
	//マウス感度
	private const float mouseSensitivity = 0.1f;

	//今どのレーンにいるか
	public int nowLine; //1,2,3

	//以下UI制御

	public GameObject musicNameText;

	public GameObject perfectCountText;
	public GameObject goodCountText;
	public GameObject badCountText;
	public GameObject missCountText;

	public GameObject scoreText;

	public GameObject comboText;
	public GameObject maxComboText;


	//keyBeam
	public GameObject[] keyBeams;

	//Music経過バー
	public Slider musicSlider;

	//1回の入力で1ノーツだけ反応させるため(巻き込み防止) Redはまだ上手くいってない。青は多分大丈夫   最初のノーツの判定がおかしい！！！！！！
	public static bool inputLimit; //falseの時しか入力を受け付けない　判定されたらfalseにもどる
	public static bool inputTrigger;

	private float recentWheelSign;
	// Use this for initialization
	void Start () {
		/*Vector3 init_Pos = new Vector3(0.0f, -2.36111111f, 10f);
		player_Pos.transform.position = init_Pos;*/

		Vector3 init_Pos = new Vector3 (0, 0, 0);
		player_Pos.transform.localPosition = init_Pos;

		musicNameText.GetComponent<Text> ().text = ListUpManager.nextPlayMusic.name;

		musicSlider.value = 0;

		inputLimit = false;
		inputTrigger = true;

		recentWheelSign = 0;

	}



	// Update is called once per frame
	void Update () {
		Get_Player_Pos ();　//Input.GetAxis("Mouse X")

		DisplayScore ();

		DisplayCombo ();

		DisplayTime ();

		Get_Now_Line (player_Pos.transform.localPosition.x);

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.J) || Input.GetAxisRaw("Mouse ScrollWheel") != 0 ) {
			LightKeyBeam (nowLine);
		}
		if ((Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.J) || (Input.GetAxis("Mouse ScrollWheel") != 0 && Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) != recentWheelSign) ) && inputTrigger && !inputLimit) {
			recentWheelSign = Mathf.Sign (Input.GetAxis ("Mouse ScrollWheel"));
			Invoke ("ResetRecentWheelSign", NotesManagement.goodTime / 2);
			inputLimit = true;
			inputTrigger = false;
		}
	}

	private void ResetRecentWheelSign(){
		recentWheelSign = 0;
	}


	public void Get_Player_Pos(){
		//smoothモード
		mouse_Pos = Input.mousePosition;
		//z軸修正
		mouse_Pos.z = 10f;
		//変換
		screenTOWorldPoint_Pos = Camera.main.ScreenToWorldPoint (mouse_Pos);
		Vector3 tmp = screenTOWorldPoint_Pos;
		tmp.y = -2.36111111f;
		if (screenTOWorldPoint_Pos.x >= 5.0f) {
			tmp.x = 5.0f;
		} else if (screenTOWorldPoint_Pos.x <= -5.0f) {
			tmp.x = -5.0f;
		}
		screenTOWorldPoint_Pos = tmp;
		player_Pos.transform.position = screenTOWorldPoint_Pos;

	}

	/*public void Get_Player_Pos(float mouseAxisX){
		//ブロックモード
		if (mouseAxisX > 5) {
			if (nowLine <= 2 && flag) {
				player_Pos.transform.localPosition = new Vector3 (posX [(nowLine - 1) + 1], 0, 0);
				flag = false;
				return;
			}
		} else if (mouseAxisX < -5) {
			if (nowLine >= 2 && flag) {
				player_Pos.transform.localPosition = new Vector3 (posX [(nowLine - 1) - 1], 0, 0);
				flag = false;
				return;
			}
		}
	} */


	private void DisplayScore(){
		perfectCountText.GetComponent<Text> ().text = "Perfect: " + NotesManagement.perfectCount.ToString();
		goodCountText.GetComponent<Text> ().text = "Good: " + NotesManagement.goodCount.ToString();
		badCountText.GetComponent<Text> ().text = "Bad: " + NotesManagement.badCount.ToString();
		missCountText.GetComponent<Text> ().text = "Miss: " + NotesManagement.missCount.ToString();

		scoreText.GetComponent<Text> ().text = NotesManagement.score.ToString();
		
	}

	private void DisplayCombo(){
		if (NotesManagement.combo == 0) {
			comboText.GetComponent<Text> ().text = "";
		} else {
			comboText.GetComponent<Text> ().text = NotesManagement.combo.ToString () + " COMBO ";
		}
		maxComboText.GetComponent<Text> ().text = NotesManagement.maxCombo.ToString();
	}

	private void DisplayTime(){
		int m, s;
		if (NotesManagement.timer <= 4) {
			m = 0;
			s = 0;
			return;
		}
		m = (int)((NotesManagement.timer - 4.0f) / 60);
		s = (int)(NotesManagement.timer - 4.0f) - 60 * m;

		musicSlider.value = (60*m + s)/AudioManager.Instance.AttachBGMSource.clip.length;
	}

	private void Get_Now_Line(float localPos_X){
		if (-450f <= localPos_X && localPos_X <= -150f) {
			nowLine = 1;
		} else if (-150f < localPos_X && localPos_X < 150f) {
			nowLine = 2;
		} else {
			nowLine = 3;
		}
	}

	private void LightKeyBeam(int line){
		keyBeams [line - 1].SetActive (true);
		StartCoroutine (TurnOffKeyBeam (NotesManagement.perfectTime/2,line-1));
	}

	IEnumerator TurnOffKeyBeam(float delay,int num){
		yield return null;
		yield return new WaitForSeconds (delay);
		keyBeams [num].SetActive (false);
	}
}
