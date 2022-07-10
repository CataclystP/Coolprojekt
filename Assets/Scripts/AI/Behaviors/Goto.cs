using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cataclyst.AI.Behaviors
{
    internal class Goto : BaseBehavior
    {
        public PathFinding.Entity target;
        private Vector3 _startingPosition, _finalPosition;
        private int _updateCounter;
        private float Progress => _updateCounter / (float) AIBase.TickRate;

        public override void Tick()
        {
            var path = PathFinder.FindPath(Entity.Position, target.Position);
            if (path != null)
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

            _startingPosition = path[0].WorldPosition;
            _finalPosition = path[1].WorldPosition;
            _updateCounter = 0;
        }

        public void FixedUpdate()
        {
            if (Mathf.Approximately(transform.position.x, _finalPosition.x) && Mathf.Approximately(transform.position.z, _finalPosition.z)) {
                transform.position = new Vector3(_finalPosition.x, transform.position.y, _finalPosition.z);
                _updateCounter = 0;
            }
            else
            {
                transform.position = Vector3.Lerp(new Vector3(_startingPosition.x, transform.position.y, _startingPosition.z), new Vector3(_finalPosition.x, transform.position.y, _finalPosition.z), Progress);
                _updateCounter++;
            }
        }
    }
}
