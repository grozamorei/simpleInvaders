using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    [SerializeField] private float minimumInterval = 2;
    [SerializeField] private float shotInterval = 2;
    [SerializeField] private int burstPerShot;
    [SerializeField] private Transform bulletStartPosition;
    [SerializeField] private Bullet bullet;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionPoint;
    
    private Grid _grid;
    private float _currentTime = 0;
    private float _nextShotTime;
    private int _x;
    private int _y;
    
    public void Init(Grid grid, int x, int y)
    {
        _x = x; _y = y;
        _grid = grid;
        _nextShotTime = Random.Range(minimumInterval, minimumInterval + shotInterval);
    }
    
    public bool internalUpdate(bool resetCounter)
    {
        if (resetCounter) {
            _currentTime = 0;
            _nextShotTime = Random.Range(minimumInterval, minimumInterval + shotInterval);
            return false;
        }
        
        _currentTime += Time.deltaTime;
        if (_currentTime >= _nextShotTime) {
            _currentTime = 0;
            _nextShotTime = Random.Range(minimumInterval, minimumInterval + shotInterval);
            var b = Instantiate(bullet);
            b.transform.position = bulletStartPosition.position;
            return true;
        }
        
        return false;
    }
    
    void OnCollisionEnter2D (Collision2D col)
    {
        _grid.removeEnemy(_x, _y);
        var exp = Instantiate(explosionPrefab);
        exp.transform.position = explosionPoint.position;
        Destroy(gameObject);
    }
}
