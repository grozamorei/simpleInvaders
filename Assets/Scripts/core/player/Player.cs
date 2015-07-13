using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] private ParticleSystem leftP;
    [SerializeField] private ParticleSystem rightP;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float baseEmission = 10;
    [SerializeField] private float fasterEmission = 100;
    
    [SerializeField] private float moveSpeed = 5;
    
    private bool _leftDown = false;
    private bool _rightDown = false;
    
    void Awake()
    {
        leftP.emissionRate = baseEmission;
        rightP.emissionRate = baseEmission;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            rightP.emissionRate = fasterEmission;
            _leftDown = true;
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            rightP.emissionRate = baseEmission;
            _leftDown = false;
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            leftP.emissionRate = fasterEmission;
            _rightDown = true;
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            leftP.emissionRate = baseEmission;
            _rightDown = false;
        }
        
        float currentVel = 0;
        if (_rightDown) {
            currentVel += Time.deltaTime * moveSpeed * 1;
        }
        if (_leftDown) {
            currentVel += Time.deltaTime * moveSpeed * -1;
        }
        
        var pos = transform.position;
        transform.position = new Vector3(pos.x + currentVel, pos.y, pos.z);
    }
    
    void OnCollisionEnter2D (Collision2D col)
    {
        Vector2 midPoint = new Vector2();
        int collisionsNum = col.contacts.Length;
        for (int i = 0; i < collisionsNum; i++) {
            midPoint.x += col.contacts[i].point.x;
            midPoint.y += col.contacts[i].point.y;
        }
        midPoint.x /= collisionsNum; 
        midPoint.y /= collisionsNum; 
        
        var ps = Instantiate(explosion);
        ps.transform.position = new Vector3(midPoint.x, midPoint.y, -3);
        ps.Play();
    }
}
