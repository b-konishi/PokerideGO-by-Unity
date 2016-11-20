using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shot : MonoBehaviour {
    public GameObject long_bullet_base;
    public GameObject middle_bullet_base;
    public GameObject short_bullet_base;
    public GameObject bullet_prefab;
    public KeyCode shot_key;

    private Dictionary<string, GameObject> bullet_base = new Dictionary<string, GameObject>();
    private Dictionary<string, Vector3> bullet_base_scale = new Dictionary<string, Vector3>();
    private Dictionary<string, Bullet> bs = new Dictionary<string, Bullet>();
    private float duration_timer;
    private Status status;
	private Io io;
    private BulletStatus bstatus;
	private int id;
	private TimeKeeper tk;

    private float short_rotate_bias = 0;

	// Use this for initialization
	void Start () {
        status = GetComponent<Status>();
        bstatus = GetComponent<BulletStatus>();
        bullet_base.Add("Long", long_bullet_base);
        bullet_base.Add("Middle", middle_bullet_base);
        bullet_base.Add("Short", short_bullet_base);
        bullet_base_scale.Add("Long", bullet_base["Long"].transform.localScale);
        bullet_base_scale.Add("Middle", bullet_base["Middle"].transform.localScale);
        bullet_base_scale.Add("Short", bullet_base["Short"].transform.localScale);
        bs.Add("Long", bstatus.type["Long"]);
        bs.Add("Middle", bstatus.type["Middle"]);
        bs.Add("Short", bstatus.type["Short"]);

        bullet_base["Long"].transform.localScale = Vector3.zero;
        bullet_base["Middle"].transform.localScale = Vector3.zero;
        bullet_base["Short"].transform.localScale = Vector3.zero;

		io = GameObject.Find ("System").GetComponent<Io> ();
		id = int.Parse(tag.Substring (6, 1))-1;
		tk = GameObject.Find ("System").GetComponent<TimeKeeper> ();

    }
	
	// Update is called once per frame
	void Update () {
		if (!tk.is_game_started) {
			return;
		}

		if (Input.GetKeyDown(io.shotkey[id]) && status.cool_down_timer == 0 && !status.is_invincible)
		{
			resetCooldown();
			setDurationTimer();
			if (status.bullet_type == "Short")
			{
				StartCoroutine(BulletSizeUp(status.bullet_type));
			}
			else
			{
				StartCoroutine(shotBullet());
				StartCoroutine(BulletView2zero(status.bullet_type));
			}
		}
	}

	void FixedUpdate(){
		coolDownCalc();
		updateDurationFrag();
		updateBulletView();
		shortBulletAction ();
	}


    IEnumerator BulletSizeUp(string bullet_type)
    {
        Vector3 scale;
        float align_size;
        float dsize;
        scale = short_bullet_base.transform.localScale;
        align_size = scale.x * bs[bullet_type].max_size;
        dsize = (align_size - scale.x) / 20;
        for (int i = 0; i < 20; i++)
        {
            scale.x += dsize;
            scale.z += dsize;

			if(status.bullet_type == bullet_type)
			{
            	short_bullet_base.transform.localScale = scale;
			}
			yield return null;
        }
        yield return new WaitForSeconds(bs[bullet_type].duration_time-0.2f);
        for (int i = 0; i < 20; i++)
        {
            scale.x -= dsize;
            scale.z -= dsize;

			if(status.bullet_type == bullet_type)
			{
				short_bullet_base.transform.localScale = scale;
			}
			yield return null;
        }
        //short_bullet_base.transform.localScale = align_size;
    }

	public void changeBullet(string bullet_type){
		duration_timer = 0;
		status.is_shot_duration = false;
		StartCoroutine (BulletView2zero(bullet_type));
	}

    IEnumerator BulletView2zero(string bullet_type)
    {
        Vector3 scale;
        scale = bullet_base[bullet_type].transform.localScale;
        for (int i = 0; i< 20; i++)
        {
            scale.x /=1.4f;
            scale.y /= 1.4f;
            scale.z /= 1.4f;
            bullet_base[bullet_type].transform.localScale = scale;
            yield return null;
        }

        bullet_base[status.bullet_type].transform.localScale = Vector3.zero;
    }

    void updateBulletView()
    {
       if(status.cool_down_timer == 0)
        {
            bullet_base[status.bullet_type].transform.localScale = bullet_base_scale[status.bullet_type];
        }
        
    }

    void shortBulletAction()
    {
        if (status.bullet_type == "Short")
        {
            if (status.is_shot_duration) {
                short_rotate_bias = bs["Short"].rotate_speed;
            }else if(status.cool_down_timer != 0)
            {
                if (short_rotate_bias > - bs["Short"].rotate_speed + 1)
                {
                    short_rotate_bias -= 0.3f;
                }
                    
            }
            else
            {
                short_rotate_bias = 0;
            }
            short_bullet_base.transform.Rotate(new Vector3(0, short_rotate_bias + bs["Short"].rotate_speed, 0));
        }
    }

    void updateDurationFrag()
    {
        if(duration_timer > 0)
        {
            status.is_shot_duration = true;
            duration_timer -= Time.deltaTime;
        }
        else
        {
            status.is_shot_duration = false;
            duration_timer = 0;
        }
        
    }

    void resetCooldown()
    {
        status.cool_down_timer = bs[status.bullet_type].cool_down;
    }

    void setDurationTimer()
    {
        duration_timer = bs[status.bullet_type].duration_time;
    }

    IEnumerator shotBullet()
    {
        if(status.bullet_type == "Long")
        {
            GameObject bullet = Instantiate(bullet_prefab, transform.position, transform.rotation) as GameObject;
            bullet.GetComponent<Rotate>().rotate_speed = bs["Long"].rotate_speed;
            bullet.GetComponent<Transform>().localScale *= bs["Long"].max_size;
            bullet.GetComponent<BulletData>().proj_player = this.gameObject;
            Vector3 velo;
            velo.x = transform.forward.x;
            velo.y = 0;
            velo.z = transform.forward.z;
            bullet.GetComponent<Rigidbody>().velocity = velo * bs["Long"].bullet_speed;
            bullet.tag = "Bullet" + gameObject.tag.Substring(6, 1);
			Debug.Log ("Shot Long");
            yield return new WaitForSeconds(bs["Long"].duration_time);
            Destroy(bullet);
        } else if (status.bullet_type == "Middle")
        {
			int num = 5;
			float angle = 10;
            GameObject[] bullet = new GameObject[num];
            for(int i = 0; i< num; i++)
            {
                bullet[i] = Instantiate(bullet_prefab, transform.position, transform.rotation * Quaternion.Euler(0, ((num-1)/2f-i)*angle, 0)) as GameObject;
                bullet[i].GetComponent<Rotate>().rotate_speed = bs["Middle"].rotate_speed;
                bullet[i].GetComponent<Transform>().localScale *= bs["Middle"].max_size;
                bullet[i].GetComponent<BulletData>().proj_player = this.gameObject;
                Vector3 velo;
                velo.x = transform.forward.x;
                velo.y = 0;
                velo.z = transform.forward.z;
				velo = Quaternion.Euler(0, ((num-1)/2f-i)*angle, 0) * velo;
                bullet[i].GetComponent<Rigidbody>().velocity = velo * bs["Middle"].bullet_speed;
                bullet[i].tag = "Bullet" + gameObject.tag.Substring(6, 1);
            }
			Debug.Log ("Shot Middle");
            yield return new WaitForSeconds(bs["Middle"].duration_time);
            foreach (GameObject bul in bullet)
            {
                Destroy(bul);
            }
        }
    }

    void coolDownCalc()
    {
        if (status.cool_down_timer > 0)
        {
            status.cool_down_timer -= Time.deltaTime;
        }
        else
        {
            status.cool_down_timer = 0;
        }
    }
}
