using Sirenix.OdinInspector;
using UnityEngine;
using static Cataclyst.PathFinding.Node;
using UnityEditor;
using Sirenix.Utilities;

namespace Cataclyst.PathFinding
{
    [CreateAssetMenu(fileName = "Grid", menuName = "Pathfinding/Pathfinding Grid", order = 1)]
    public class Grid : SerializedScriptableObject
    {

        [TableMatrix(DrawElementMethod = "DrawGrid")]
        public Node[,] Nodes;
        public Vector2Int Dimensions;

        //[ReadOnly]
        //public int CountAir()
        //{
        //    int count = 0;
        //    foreach (var tile in Nodes)
        //    {
        //        if (tile.Properties.HasFlag(NodeProperty.Walkable)) count++;
        //    }
        //    return count;
        //}

        //[ReadOnly]
        //public int CountObstacles()
        //{
        //    int count = 0;
        //    foreach (var tile in Nodes)
        //    {
        //        if (!tile.Properties.HasFlag(NodeProperty.Walkable)) count++;
        //    }
        //    return count;
        //}

        private static Node DrawGrid(Rect rect, Node value)
        {
            if (Event.current.type is EventType.MouseDown or EventType.MouseDrag && rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.button == 0)
                {
                    value.Properties = NodeProperty.None;
                    GUI.changed = true;
                    Event.current.Use();
                }
                else if (Event.current.button == 1)
                {
                    value.Properties = NodeProperty.Walkable;
                    GUI.changed = true;
                    Event.current.Use();
                }
            }

            Color color;

            if (value.Properties.HasFlag(NodeProperty.Walkable))
            {
                ColorUtility.TryParseHtmlString("#FFFFFF", out color);
            }
            else
            {
                ColorUtility.TryParseHtmlString("#1F366F", out color);
            }
            EditorGUI.DrawRect(rect.Padding(1), color);

            return value;
        }

        // Start is called before the first frame update

        [Button]
        private void Initialise()
        {
            Nodes = new Node[Dimensions.x, Dimensions.y];


            for (int x = 0; x < Dimensions.x; x++)
            {
                for (int y = 0; y < Dimensions.y; y++)
                {
                    Nodes[x, y] = new Node
                    {
                        Position = new Vector2Int(x, y),
                        Properties = NodeProperty.Walkable
                    };
                }
            }
        }
    }
}
