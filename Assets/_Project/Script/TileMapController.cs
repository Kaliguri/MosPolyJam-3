using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    [SerializeField] public List<Tilemap> tilemapList = new();

    public static TileMapController instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
