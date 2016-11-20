using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System.Linq;

public class Io : MonoBehaviour
{
	// 各スレッドごとに必要な通信用変数をもつ構造体
	public struct TransportData {
		public SerialPort serialPort_;
		public Thread thread_;
		public bool isRunning_;
		
		public string message_;
		public bool isNewMessageReceived_;
	}
	
	// 9600に固定
	private const int BAUDRATE = 9600;
	
	public string[] portName = {"COM7", "COM6", "COM3", "COM4"};
	
	private TransportData[] info = new TransportData[4];
	
	// 関数配列定義
	// private delegate void SerialReader();
	// SerialReader[] reader = new SerialReader[4];
	
	// アイテムボタンのキー設定
	public KeyCode[] shotkey = {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D};
	
	[HideInInspector]
	public float[] input = {0.0f,0.0f,0.0f,0.0f};
	
	// 通信準備
	void Awake()
	{
		foreach (int i in Enumerable.Range(0, info.Length)) {
			info[i].isRunning_ = false;
			info[i].isNewMessageReceived_ = false;
		}
		
		Open();
	}
	
	// 通信破棄処理
	void OnDestroy()
	{
		foreach (var i in info) {
			i.serialPort_.Close();
			i.serialPort_.Dispose();
		}
	}
	
	// スレッドを立てて通信開始
	private void Open()
	{
		foreach (int i in Enumerable.Range(0, info.Length)) {
			info[i].serialPort_ = new SerialPort(portName[i], BAUDRATE, Parity.None, 8, StopBits.One);
			
			info[i].serialPort_.Open();
			
			info[i].isRunning_ = true;
			
			info[i].thread_ = new Thread(new ParameterizedThreadStart(Read));
			info[i].thread_.Start(i);
		}
		
	}
	
	// Arduinoからのデータ読み込み
	// obj: どのスレッドかを示すID
	private void Read(object obj)
	{
		int id = (int)obj;
		while (info[id].isRunning_ && info[id].serialPort_ != null && info[id].serialPort_.IsOpen) {
			try {
				info[id].message_ = info[id].serialPort_.ReadLine();
				info[id].isNewMessageReceived_ = true;
				
				var data = info[id].message_.Split(new string[]{"\t"}, System.StringSplitOptions.None);
				if (data.Length < 2) continue;
				
				// 角度情報として取得
				input[id] = float.Parse (data[0]);
				
			} catch (System.Exception e) {
				Debug.LogWarning(e.Message);
			}
		}
	}
}