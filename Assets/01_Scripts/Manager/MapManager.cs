using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap _currentTilemap;
    public Tilemap CurrentTileMap => _currentTilemap;

    /// <summary>
    /// 초기화
    /// </summary>
    public void Initialize()
    {
        if(_currentTilemap == null)
        {
            _currentTilemap = GameObject.FindWithTag(Constants.TAG_Ground).GetComponent<Tilemap>();
        }
    }
}
