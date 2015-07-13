using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
    public Rect border = new Rect(1, 3, 10, 5);
    public GameObject grid;
    private Grid _grid;
    
    void Awake()
    {
        var g = Instantiate(grid);
        g.transform.SetParent(transform, false);
        g.transform.position = new Vector3(border.x, border.y + border.height, 0);
        _grid = g.GetComponent<Grid>();
    }
    
    void Update()
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
