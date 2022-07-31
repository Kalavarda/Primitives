using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.Utils;
using Kalavarda.Primitives.WPF.Binds;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class SkillControl
    {
        private ISkill _skill;
        private KeyBind _bind;
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(0.1) };

        public ISkill Skill
        {
            get => _skill;
            set
            {
                if (_skill == value)
                    return;

                _skill = value;

                if (_skill != null)
                {
                    _border.ToolTip = _skill.Name;
                    _timer.Start();
                    _rectCooldown.Visibility = Visibility.Visible;

                    if (_skill is IHasCount<long> hasCount)
                    {
                        hasCount.CountChanged += HasCount_CountChanged;
                        HasCount_CountChanged(hasCount);
                    }

                    if (_skill is IHasImage hasImage)
                    {
                        _image.Visibility = Visibility.Visible;
                        _image.Source = BitmapImageCache.Instance.Get(hasImage.ImageUri);
                    }
                }
                else
                {
                    _rectCooldown.Visibility = Visibility.Collapsed;
                    _border.ToolTip = null;
                    _timer.Start();
                }
            }
        }

        private void HasCount_CountChanged(IHasCount<long> hasCount)
        {
            this.Do(() =>
            {
                _count.Visibility = Visibility.Visible;
                _count.Text = hasCount.Max == null
                    ? hasCount.Count.ToStr()
                    : $"{hasCount.Count.ToStr()} / {hasCount.Max.Value.ToStr()}";

                _image.Opacity = hasCount.Count > 0 ? 1 : 0.25;
            });
        }

        public KeyBind Bind
        {
            get => _bind;
            set
            {
                if (_bind == value)
                    return;

                _bind = value;

                if (_bind != null)
                    _tbBind.Text = _bind.Key?.ToString().TrimStart('D');
            }
        }

        public SkillControl()
        {
            InitializeComponent();
            _timer.Tick += _timer_Tick;
            _timer_Tick(this, null);
            Unloaded += SkillControl_Unloaded;
        }

        private void SkillControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Tick -= _timer_Tick;
            _timer.Stop();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_skill?.TimeLimiter != null)
                _scale.ScaleY = _skill.TimeLimiter.Remain.TotalSeconds / _skill.TimeLimiter.Interval.TotalSeconds;
            else
                _scale.ScaleY = 0;
        }
    }
}
