using UnityEngine;
using System.Collections;

public class ShortBulletMove : MonoBehaviour
{
    public float speed;
    private float t;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float angle;

        angle = (1.5f + (Mathf.Sin(t))/2f) * speed;
        transform.Rotate(new Vector3(0,0,1), angle);
        t += speed * Time.deltaTime;

    }
}
