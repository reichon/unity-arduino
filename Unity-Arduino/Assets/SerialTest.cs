using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;
using UnityEngine;

public class SerialTest : MonoBehaviour {
	public GameObject rocket;
	public static SerialLib.MyClass serial;

	// Use this for initialization
	void Start () {
		serial = new SerialLib.MyClass ("/dev/tty.usbserial-AI04SV56", 115200, 256);
		serial.ThreadStart ();
	}
	
	// Update is called once per frame
	void Update () {
		string str = serial.GetData ();
		if (str != null) {
			int r = int.Parse (str);
		}
		Debug.Log (serial.GetData ());
	}

	void OnDestroy() {
		serial.ThreadEnd ();
	}
}
