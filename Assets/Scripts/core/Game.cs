using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
    public Rect border = new Rect(1, 3, 10, 5);
    public Audio audio;
    [SerializeField] private GameObject grid;
    [SerializeField] private Transform[] _lifeIndicators;
    [SerializeField] private Text _scoreLabel;
    
    private Grid _grid;
    public int currentLifes { get; private set; }
    
    private int _score = 0;
    
    void Awake()
    {
        currentLifes = _lifeIndicators.Length;
        var g = Instantiate(grid);
        g.transform.SetParent(transform, false);
        g.transform.position = new Vector3(border.x, border.y + border.height, 0);
        _grid = g.GetComponent<Grid>();
    }
    
    public void subtractLife()
    {
        addScore(-120);

        currentLifes -= 1;
        for (int i = 0; i < _lifeIndicators.Length; i++) {
            if (_lifeIndicators[i] != null) {
                Destroy(_lifeIndicators[i].gameObject);
                _lifeIndicators[i] = null;
                break;
            }
        }
        
        if (currentLifes == 0) {
            end();
        }
    }
    
    public void end()
    {
        Debug.LogWarning("END GAME");
    }
    
    public void addScore(int score) 
    {
        _score = Mathf.Max(0, _score + score);
        
        if (_score == 0) {
            _scoreLabel.text = "0000";
        } else if (_score < 100) {
            _scoreLabel.text = "00" + _score.ToString();
        } else if (_score < 1000) {
            _scoreLabel.text = "0" + _score.ToString();
        } else {
            _scoreLabel.text = _score.ToString();
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(border.x, border.y, 0), new Vector3(border.x + border.width, border.y, 0));
        Gizmos.DrawLine(new Vector3(border.x + border.width, border.y, 0), new Vector3(border.x + border.width, border.y + border.height, 0));
        Gizmos.DrawLine(new Vector3(border.x + border.width, border.y + border.height, 0), new Vector3(border.x, border.y + border.height, 0));
        Gizmos.DrawLine(new Vector3(border.x, border.y + border.height, 0), new Vector3(border.x, border.y, 0));
    }
}
