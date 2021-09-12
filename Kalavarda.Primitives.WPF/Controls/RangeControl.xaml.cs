using System.Windows.Media;
using Kalavarda.Primitives.Utils;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class RangeControl
    {
        private RangeF _range;

        public RangeF Range
        {
            get => _range;
            set
            {
                if (_range == value)
                    return;

                if (_range != null)
                {
                    _range.MinChanged -= OnChanged;
                    _range.ValueChanged -= OnChanged;
                    _range.MaxChanged -= OnChanged;
                }

                _range = value;

                if (_range != null)
                {
                    _range.MinChanged += OnChanged;
                    _range.ValueChanged += OnChanged;
                    _range.MaxChanged += OnChanged;
                    OnChanged(_range);
                }
            }
        }

        private void OnChanged(RangeF range)
        {
            this.Do(() =>
            {
                _front.Width = ActualWidth * range.ValueN;
                _tb.Text = range.Value.ToStr() + " / " + (range.Max - range.Min).ToStr();
            });
        }

        public SolidColorBrush MainBrush
        {
            get => (SolidColorBrush)_back.Fill;
            set => _back.Fill = value;
        }

        public RangeControl()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                if (Range != null)
                    OnChanged(Range);
            };
        }
    }
}
