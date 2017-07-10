using System;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace SerialLib {
	public class MyClass {
		SerialPort serial;
		byte[] buf;
		string message;
		Thread SerialThread;
		bool isThreading = false;

		public MyClass(string portName, int baudRate, int bufSize) {
			buf = new byte[bufSize];
			Init(portName, baudRate);
			SerialThread = new Thread(this.UpdateData);
		}

		public void ThreadStart() {
			SerialThread.Start ();
		}

		public void ThreadEnd() {
			isThreading = false;
			SerialThread.Abort ();
			serial.Close ();
		}

		void Init(string portName, int baudRate) {
			serial = new SerialPort (portName, baudRate, Parity.None, 8, StopBits.One);
			try {
				serial.Open ();
				serial.DtrEnable = true;
				serial.RtsEnable = true;
				serial.DiscardInBuffer ();
				serial.ReadTimeout = 50;
			} catch (Exception e) {
				serial = null;
				return;
			}
		}

		public string GetData() {
			return message;
		}

		public void UpdateData() {
			isThreading = true;

			while (isThreading) {
				//int read = 0;
				try {
					message = serial.ReadLine ();
				} catch (TimeoutException e) {
				}
			}
		}
	}
}

