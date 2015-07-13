using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    [SerializeField] private float shotInterval = 3;
    [SerializeField] private int burstPerShot;
    [SerializeField] private Transform bulletStartPosition;
    [SerializeField] private Bullet bullet;
    
    private Grid _grid;
    
    public void Init(Grid grid)
    {
        _grid = grid;
    }
    
    public void internalUpdate()
    {
        
    }
}
