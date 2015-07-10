using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 3;

    void Update()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - speed * Time.deltaTime, pos.z);
    }

    void OnCollisionEnter2D (Collision2D col)
    {
//        Debug.Log ("bullet: collision: " + col);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D col)
    {
//        Debug.Log ("bullet: trigger collision: " + col);
    }
}
