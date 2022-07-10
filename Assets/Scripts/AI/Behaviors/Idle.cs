using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cataclyst.AI.Behaviors
{
    internal class Idle : BaseBehavior
    {

        public override void Tick()
        {
            Finished.Invoke(CompletionType.Error);
        }
    }
}
