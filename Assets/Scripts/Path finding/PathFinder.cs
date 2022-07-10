using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static Cataclyst.PathFinding.Node;

namespace Cataclyst.PathFinding
{
    public class PathFinder : SerializedMonoBehaviour
	{
		[SerializeField] private Grid _grid;
		private Node[,] Nodes => _grid.Nodes;

		private Dictionary<Node, NodeInformation> _nodeInfo = new Dictionary<Node, NodeInformation>();

		private Vector2Int GridDimensions
		{
			get
			{
				return new Vector2Int(Nodes.GetLength(0), Nodes.GetLength(1));
			}
		}
		public Vector2Int RouteStart, RouteEnd;

		public List<Node> GetNeighbours(Node node)
		{
			List<Node> neighbours = new List<Node>();
			Node[] cardinalNeighbours = new Node[4];

			var nodePositionX = node.Position.x; var nodePositionY = node.Position.y;

			if ((nodePositionX - 1) >= 0 && (nodePositionX - 1) < GridDimensions.x && (nodePositionY) >= 0 && (nodePositionY) < GridDimensions.y)
			{
				if (Nodes[nodePositionX - 1, nodePositionY].Properties.HasFlag(NodeProperty.Walkable))
				{
					cardinalNeighbours[0] = Nodes[nodePositionX - 1, nodePositionY]; //leftmost
					neighbours.Add(Nodes[nodePositionX - 1, nodePositionY]);
				}
			}
			if ((nodePositionX) >= 0 && (nodePositionX) < GridDimensions.x && (nodePositionY + 1) >= 0 && (nodePositionY + 1) < GridDimensions.y)
			{
				if (Nodes[nodePositionX, nodePositionY + 1].Properties.HasFlag(NodeProperty.Walkable))
				{
					cardinalNeighbours[1] = Nodes[nodePositionX, nodePositionY + 1]; //topmost
					neighbours.Add(Nodes[nodePositionX, nodePositionY + 1]);
				}

			}
			if ((nodePositionX + 1) >= 0 && (nodePositionX + 1) < GridDimensions.x && (nodePositionY) >= 0 && (nodePositionY) < GridDimensions.y)
			{
				if (Nodes[nodePositionX + 1, nodePositionY].Properties.HasFlag(NodeProperty.Walkable))
				{
					cardinalNeighbours[2] = Nodes[nodePositionX + 1, nodePositionY]; //rightmost
					neighbours.Add(Nodes[nodePositionX + 1, nodePositionY]);
				}
			}
			if ((nodePositionX) >= 0 && (nodePositionX) < GridDimensions.x && (nodePositionY - 1) >= 0 && (nodePositionY - 1) < GridDimensions.y)
			{
				if (Nodes[nodePositionX, nodePositionY - 1].Properties.HasFlag(NodeProperty.Walkable))
				{
					cardinalNeighbours[2] = Nodes[nodePositionX, nodePositionY - 1]; //bottommost
					neighbours.Add(Nodes[nodePositionX, nodePositionY - 1]);
				}
			}

			return neighbours;
		}

		public List<Node> FindPath(Vector2Int startPos, Vector2Int targetPos)
		{
			_nodeInfo.Clear();

			Node startNode = Nodes[startPos.x, startPos.y];
			Node targetNode = Nodes[targetPos.x, targetPos.y];

			List<Node> openSet = new();
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node node = openSet[0];
				if (!_nodeInfo.ContainsKey(node)) _nodeInfo.Add(node, new NodeInformation());
				for (int i = 1; i < openSet.Count; i++)
				{
					if (!_nodeInfo.ContainsKey(openSet[i])) _nodeInfo.Add(openSet[i], new NodeInformation());
					if (_nodeInfo[openSet[i]].FCost < _nodeInfo[node].FCost || _nodeInfo[openSet[i]].FCost == _nodeInfo[node].FCost)
					{
						if (_nodeInfo[openSet[i]].HCost < _nodeInfo[node].HCost)
							node = openSet[i];
					}
				}

				openSet.Remove(node);
				closedSet.Add(node);

				if (node == targetNode)
				{
					Debug.Log(_nodeInfo.Count + "nodes checked");
					return RetracePath(startNode, targetNode);
				}

				foreach (Node neighbour in GetNeighbours(node))
				{
					if (closedSet.Contains(neighbour))
					{
						continue;
					}
					if (!_nodeInfo.ContainsKey(neighbour)) _nodeInfo.Add(neighbour, new NodeInformation());


					int newCostToNeighbour = _nodeInfo[node].GCost + GetDistance(node, neighbour);
					if (newCostToNeighbour < _nodeInfo[neighbour].GCost || !openSet.Contains(neighbour))
					{
						var newNeighbour = _nodeInfo[neighbour];
						newNeighbour.GCost = newCostToNeighbour;
						newNeighbour.HCost = GetDistance(neighbour, targetNode);
						newNeighbour.parent = node;
						_nodeInfo[neighbour] = newNeighbour;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
			return null;
		}

		public List<Node> RetracePath(Node startNode, Node endNode)
		{
			List<Node> newPath = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				newPath.Add(currentNode);
				currentNode = _nodeInfo[currentNode].parent;
			}
			newPath.Reverse();

			return newPath;

		}

		public int GetDistance(Node nodeA, Node nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
			int dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}
	}
}