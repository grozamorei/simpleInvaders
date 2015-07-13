using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
    public int gridWidth = 6;
    public float[] updateFrequency = new float[] {0.8f, 0.4f, 0.2f, 0.1f};
    public float[] updateStep = new float[] {0.25f, 0.2f, 0.2f, 0.1f};
    public Enemy[] enemiesPerLine;
    public int instantShots = 2;
    
    private Game _game;
    private List<List<Enemy>> _grid;
    private int _phase = 0;
    private int _direction = 1;
    private float _currentTime = 0;
    private int _currentShots = 0;
    
    public void Awake()
    {
        _game = FindObjectOfType<Game>();
        
        var w = gridWidth;
        var h = enemiesPerLine.Length;
        _grid = new List<List<Enemy>>();
        
        for (int i = 0; i < w; i++) {
            _grid.Add(new List<Enemy>());
            for (int j = 0; j < h; j++) {
                var e = Instantiate(enemiesPerLine[j]);
                e.GetComponent<Enemy>().Init(this);
                _grid[i].Add(e);
                e.transform.SetParent(transform);
                e.transform.localPosition = new Vector3(i, -j, 0);
            }
        }
    }
    
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= updateFrequency[_phase]) {
            _currentTime = 0;
            
            int w = _grid.Count;
            var p = transform.position;
            if (_direction == 1) {
                if (transform.position.x + _grid.Count-1 >= _game.border.x + _game.border.width) {
                    transform.position = new Vector3(p.x, p.y - 0.5f, p.z);
                    _direction = -1;
                } else {
                    transform.position = new Vector3(p.x + updateStep[_phase], p.y, p.z);
                }
            } else {
                if (transform.position.x <= _game.border.x) {
                    transform.position = new Vector3(p.x, p.y - 0.5f, p.z);
                    _direction = 1;
                } else {
                    transform.position = new Vector3(p.x - updateStep[_phase], p.y, p.z);
                }
            }
        }
        
        for (int i = 0; i < _grid.Count; i++) {
            for (int j = enemiesPerLine.Length-1; j >= 0; j--) {
                if (_grid[i][j] == null) continue;
                if (_grid[i][j].internalUpdate(_currentShots == instantShots)) {
                    _currentShots += 1;
                }
                break;
            }
        }
        if (_currentShots == instantShots) {
            _currentShots = 0;
        }
    }
}
