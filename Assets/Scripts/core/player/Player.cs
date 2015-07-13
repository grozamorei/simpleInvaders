using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] private ParticleSystem leftP;
    [SerializeField] private ParticleSystem rightP;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float baseEmission = 10;
    [SerializeField] private float fasterEmission = 100;
    
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private Game _game;
    [SerializeField] private int leftBound = 1;
    [SerializeField] private int rightBound = 10;
    
    [SerializeField] private int flickerTime = 2;
    [SerializeField] private int flickerSpeed = 1;
    
    private bool _leftDown = false;
    private bool _rightDown = false;
    
    private bool _flicker = false;
    private float _currentTime = 0;
    private int direction = -1;
    private Material _material;
    private PolygonCollider2D _collider;
    
    void Awake()
    {
        leftP.emissionRate = baseEmission;
        rightP.emissionRate = baseEmission;
        
        _material = GetComponent<SpriteRenderer>().material;
        _collider = GetComponent<PolygonCollider2D>();
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
        if (pos.x + currentVel >= rightBound) {
            return;
        }
        if (pos.x + currentVel <= leftBound) {
            return;
        }
        transform.position = new Vector3(pos.x + currentVel, pos.y, pos.z);
        
        if (_flicker) {
            _currentTime += Time.deltaTime;
            
            if (_currentTime >= flickerTime) {
                _flicker = false;
                _collider.enabled = true;
                return;
            }
            
            var c = _material.GetColor("_Color");
            if (direction == -1) {
                c.a -= Time.deltaTime * flickerSpeed;
                if (c.a <= 0.2) {
                    direction = 1;
                }
            } else {
                c.a += Time.deltaTime * flickerSpeed;
                if (c.a >= 1) {
                    direction = -1;
                }
            }
            _material.SetColor("_Color", c);
        }
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
        
        _game.subtractLife();
        if (_game.currentLifes == 0) {
            Destroy(gameObject);
        } else {
            _flicker = true;
            _currentTime = 0;
            _collider.enabled = false;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(new Vector3(leftBound, 0, 0), new Vector3(leftBound, 2, 0));
        Gizmos.DrawLine(new Vector3(rightBound, 0, 0), new Vector3(rightBound, 2, 0));
    }
}
