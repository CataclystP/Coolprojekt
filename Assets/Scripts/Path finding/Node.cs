using System;
using UnityEngine;

namespace Cataclyst.PathFinding
{
	public class Node
	{
		public Vector2Int Position;
		public Vector3 WorldPosition;
		public NodeProperty Properties;

		public int CompareTo(NodeInformation firstNodeCost, NodeInformation targetNodeCost)
		{
			int compare = firstNodeCost.FCost.CompareTo(targetNodeCost.FCost);
			if (compare == 0)
			{
				compare = firstNodeCost.HCost.CompareTo(targetNodeCost.HCost);
			}
			return -compare;
		}

		[Flags]
		public enum NodeProperty : short
		{
			None = 0x0,
			Walkable = 0x1,
			Swimable = 0x2,
			Flyable = 0x4,
			Rideable = 0x8,

			Slowdown = 0x20,
			Speedup = 0x40,
		}

		public struct NodeInformation
		{
			public int GCost;
			public int HCost;
			public int FCost => GCost + HCost;

			public Node parent;
		}
	}
}
