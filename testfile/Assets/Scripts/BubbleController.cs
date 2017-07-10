using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;

public class BubbleController : MonoBehaviour {

	Rigidbody2D rigidbody2d;
	Animator animator;
	float angle;
	bool isDead = false;

	public float maxHeigh;
	public float jumpVelocity;
	public float movingVelocityX;
	public GameObject bubbleSprite;
	bool isGround = false;

	public bool IsDead() {
		return isDead;
	}

	public static SerialLib.MyClass serial;

	void OnDestroy() {
		serial.ThreadEnd ();
	}

	// Use this for initialization
	void Start () {
		serial = new SerialLib.MyClass ("/dev/tty.usbserial-AI04SV56", 115200, 256);
		serial.ThreadStart ();

		rigidbody2d = GetComponent<Rigidbody2D> ();
		animator = bubbleSprite.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

		int r = 0;
		string str = serial.GetData ();
		if (str != null) {
			r = int.Parse (str);
		}
		Debug.Log (serial.GetData ());

		if ( r > 1 && transform.position.y < maxHeigh) {
			Jump ();
		}
		ReflectAngle ();
		animator.SetBool ("isGround", isGround);
	}

	void Jump() {
		if (isDead) {
			return;
		}
		isGround = false;
		rigidbody2d.velocity = new Vector2 (0.0f, jumpVelocity);
	}

	void ReflectAngle() {
		float targetAngle;

		// 死亡したらひっくり返す
		if (isDead) {
			targetAngle = 180.0f;
		} else {
			// Y軸に加速している速度、地面などの移動速度から角度を求める
			targetAngle = Mathf.Atan2 (rigidbody2d.velocity.y, movingVelocityX) * Mathf.Rad2Deg;
		}
		// 回転アニメーションを滑らかに
		angle = Mathf.Lerp (angle, targetAngle, Time.deltaTime * 10.0f);
		// Rotationに反映させる 度で回転させる
		bubbleSprite.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Block") {
			if (isDead) {
				return;
			}
			animator.enabled = false;
			// 何かにぶつかったら死亡フラグを立てる
			isDead = true;
		} else {
			isGround = true;
		}
	}

}