using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
    public int gridWidth = 6;
    public float[] updateFrequency = new float[] {0.8f, 0.4f, 0.2f, 0.1f, 0.1f};
    public float[] updateStep = new float[] {0.25f, 0.2f, 0.2f, 0.1f, 0.1f};
    public Enemy[] enemiesPerLine;
    public int instantShots = 2;
    
    private Game _game;
    private List<List<Enemy>> _grid;
    private float xOffset = 0;
    private float currentWidth;
    
    private int _phase = 0;
    private int _direction = 1;
    private float _currentTime = 0;
    private int _currentShots = 0;
    
    private float _enemiesLeft;
    private float _enemiesTotal;
    
    public void Awake()
    {
        _game = FindObjectOfType<Game>();
        
        var w = gridWidth;
        currentWidth = w;
        var h = enemiesPerLine.Length;
        _enemiesLeft = _enemiesTotal = w * h;
        _grid = new List<List<Enemy>>();
        
        for (int i = 0; i < w; i++) {
            _grid.Add(new List<Enemy>());
            for (int j = 0; j < h; j++) {
                var e = Instantiate(enemiesPerLine[j]);
				e.GetComponent<Enemy>().Init(_game, this, i, j);
                _grid[i].Add(e);
                e.transform.SetParent(transform);
                e.transform.localPosition = new Vector3(i, -j * 1.1f, 0);
            }
        }
    }
    
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= updateFrequency[_phase]) {
            _currentTime = 0;
            
            var p = transform.position;
            if (_direction == 1) {
                if (transform.position.x + xOffset + currentWidth-1 >= _game.border.x + _game.border.width) {
                    transform.position = new Vector3(p.x, p.y - 0.5f, p.z);
                    _direction = -1;
                } else {
                    transform.position = new Vector3(p.x + updateStep[_phase], p.y, p.z);
                }
            } else {
                if (transform.position.x + xOffset <= _game.border.x) {
                    transform.position = new Vector3(p.x, p.y - 0.5f, p.z);
                    _direction = 1;
                } else {
                    transform.position = new Vector3(p.x - updateStep[_phase], p.y, p.z);
                }
            }

            if (transform.position.y < _game.border.y) {
                while(_game.currentLifes > 0) {
                    _game.subtractLife();
                }
                _game.end();
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
    
    public bool isEnemyAlive(int x, int y) {
        return _grid[x][y] != null;
    }
    
    public void removeEnemy(int x, int y)
    {
        _grid[x][y] = null;
        
        for (int i = 0; i < _grid.Count; i++) {
            if (_grid[i][0] != null) {
                xOffset = i;
                break;
            }
        }
        
        for (int i = 0; i < _grid.Count; i++) {
            if (_grid[_grid.Count - 1 - i][0] != null) {
                currentWidth = _grid.Count - xOffset - i;
                break;
            }
        }
        
        _enemiesLeft -= 1;
        if (_enemiesLeft == 0) {
            _game.end();
        }
        var ratio = _enemiesLeft / _enemiesTotal;
        
        switch(_phase) {
            case 0:
                if (ratio <= 0.8f)
                    _phase = 1;
            break;
            case 1:
                if (ratio <= 0.6f)
                    _phase = 2;
            break;
            case 2:
                if (ratio <= 0.4f)
                    _phase = 3;
            break;
            case 3:
            if (ratio <= 0.2f)
                _phase = 4;
            break;
        }
        if (_phase == 4 && _enemiesLeft == 1) {
            _phase = 5;
        }
    }
    
    public void end()
    {
        var w = gridWidth;
        var h = enemiesPerLine.Length;
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                if (_grid[i][j] == null) continue;
                _grid[i][j].end();
            }
        }
    }
}
