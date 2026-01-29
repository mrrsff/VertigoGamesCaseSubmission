using System;
using System.Collections.Generic;

namespace SpinGameDemo.Context
{
    public class ContextContainer
    {
        public List<IContextUnit> Units { get; } = new List<IContextUnit>();
        private List<IContextBehaviour> _behaviours = new List<IContextBehaviour>();

        private Dictionary<Type, IContextUnit> _unitTypeMap = new Dictionary<Type, IContextUnit>();

        public void Add(IContextUnit unit)
        {
            Units.Add(unit);
            if (unit is IContextBehaviour behaviour)
            {
                _behaviours.Add(behaviour);
            }

            _unitTypeMap[unit.GetType()] = unit;
        }

        public void Initialize()
        {
            foreach (var unit in Units)
            {
                unit.Initialize();
            }
        }

        public void Update()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.Update();
            }
        }

        public void Dispose()
        {
            foreach (var unit in Units)
            {
                unit.Dispose();
            }

            Units.Clear();
            _behaviours.Clear();
        }

        public T Get<T>() where T : IContextUnit
        {
            var type = typeof(T);
            if (_unitTypeMap.TryGetValue(type, out var unit))
            {
                return (T)unit;
            }

            throw new Exception($"Context unit of type {type} not found.");
        }
    }
}