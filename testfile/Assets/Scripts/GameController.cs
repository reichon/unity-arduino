using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	enum State {
		Ready,
		Play,
		GameOver
	}

	State state;

	public BubbleController bubble;
	public GameObject blocks;

	// Use this for initialization
	void Start () {
		Ready ();
	}

	// Update is called once per frame
	void Update () {
		// ゲームステートごとにイベントを監視
		switch (state) {
		case State.Ready:
			// タッチしたらゲームスタート
			if (Input.GetButtonDown ("Fire1")) {
				GameStart ();
			}
			break;
		case State.Play:
			// バブルが死んだらゲームオーバー
			if (bubble.IsDead ()) {
				GameOver ();
			}
			break;
		case State.GameOver:
			// タッチしたらシーンのリロード
			if (Input.GetButtonDown ("Fire1")) {
				Reload ();
			}
			break;
		}
	}

	void Ready() {
		state = State.Ready;
		blocks.SetActive (false);
	}

	void GameStart() {
		state = State.Play;
		blocks.SetActive (true);
	}

	void GameOver() {
		state = State.GameOver;
		// シーン中の全てのScrollObjectコンポーネントを探し出す。
		ScrollObjects[] scrollObjects = GameObject.FindObjectsOfType<ScrollObjects>();
		// 全ScrollObjectのスクロール処理を無効にする
		foreach (ScrollObjects scroll in scrollObjects) {
			scroll.enabled = false;
		}
	}

	void Reload() {
		// Mainシーンを再読み込み
		SceneManager.LoadScene("Main");
	}
}