using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
    public Rect border = new Rect(1, 3, 10, 5);
    public GameObject grid;
    [SerializeField] private Transform[] _lifeIndicators;
    private Grid _grid;
    public int currentLifes { get; private set; }
    
    void Awake()
    {
        currentLifes = _lifeIndicators.Length;
        var g = Instantiate(grid);
        g.transform.SetParent(transform, false);
        g.transform.position = new Vector3(border.x, border.y + border.height, 0);
        _grid = g.GetComponent<Grid>();
    }
    
    void Update()
    {
    }
    
    public void subtractLife()
    {
        currentLifes -= 1;
        for (int i = 0; i < _lifeIndicators.Length; i++) {
            if (_lifeIndicators[i] != null) {
                Destroy(_lifeIndicators[i].gameObject);
                break;
            }
        }
        
        if (currentLifes == -1) {
            end();
        }
    }
    
    public void end()
    {
    
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
