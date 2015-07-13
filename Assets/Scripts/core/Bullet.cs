using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    
    [SerializeField] private float colorLerpSpeed = 10;
    [SerializeField] private Color color1 = new Color(0.8f, 0, 0, 0.8f);
    [SerializeField] private Color color2 = new Color(0, 0.8f, 0, 0.8f);
    [SerializeField] private Color color3 = new Color(0, 0, 0.8f, 0.8f);
    [SerializeField] private Color color4 = new Color(0, 0, 0.8f, 0.8f);
    
    private float _currentT = 0;
    private Color _currentC;
    private Color _nextC;
    
    void Awake()
    {
        _currentC = color1;
        _nextC = color2;
    }

    void Update()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - speed * Time.deltaTime, pos.z);
        
        if (_currentT <= 1) {
            _currentT += Time.deltaTime * colorLerpSpeed;
            var c = Color.Lerp(_currentC, _nextC, _currentT);
            GetComponent<SpriteRenderer>().material.SetColor("_Color", c);
        } else {
            _currentT = 0;
            if (_nextC == color1) {
                _currentC = color1; _nextC = color2;
            } else if (_nextC == color2) {
                _currentC = color2; _nextC = color3;
            } else if (_nextC == color3) {
                _currentC = color3; _nextC = color4;
            } else if (_nextC == color4) {
                _currentC = color4; _nextC = color1;
            }
        }
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
