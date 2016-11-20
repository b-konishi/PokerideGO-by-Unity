using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    private Status status;
    private BulletStatus bs;
    public GameObject player_star;
    public GameObject Kinomi_prefab;
    public Material star_material;
	// Use this for initialization
	void Start () {
        status = GetComponent<Status>();
        bs = GetComponent<BulletStatus>();
        star_material.EnableKeyword("_EMISSION");
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(updateSlip());
            StartCoroutine(updateInvinsible());
            ReleaseKinomi(GetComponent<Collider>());
        }
    
	}

    void OnTriggerEnter(Collider col)
    {
        string id = tag.Substring(6, 1);
        if (0 <= col.gameObject.tag.IndexOf("Bullet") 
		    && col.gameObject.tag != "Bullet" + id 
		    && !status.is_invincible 
		    && !col.gameObject.GetComponent<BulletData>().proj_player.GetComponent<Status>().is_invincible
		    )
        {
            StartCoroutine(updateSlip());
            StartCoroutine(updateInvinsible());
            ReleaseKinomi(col);
        }
    }

    void ReleaseKinomi(Collider col)
    {
        GameObject enemy;
        enemy = col.GetComponent<BulletData>().proj_player;
 //       enemy = col.gameObject;

        BulletStatus ebs = enemy.GetComponent<BulletStatus>();
        Bullet e_bullet = ebs.type[enemy.GetComponent<Status>().bullet_type];
        int num = (int)(status.kinomi_num * (e_bullet.damage / 100));
        status.kinomi_num -= num;
        Vector3 pos = transform.position;
        pos.y += 0.2f;
        for(int i = 0; i< num; i++)
        {
            GameObject kinomi = Instantiate(Kinomi_prefab, pos,new Quaternion(0, 0, 0, 0)) as GameObject;
            Vector3 force = new Vector3(0, 100, 0);
            force.x = e_bullet.dispersion * Random.Range(-1f, 1f);
            force.z = e_bullet.dispersion * Random.Range(-1f, 1f);
            Vector3 norm = force.normalized;
            kinomi.GetComponent<Rigidbody>().AddForce(norm * 700f);
        }
    }

    IEnumerator updateInvinsible()
    {
        float timer = 0;
        int num = 4;
        status.is_invincible = true;
        while (timer < status.invincible_time)
        {
           // star_material.SetColor("_EmissionColor", Color.HSVToRGB(0, 0, Mathf.Cos(timer/ status.invincible_time * num *2*Mathf.PI+Mathf.PI)/2+0.5f));
          	
			timer += Time.deltaTime;
            yield return null;
        }

        status.is_invincible = false;
    }

    IEnumerator updateSlip()
    {
        status.is_slip = true;
        float timer = 0;
        int num = 2;
        float angle = 180;
        while (timer < status.slip_time)
        {
//            Debug.Log("angle "+ angle);
            player_star.transform.localEulerAngles = new Vector3(0, angle, 0);
            angle = 180 + timer / status.slip_time * 360 * num;
            timer += Time.deltaTime;
            yield return null;
        }
        player_star.transform.localEulerAngles = new Vector3(0, 180, 0);
        status.is_slip = false;
    }
}
