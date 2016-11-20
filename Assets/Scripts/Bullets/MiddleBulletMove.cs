using UnityEngine;
using System.Collections;

public class MiddleBulletMove : MonoBehaviour {
    public float bias;
    public float t_bias;
    public float speed = 3;

    private float t;
    private Vector3 base_pos;
	// Use this for initialization
	void Start () {
        base_pos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = base_pos;
        Vector3 angle = Vector3.zero;
        pos.z += Mathf.Sin(t + t_bias) * bias;
        angle.x = (1+Mathf.Sin(t + t_bias)) * bias;
        transform.localPosition = pos;
        transform.Rotate(angle);
        t += speed * Time.deltaTime;

	}
}
