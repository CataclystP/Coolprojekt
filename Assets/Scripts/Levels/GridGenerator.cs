using Cataclyst.PathFinding;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static Cataclyst.PathFinding.Node;

namespace Cataclyst.Levels
{
    public class GridGenerator : SerializedMonoBehaviour
    {
        [HideInInspector] public GameObject[,] gridObjects;

        [SerializeField] private GameObject _tileAirPrefab, _tileBlockPrefab;
        [SerializeField] private Vector2 _spacing, _blockSize;
        [SerializeField] private PathFinding.Grid _grid;
        private Node[,] Nodes => _grid.Nodes;

        [Button("Generate Grid")]
        private void Generate()
        {
            if (_grid is null || Nodes is null) return;

            Clear();
            int airCount = 0; int obstacleCount = 0;
            for (int y = 0; y < _grid.Dimensions.y; y++)
            {
                for (int x = 0; x < _grid.Dimensions.x; x++)
                {
                    GameObject tileObject;
                    if (Nodes[x, y].Properties.HasFlag(NodeProperty.Walkable))
                    {
                        tileObject = Instantiate(_tileAirPrefab, transform);
                        tileObject.name = "Air " + (airCount++);
                    }
                    else
                    {
                        tileObject = Instantiate(_tileBlockPrefab, transform);
                        tileObject.name = "Wall " + (obstacleCount++);
                    }
                    tileObject.transform.position = new Vector3(x * _blockSize.x + transform.position.x , transform.position.z, y * _blockSize.y + transform.position.y);
                    tileObject.GetComponent<Unit>().Node = Nodes[x, y];
                    Nodes[x, y].WorldPosition = tileObject.transform.position;
                    gridObjects[x, y] = tileObject;
                }
            }
        }

        [Button("Clear Grid")]
        private void Clear()
        {
            if (_grid is null || _grid.Nodes is null) return;
            gridObjects = new GameObject[_grid.Dimensions.x, _grid.Dimensions.y];
            int countDestroyed = 0;
            int childCount = transform.childCount;
            List<Transform> children = new List<Transform>();

            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            if (Application.isPlaying)
            {
                foreach (Transform child in children)
                {
                    countDestroyed++;
                    Destroy(child.gameObject);
                }
            }
            else
            {
                foreach (Transform child in children)
                {
                    countDestroyed++;
                    DestroyImmediate(child.gameObject);
                }
            }
            Debug.Log($"destroyed {countDestroyed} children out of {childCount}");
        }
    }
}
