using UnityEngine;
using System.Collections;

public class Collect : MonoBehaviour {
    private Status status;
	private Shot shot;
	// Use this for initialization
	void Start () {
        status = GetComponent<Status>();
		shot = GetComponent<Shot> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Kinomi")
        {
            status.kinomi_num += 1;
            GameObject.Destroy(col.gameObject);
        }

		if (col.gameObject.tag == "ShortPanel" && status.bullet_type != "Short")
		{
			shot.changeBullet(status.bullet_type);
			status.bullet_type = "Short";
		}

		if (col.gameObject.tag == "MiddlePanel" && status.bullet_type != "Middle")
		{
			shot.changeBullet(status.bullet_type);
			status.bullet_type = "Middle";
		}

		if (col.gameObject.tag == "LongPanel" && status.bullet_type != "Long")
		{
			shot.changeBullet(status.bullet_type);
			status.bullet_type = "Long";
		}

    }
}
