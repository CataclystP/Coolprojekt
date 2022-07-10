using Sirenix.OdinInspector;
using Cataclyst.AI.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cataclyst.AI
{
    public class BaseAI : SerializedMonoBehaviour
    {
        public PathFinding.Grid NavigationGrid;
        [Title("Navigation State")]
        [EnumToggleButtons, HideLabel]
        public NavigationMode NavigationState;
        public int TickRate;
        BaseBehavior _currentBehavior;
        public bool Frozen = false;
        public bool SelfTick = false;
        public uint UpdateCount = 0;

        [Button("Tick")]

        private void Awake()
        {
            StateChange(NavigationState);
        }
        private void Start()
        {
            StateChange(NavigationState);
        }

        public void Tick()
        {
            _currentBehavior.Tick();
        }

        public void BehaviorCallback(CompletionType remark)
        {
            switch (NavigationState)
            {
                case NavigationMode.None:
                    break;
                case NavigationMode.Idle:
                    break;
                case NavigationMode.Patrol:
                    //handle stages transitions here
                    break;
                case NavigationMode.Travel:
                    break;
                case NavigationMode.Follow:
                    break;
                case NavigationMode.Flee:
                    break;
                case NavigationMode.Flank:
                    break;
                case NavigationMode.Goto:
                    break;
            }
        }

        public void StateChange(NavigationMode newState)
        {
            switch (newState)
            {
                case NavigationMode.None:
                    Destroy(_currentBehavior);
                    break;
                case NavigationMode.Idle:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Idle>();
                    break;
                case NavigationMode.Patrol:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Patrol>();
                    break;
                case NavigationMode.Travel:
                     Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Travel>();
                    break;
                case NavigationMode.Follow:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Follow>();
                    break;
                case NavigationMode.Flee:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Flee>();
                    break;
                case NavigationMode.Flank:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Flank>();
                    break;
                case NavigationMode.Goto:
                    Destroy(_currentBehavior);
                    _currentBehavior = gameObject.AddComponent<Goto>();
                    break;
            }
        }

        public void FixedUpdate()
        {
            if (UpdateCount >= TickRate)
            {
                Tick();
            }
            UpdateCount = UpdateCount > TickRate? 0 : UpdateCount + 1;
        }

        public T GetorInstantiateComponent<T>()
            where T : Component
        {
            T component;
            TryGetComponent<T>(out component);
            if (component is not null)
            {
                return component;
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }
    }
    public enum NavigationMode : short
    {
        /// <summary>
        /// No movement or logic
        /// </summary>
        None,

        /// <summary>
        /// No movement, idle logic
        /// </summary>
        Idle,

        /// <summary>
        /// Travel to a unit
        /// </summary>
        Travel,

        /// <summary>
        /// Travel to an entity
        /// </summary>
        Goto,

        /// <summary>
        /// Move between units, with idle stages in between
        /// </summary>
        Patrol,

        /// <summary>
        /// Continously move towards an entity but always stay a distance away from it
        /// </summary>
        Follow,

        /// <summary>
        /// Run away from an entity
        /// </summary>
        Flee,

        /// <summary>
        /// Using other entities, take a path not taken by other entities towards a target entity/unit
        /// </summary>
        Flank
    }
}
