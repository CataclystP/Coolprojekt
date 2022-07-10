using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cataclyst.AI.Behaviors
{
    internal class Flank : BaseBehavior
    {
        public PathFinding.Entity target;

        public override void Tick()
        {
            PathFinder.FindPath(Entity.Position, target.Position);

            Finished.Invoke(CompletionType.Error);
        }
    }
}
