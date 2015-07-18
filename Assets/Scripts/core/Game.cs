using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using social;
using util;
using graphics;


public class Game : MonoBehaviour 
{
    public Rect border = new Rect(1, 3, 10, 5);
    public Audio soundSystem;
    [SerializeField] private GameObject grid = null;
    [SerializeField] private Transform[] _lifeIndicators = null;
    [SerializeField] private Text _scoreLabel = null;
    [SerializeField] private Text _startLabel = null;
    [SerializeField] private EndScreenMediator _endScreen = null;
    
    private Grid _grid;
    public int currentLifes { get; private set; }

    public Blur blur {get; private set;}

    public CameraShake shake {get; private set;}
    public Player player {get; private set;}
    
    public bool started { get; private set; }
    private float _currentTime = 0;
    private int _flickerTime = 0;
    
    private int _score = 0;
    private bool _ended = false;
    
    void Awake()
    {
        started = false;
        blur = Camera.main.GetComponent<Blur>();
        shake = Camera.main.GetComponent<CameraShake>();
        player = FindObjectOfType<Player>();
    }
    
    void Update()
    {
        if (started) return;

        if (_currentTime >= 0.5f) {
            _startLabel.enabled = !_startLabel.enabled;
            _currentTime = 0;
            _flickerTime += 1;
            if (_flickerTime > 6) {
                started = true;
                _startLabel.enabled = false;
                
                currentLifes = _lifeIndicators.Length;
                var g = Instantiate(grid);
                g.transform.SetParent(transform, false);
                g.transform.position = new Vector3(border.x, border.y + border.height, 0);
                _grid = g.GetComponent<Grid>();
            }
        }
        _currentTime += Time.deltaTime;
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
        
        if (currentLifes <= 0) {
            end();
        }
    }
    
    public void end()
    {
        if (_ended) return;
        _ended = true;
        _grid.end();
        player.end();
        StartCoroutine(showEndScreen());
    }
    
    public void addScore(int score) 
    {
        _score = Mathf.Max(0, _score + score);
        _scoreLabel.text = Score.formatToStr(_score);
    }
    
    IEnumerator showEndScreen()
    {
        yield return new WaitForSeconds(2);
        _endScreen.init(currentLifes > 0, _score, _scoreLabel.text);
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
