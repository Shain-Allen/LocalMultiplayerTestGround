using System.Collections.Generic;
using Singletons;
using UnityEngine;

public class PlaceablesManager : Singleton<PlaceablesManager>
{
    public Grid _grid;
    private Dictionary<Vector3Int, IPlaceable> _gridObjects;

    private new void Awake()
    {
        base.Awake();
        _grid = GetComponent<Grid>();
        _gridObjects = new Dictionary<Vector3Int, IPlaceable>();
    }
}