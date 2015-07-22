using UnityEngine;
using System.Collections;
using graphics;

public class Enemy : MonoBehaviour {
    
    [SerializeField] private float minimumInterval = 2;
    [SerializeField] private float shotInterval = 2;
    [SerializeField] private int burstPerShot = 1;
    [SerializeField] private float burstDelay = 0.5f;
    [SerializeField] private Transform bulletStartPosition = null;
    [SerializeField] private Bullet bullet = null;
    [SerializeField] private GameObject explosionPrefab = null;
    [SerializeField] private Transform explosionPoint = null;
    
    [SerializeField] private float _shakeAmount = 0.1f;
    [SerializeField] private float _shakeTime = 0.2f;
    
    [SerializeField] private int score = 0;

    private Game _game;   
    private Grid _grid;
    private float _currentTime = 0;
    private float _nextShotTime;
    private int _x;
    private int _y;
    private int _currentBurstCount;
    private float _currentBurstTime = -1f;
    
    public void Init(Game game, Grid grid, int x, int y)
    {
        _game = game;
        
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
            _game.soundSystem.playShoot();
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
        if (col.collider.GetComponent<Shield>() != null) {
            transform.localPosition = new Vector3(_x, -_y, 0);
            return;
        }
        if (col.collider.GetComponent<Bullet>() == null) return;
        if (_y < 2 && _grid.isEnemyAlive(_x, _y+1)) return;
        
        _game.addScore(score);
        _game.shake.play(_shakeAmount, _shakeTime);
        _game.blur.play(BlurType.SOFT);
        explode();
    }
    
    void explode()
    {
        _game.soundSystem.playExplosion();
        _grid.removeEnemy(_x, _y);
        var exp = Instantiate(explosionPrefab);
        exp.transform.position = explosionPoint.position;
        Destroy(gameObject);
    }
    
    public void end()
    {
        explode();
    }
}
