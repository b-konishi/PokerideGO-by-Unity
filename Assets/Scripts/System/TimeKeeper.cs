using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour {
	public Text system_text;
	public Light sun;
	public float time_limit = 150;
	public float start_sun_height = 90;
	public float end_sun_height = 190;

	public bool is_game_started = false;

	private float base_limit = 0;
	// Use this for initialization
	void Start () {
		is_game_started = false;
		base_limit = time_limit;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine(StartCountDown());
		}
	}

	void FixedUpdate()
	{
		UpdateSun ();
		if (is_game_started) {
			time_limit -= Time.deltaTime;
		}
		if (time_limit < 11 && time_limit > 0) {
			system_text.text = time_limit.ToString("F0");
		}
		if (time_limit < 0 && is_game_started) {
			StartCoroutine(GameOver());
		}
	}

	void UpdateSun(){
		float bias = (1 - (time_limit / base_limit)) * (end_sun_height - start_sun_height);
		Debug.Log (bias);
		sun.transform.rotation = Quaternion.Euler(start_sun_height + bias, 0, 0);

	}

	IEnumerator StartCountDown(){
		Debug.Log ("STARTCOUNTDOWN");
		system_text.text = "3";
		yield return new WaitForSeconds (1);
		system_text.text = "2";
		yield return new WaitForSeconds (1);
		system_text.text = "1";
		yield return new WaitForSeconds (1);
		system_text.text = "GO!!";
		is_game_started = true;
		yield return new WaitForSeconds (1);
		system_text.text = "";
	}

	IEnumerator CountIn(string num){
		system_text.text = num;
		yield return new WaitForSeconds (1);
		system_text.text = "";
	}

	IEnumerator GameOver(){
		is_game_started = false;
		system_text.text = "TIME UP!";
		yield return new WaitForSeconds (2);
		system_text.text = "";
	}
}
