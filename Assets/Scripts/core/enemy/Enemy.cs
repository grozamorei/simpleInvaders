using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    [SerializeField] private float minimumInterval = 2;
    [SerializeField] private float shotInterval = 2;
    [SerializeField] private int burstPerShot = 1;
    [SerializeField] private float burstDelay = 0.5f;
    [SerializeField] private Transform bulletStartPosition = null;
    [SerializeField] private Bullet bullet = null;
    [SerializeField] private GameObject explosionPrefab = null;
    [SerializeField] private Transform explosionPoint = null;
    
    private Grid _grid;
    private float _currentTime = 0;
    private float _nextShotTime;
    private int _x;
    private int _y;
    private int _currentBurstCount;
    private float _currentBurstTime = -1f;
    
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
            _currentBurstCount = burstPerShot;
            return true;
        }
        
        return false;
    }
    
    void Update()
    {
        if (_currentBurstCount == 0) return;
        
        if (_currentBurstTime <= 0) {
            var b = Instantiate(bullet);
            b.transform.position = bulletStartPosition.position;
            _currentBurstTime = burstDelay;
            _currentBurstCount -= 1;
        } else {
            _currentBurstTime -= Time.deltaTime;
        }
    }
    
    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.collider.GetComponent<Bullet>() == null) return;
        _grid.removeEnemy(_x, _y);
        var exp = Instantiate(explosionPrefab);
        exp.transform.position = explosionPoint.position;
        Destroy(gameObject);
    }
}
