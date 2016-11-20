using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {
    public int kinomi_num;
    public string bullet_type = "Middle";
    public float invincible_time = 3;
    public bool is_invincible = false;
    public float slip_time = 0.5f;
    public bool is_slip = false;
    public bool is_shot_duration = false;
    public float cool_down_timer;
  //  public int id;
    // Use this for initialization
    void Start () {
     //   this.gameObject.tag = "Player" + id.ToString();
     //   Debug.Log("Player" + id.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
