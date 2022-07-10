using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cataclyst.AI.Behaviors
{
    internal class Travel : BaseBehavior
    {
        public PathFinding.Node target;
        private Vector3? _startingWorldPosition = null, _finalWorldPosition = null;
        private Vector2Int _newPosition;
        private int _updateCounter;
        private float Progress => _updateCounter / (float)AIBase.TickRate;

        [Button]
        public override void Tick()
        {
            var path = PathFinder.FindPath(Entity.Position, target.Position);
            if (path == null)
            {
                //no path
                Finished.Invoke(CompletionType.Error);
                return;
            }
            //finished :)
            else if (path.Count == 1)
            {
                Finished.Invoke(CompletionType.FullCompletion);
                return;
            }

            _startingWorldPosition = transform.position;
            _finalWorldPosition = path[0].WorldPosition;
            _newPosition = path[0].Position;
            _updateCounter = 0;
        }

        public void FixedUpdate()
        {
            if (_startingWorldPosition is null || _finalWorldPosition is null)
            {
                return;
            }
            if (Mathf.Approximately(transform.position.x, _finalWorldPosition.Value.x) && Mathf.Approximately(transform.position.z, _finalWorldPosition.Value.z) || (_updateCounter > AIBase.TickRate))
            {
                transform.position = new Vector3(_finalWorldPosition.Value.x, transform.position.y, _finalWorldPosition.Value.z);
                Entity.Position = _newPosition;
                _startingWorldPosition = null; _finalWorldPosition = null;
                _updateCounter = 0;
            }
            else
            {
                transform.position = Vector3.Lerp(new Vector3(_startingWorldPosition.Value.x, transform.position.y, _startingWorldPosition.Value.z), new Vector3(_finalWorldPosition.Value.x, transform.position.y, _finalWorldPosition.Value.z), Progress);
                _updateCounter++;
            }
        }
    }
}
