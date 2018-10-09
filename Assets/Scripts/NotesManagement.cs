using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NotesManagement : MonoBehaviour {

	//Notesプレハブ
	public GameObject noteBlue;
	public GameObject noteRed;
	public GameObject noteSlideRight;
	public GameObject noteSlideLeft;

	private NoteObject noteSample;

	//NotesをDicisionLineに並べるために必要
	private GameObject obj;

	private float[] note_PosX = {-300f,0,300f,-300f,0,300f,-150f,-150f,150f,150f}; 

	public static float laneLength = 600f; //レーンの長さ
	public static float createNotes_PosY = 800f; //ノーツを生成する高さ　（判定ラインからの）

	//エフェクト プレハブ
	public GameObject[] effectBlue;
	public GameObject effectRed;
	public GameObject effectSlide;

	public GameObject timeText2;

	//タイマー
	public static float timer;


	//各判定数
	public static int perfectCount;
	public static int goodCount;
	public static int badCount;
	public static int missCount;

	//コンボ
	public static int combo;
	public static int maxCombo;

	//判定幅
	public const float perfectTime = 0.134f;//0.066f;//0.099f;//0.099f
	public const float goodTime = 0.2f;//0.134f;//0.165f;//0.165f
	public const float badTime = 0.266f;//0.2f;//0.231f;//0.231f;

	//score
	public static int perfectScore;
	public static int goodScore;
	public static int score;
	public static bool clearStatus;



	GameObject[] tagObjects;

	//Result画面呼ぶ用
	public GameObject resultBack;

	//notesリスト
	public static List<int>[] noteList;
	private static int index = 0;


	// Use this for initialization
	void Start () {
		timer = 0.0f;
		AudioManager.Instance.AttachBGMSource.Stop ();
		noteSample = GameObject.Find ("NoteObjects").GetComponent<NoteObject> ();

		perfectCount = 0;
		goodCount = 0;
		badCount = 0;
		missCount = 0;
		combo = 0;
		maxCombo = 0;
		score = 0;

		clearStatus = false;

	

		//リスト初期化
		noteList = new List<int>[3];
		noteList [0] = new List<int> (); //レーン１にくるやつをぶちこむ。前から判定していって判定したらlistの[0]を消してずらしていく。スライドは入れない
		noteList [1] = new List<int> (); //2
		noteList [2] = new List<int> (); //3

	
		//シーン遷移後４秒後に曲の再生
		StartCoroutine (MusicStart (4.000f, ListUpManager.nextPlayMusic.name));

		//パーフェクト判定は1000000点をノーツ数で割った点数。goodはその半分

		//Notes生成
		//y座標が　createNotes_PosY = 800f(判定ラインから見て) に来る時間に生成
		if (SelectManager.difficulty == 0 || (SelectManager.difficulty == 1 && ListUpManager.nextPlayMusic.notesAnother == null)) {
			perfectScore = Mathf.CeilToInt((float)1000000/ListUpManager.nextPlayMusic.notes.Length) ;
			SelectManager.difficulty = 0;
			foreach (Notes notes in ListUpManager.nextPlayMusic.notes) {
				StartCoroutine (CreateNotes (NoteTiming (ListUpManager.nextPlayMusic.BPM, notes.LPB, notes.num) - (createNotes_PosY / (laneLength * SelectManager.highSpeed)), notes.block, NoteTiming (ListUpManager.nextPlayMusic.BPM, notes.LPB, notes.num)));
			}
		} else {
			perfectScore = Mathf.CeilToInt((float)1000000/ListUpManager.nextPlayMusic.notesAnother.Length) ;
			foreach (Notes notes in ListUpManager.nextPlayMusic.notesAnother) {
				StartCoroutine (CreateNotes (NoteTiming (ListUpManager.nextPlayMusic.BPM, notes.LPB, notes.num) - (createNotes_PosY / (laneLength * SelectManager.highSpeed)), notes.block, NoteTiming (ListUpManager.nextPlayMusic.BPM, notes.LPB, notes.num)));
			}
		}
		goodScore = (int)(perfectScore / 2);

	}
	// Update is called once per frame
	void Update () {
		if (CheckFinish()) {
			if (clearStatus == false) {
				clearStatus = true;
				SpawnResult ();
			}

		}
		//時間制御関係

		//musicの時間基準にしたい。。。けど無理。。。
		/*if (timer >= 4.0f) {
			timer = AudioManager.Instance.AttachBGMSource.time + 4.0f;
		} else {
			timer += 1 * Time.smoothDeltaTime;
		} */

		//timer += Time.deltaTime;
		timer = Time.timeSinceLevelLoad; //なんかこっちのほうがよさそう。。
		//timeText2.GetComponent<Text> ().text = NotesManagement.timer.ToString();
	
	}

	//ノートが曲が始まって何秒後に判定ラインに来るか
	private float NoteTiming (int BPM, int LPB, int num){
		return (float)(60 * (double)num / ((double)BPM * (double)LPB)) + 4.000f; //4.0秒無理矢理遅らせてる。　＜－－＞ MusicStart を4.0秒遅らせてる
	} //offsetもひきすうにいれないといけない

	IEnumerator MusicStart(float delay, string BGM){
		yield return null;
		yield return new WaitForSeconds (delay);
		AudioManager.Instance.PlayBGM (BGM);
		//timeText2.GetComponent<Text> ().text = "多分曲とのズレ時間:\n " + (NotesManagement.timer - 4.0f).ToString();
	}
		

	private bool CheckFinish(){
		return timer >= AudioManager.Instance.AttachBGMSource.clip.length + 6.0f;
	}

	IEnumerator CreateNotes(float createTime, int block, float timeDiff){
		yield return null;
		yield return new WaitForSeconds (createTime);
		if (block < 3) {
			obj = (GameObject)ObjectPool.instance.GetGameObject (noteBlue, new Vector3 (note_PosX[block], createNotes_PosY , 0.0f), transform.localRotation);
		} else if (block < 6) {
			obj = (GameObject)ObjectPool.instance.GetGameObject (noteRed, new Vector3 (note_PosX[block], createNotes_PosY , 0.0f), transform.localRotation);
		} else if (block == 6 || block == 8) {
			obj = (GameObject)ObjectPool.instance.GetGameObject (noteSlideRight, new Vector3 (note_PosX[block], createNotes_PosY , 0.0f), transform.localRotation);
		} else {
			obj = (GameObject)ObjectPool.instance.GetGameObject (noteSlideLeft, new Vector3 (note_PosX[block], createNotes_PosY , 0.0f) , transform.localRotation);
		}
		//obj.transform.SetParent(noteParent.transform,false);
		obj.GetComponent<NoteObject> ().NoteInit (block,timeDiff,index);
		index++;
	}
		

	private void SpawnResult(){
		if (resultBack.gameObject.activeSelf == false) {
			resultBack.gameObject.SetActive (true);
			if (timer >= AudioManager.Instance.AttachBGMSource.clip.length + 6.0f) {
				timer = AudioManager.Instance.AttachBGMSource.clip.length + 6.0f;
			}
		}
	}

	public void AddScore(int point){
		score += point;
		if (score >= 1000000) {
			score = 1000000;
		}
	}

	public void AddCombo(){
		combo++;
		if (combo >= maxCombo) {
			maxCombo = combo;
		}
	}

	public void ResetCombo(){
		combo = 0;
	}

	public void ExplosionBlue(string line){
		/*obj = (GameObject)ObjectPool.instance.GetGameObject (effectBlue, new Vector3 (pos_X, 0.0f , 70.0f), transform.localRotation);
		obj.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (SetEffectsFalse ((float)obj.GetComponent<ParticleSystem> ().main.duration, obj.gameObject));*/
		//if (effectBlue [int.Parse (line) - 1].gameObject.activeSelf == false) {
		//	effectBlue [int.Parse (line) - 1].gameObject.SetActive (true);
		//	effectBlue [int.Parse (line) - 1].gameObject.GetComponent<ParticleSystem> ().time = 0.1f;
			effectBlue [int.Parse (line) - 1].gameObject.GetComponent<ParticleSystem> ().Emit (1);
		//}
		//StartCoroutine (SetEffectsFalse (goodTime, effectBlue [int.Parse (line) - 1].gameObject)); //int.Parse (line) - 1
	}

	public void ExplosionRed(string line){
		/*obj = (GameObject)ObjectPool.instance.GetGameObject (effectRed, new Vector3 (pos_X, 0.0f , 70.0f), transform.localRotation);
		obj.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (SetEffectsFalse ((float)obj.GetComponent<ParticleSystem> ().main.duration, obj.gameObject));*/
		effectBlue [int.Parse (line) - 1].gameObject.GetComponent<ParticleSystem> ().Emit (1);
	}

	public void ExplosionSlide(){
		if (effectSlide.activeSelf == false) {
			effectSlide.SetActive (true);
		}
		StartCoroutine (SetEffectsFalse (goodTime, effectSlide.gameObject));

	}

	IEnumerator SetEffectsFalse(float delay, GameObject obj){
		yield return new WaitForSeconds (delay);
		//ObjectPool.instance.ReleaseGameObject (obj);
		effectSlide.SetActive(false);
	}
}
