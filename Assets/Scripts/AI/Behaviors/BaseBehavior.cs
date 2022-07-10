using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Cataclyst.AI.Behaviors
{
    [RequireComponent(typeof(PathFinding.PathFinder))]
    [RequireComponent(typeof(PathFinding.Entity))]
    internal class BaseBehavior : SerializedMonoBehaviour
    {
        internal AI.BaseAI AIBase;
        public PathFinding.PathFinder PathFinder;
        public PathFinding.Entity Entity;
        public UnityEvent<CompletionType> Finished;
        public void Awake()
        {
            AIBase = GetComponent<AI.BaseAI>();
            PathFinder = GetComponent<PathFinding.PathFinder>();
            Entity = GetComponent<PathFinding.Entity>();
            PathFinding.Node currentNode = GetCurrentNode();
        }
        public void Start()
        {
            PathFinding.Node currentNode = GetCurrentNode();
            if (currentNode is not null)
            {
                Entity.Position = currentNode.Position;
            }
        }
        [Button("Tick")]
        public virtual void Tick()
        {
            Finished.Invoke(CompletionType.Error);
        }

        [Button]
        public PathFinding.Node GetCurrentNode()
        {
            var hits = Physics.RaycastAll(transform.position + new Vector3(0, 64, 0), -Vector3.up, 128);
            Debug.DrawLine(transform.position + new Vector3(0, 64, 0), transform.position - new Vector3(0, 64, 0), Color.red, 10000);
            try
            {
                var node = hits.Where(n => n.transform.tag == "Unit").First().transform.gameObject.GetComponent<PathFinding.Unit>()?.Node ?? null;
                return node;
            }
            catch (System.Exception e) 
            { 
                Debug.LogException(e);
                return null; 
            }
        }
    }

    public enum CompletionType : byte
    {
        Incomplete,
        FullCompletion,
        PartialCompletion,
        Cancelled,
        Error
    }

}
