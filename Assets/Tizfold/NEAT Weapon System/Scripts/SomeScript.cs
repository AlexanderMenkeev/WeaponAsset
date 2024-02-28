    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SomeScript : MonoBehaviour {
    [SerializeField] private Texture2D _skinTex;
    
    // void Start()
    // {
    //     int randomX = Random.Range(0, 1);
    //     int randomY = Random.Range(0, 3);
    //     _skinTex.SetPixel(0, randomY, Random.ColorHSV());
    //     _skinTex.Apply();
    // }
    //
    // void Update()
    // {
    //     int randomX = Random.Range(0, 2);
    //     int randomY = Random.Range(0, 4);
    //     _skinTex.SetPixel(randomX, randomY, Random.ColorHSV());
    //     _skinTex.Apply();
    // }

    public Tilemap tilemap;
    public Grid grid;
    public Tilemap sourceTilemap;

    private void SomeMethod() {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        Debug.Log(tilemap.origin);
        
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null) {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                } else {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    
                }
            }
        }        
    }
    private void Start() {
        SomeMethod();
    }
}
