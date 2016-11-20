using UnityEngine;
using System.Collections;

public class KinomiCtrl : MonoBehaviour {
    private bool is_touch_floor = false;
    private Vector3 base_pos;
    private float t;
    // Use this for initialization
	void Start () {
        this.gameObject.tag = "Flying_Kinomi";
        GetComponent<SphereCollider>().isTrigger = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (is_touch_floor)
        {
            Vector3 pos;
            pos = base_pos;
            pos.y += (Mathf.Cos(t)+1)/4+0.25f;
            transform.position = pos;
            this.gameObject.tag = "Kinomi";
            t += 0.1f;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            GetComponent<Rigidbody>().useGravity = false;
            is_touch_floor = true;
            base_pos = transform.position;
            this.gameObject.tag = "Kinomi";
            this.gameObject.layer = 9;
            GetComponent<SphereCollider>().isTrigger = true;
        }
    }
}
