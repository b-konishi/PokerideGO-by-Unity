using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
    public float rotate_speed = 10;
    public float initial_power = 5f;
    public float base_speed;
    public float invi_buff_speed;
    public float acce_ration = 10;
	public float response_ping = 0.1f;

    private Rigidbody rb;
    private Status status;
    private BulletStatus bs;
    private Io io;
    private float angle;
    private GameObject system;

    private float speed;

	private float _time;
	private int id = 0;
	private TimeKeeper tk;

    // Use this for initialization
    void Awake()
    {
        system = GameObject.Find("System");
        io = system.GetComponent<Io>();
        rb = GetComponent<Rigidbody>();
        status = GetComponent<Status>();
        bs = GetComponent<BulletStatus>();
        speed = base_speed;
		_time = Time.deltaTime;
		id = (int.Parse(gameObject.tag.Substring (6, 1))) -1;
		tk = system.GetComponent<TimeKeeper> ();
    }
    // Update is called once per frame
	void Update(){
		angle = io.input [id] * response_ping /(float)1.7;
	}

    void FixedUpdate () {
		if (!tk.is_game_started) {
			return;
		}
        float align_speed;

      //  angle = io.input[0];
        align_speed = base_speed + bs.type[status.bullet_type].buff_speed;

        if (status.is_invincible)
        {
            align_speed += invi_buff_speed;
        }

        if (status.is_shot_duration)
        {
            align_speed += bs.type[status.bullet_type].dash_speed;
        }

        rb.angularVelocity = new Vector3(0f, Time.deltaTime * angle * rotate_speed, 0f);
        //transform.Rotate(0f, Time.deltaTime * angle * rotate_speed, 0f);
       if (rb.velocity.magnitude < base_speed * _time)
		{
            rb.AddForce(transform.forward * base_speed * Time.deltaTime * initial_power);
        }
        else
        {
            speed = speed - ((speed - align_speed)/acce_ration);
            rb.velocity = transform.forward * speed * Time.deltaTime;
		}

    }

}
