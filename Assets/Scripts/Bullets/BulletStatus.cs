using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bullet
{
    public float cool_down;
    public float buff_speed;
    public float rotate_speed;
    public float bullet_speed;
    public float damage;
    public float dispersion;
    public float duration_time;

    public float max_size;
    public float dash_speed;

}



public class BulletStatus : MonoBehaviour {
    public Bullet long_bullet;
    public Bullet middle_bullet;
    public Bullet short_bullet;
    public Dictionary<string, Bullet> type = new Dictionary<string, Bullet>();

    void Start () {

    }

    void Awake()
    {
        type.Add("Long", long_bullet);
        type.Add("Middle", middle_bullet);
        type.Add("Short", short_bullet);
    }
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(weapon);
	}
}
