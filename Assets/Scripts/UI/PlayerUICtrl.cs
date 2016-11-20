using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUICtrl : MonoBehaviour {
	public Text t_kinomiNum;
	public Text t_ranking;
	public Image i_cooldown;

	private Status status;
	private DB db;
	private int id;
	private BulletStatus bs;
	// Use this for initialization
	void Start () {
		status = GetComponent<Status> ();
		id = int.Parse(tag.Substring (6, 1)) -1;
		bs = GetComponent<BulletStatus> ();
	}

	void Awake()
	{
		db = GameObject.Find ("System").GetComponent<DB>();
	}
	
	// Update is called once per frame
	void Update () {
		t_kinomiNum.text = status.kinomi_num.ToString();
		t_ranking.text = (db.ranking[id]).ToString();
		if (status.cool_down_timer == 0) {
			i_cooldown.fillAmount = 0;
		} else {
			i_cooldown.fillAmount = 1-(status.cool_down_timer / bs.type[status.bullet_type].cool_down);
		}
	}
}
