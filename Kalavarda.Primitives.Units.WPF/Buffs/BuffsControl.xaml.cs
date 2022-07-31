using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Kalavarda.Primitives.Units.Buffs;
using Kalavarda.Primitives.Units.Interfaces;
using Kalavarda.Primitives.WPF;

namespace Kalavarda.Primitives.Units.WPF.Buffs
{
    public partial class BuffsControl
    {
        private IReadonlyHasBuffs _hasBuffs;
        private readonly ICollection<Buff> _buffs = new ObservableCollection<Buff>();

        public IReadonlyHasBuffs HasBuffs
        {
            get => _hasBuffs;
            set
            {
                if (_hasBuffs == value)
                    return;

                if (_hasBuffs != null)
                {
                    _hasBuffs.BuffAdded -= OnBuffAdded;
                    _hasBuffs.BuffRemoved += OnBuffRemoved;
                    _buffs.Clear();
                }

                _hasBuffs = value;

                if (_hasBuffs != null)
                {
                    _hasBuffs.BuffAdded += OnBuffAdded;
                    foreach (var buff in _hasBuffs.Buffs)
                        OnBuffAdded(_hasBuffs, buff);
                    _hasBuffs.BuffRemoved += OnBuffRemoved;
                }
            }
        }

        private void OnBuffRemoved(IReadonlyHasBuffs arg1, Buff buff)
        {
            this.Do(() =>
            {
                _buffs.Remove(buff);
            });
        }

        private void OnBuffAdded(IReadonlyHasBuffs arg1, Buff buff)
        {
            this.Do(() =>
            {
                _buffs.Add(buff);
            });
        }

        public BuffsControl()
        {
            InitializeComponent();
            _itemsControl.ItemsSource = _buffs;
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
