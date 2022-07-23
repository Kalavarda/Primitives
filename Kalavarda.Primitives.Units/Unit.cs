﻿using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;

namespace Kalavarda.Primitives.Units
{
    public abstract class Unit : IMapObject, ISkilled, ICreature
    {
        public static readonly TimeSpan GlobalCooldown = TimeSpan.FromSeconds(0.5);
        private Unit _target;
        private bool _isSelected;

        public AngleF MoveDirection { get; } = new();

        public RangeF MoveSpeed { get; }

        public PointF Position { get; } = new();

        /// <summary>
        /// В какую точку нужно двигаться
        /// </summary>
        public PointF MoveTarget { get; } = new();

        protected Unit(RangeF moveSpeed)
        {
            MoveSpeed = moveSpeed;
            HP.ValueMin += HP_ValueMin;
        }

        private void HP_ValueMin(RangeF obj)
        {
            Died?.Invoke(this);
        }

        public abstract BoundsF Bounds { get; }

        public RangeF HP { get; } = new();

        /// <inheritdoc/>
        public bool IsAlive => !HP.IsMin;

        /// <inheritdoc/>
        public bool IsDead => HP.IsMin;

        public event Action<ICreature> Died;

        public Unit Target
        {
            get => _target;
            set
            {
                if (_target == value)
                    return;

                _target = value;
                TargetChanged?.Invoke();
            }
        }

        public event Action TargetChanged;

        public abstract IEnumerable<ISkill> Skills { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                IsSelectedChanged?.Invoke(this);
            }
        }

        public event Action<Unit> IsSelectedChanged;
    }
}