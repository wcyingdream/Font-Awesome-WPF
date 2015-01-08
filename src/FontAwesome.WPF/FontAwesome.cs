﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FontAwesome.WPF
{
    /// <summary>
    /// Provides a lightweight control for displaying a FontAwesome icon as text.
    /// </summary>
    public class FontAwesome
        : TextBlock
    {
        /// <summary>
        /// FontAwesome FontFamily.
        /// </summary>
        private static readonly FontFamily FontAwesomeFontFamily = new FontFamily(new Uri("pack://application:,,,/FontAwesome.WPF;component/"), "./#FontAwesome");
        /// <summary>
        /// The key used for storing the spinner Storyboard.
        /// </summary>
        private static readonly string StoryBoardName = String.Format("{0}-storyboard-spinner", typeof(FontAwesome).Name);
        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(FontAwesomeIcon), typeof(FontAwesome), new PropertyMetadata(FontAwesomeIcon.None, OnIconPropertyChanged));
        /// <summary>
        /// Identifies the FontAwesome.WPF.FontAwesome.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool), typeof(FontAwesome), new PropertyMetadata(false, OnSpinPropertyChanged));

        /// <summary>
        /// Gets or sets the FontAwesome icon. Changing this property will cause the icon to be redrawn.
        /// </summary>
        public FontAwesomeIcon Icon
        {
            get { return (FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// Gets or sets the current spin (angle) animation of the icon.
        /// </summary>
        public bool Spin
        {
            get { return (bool)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(FontFamilyProperty, FontAwesomeFontFamily);
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
            d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fontAwesome = d as FontAwesome;

            if (fontAwesome == null) return;

            if ((bool)e.NewValue)
            {
                var storyboard = new Storyboard();

                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    AutoReverse = false,
                    RepeatBehavior = RepeatBehavior.Forever,
                    Duration = new Duration(TimeSpan.FromSeconds(1))
                };
                storyboard.Children.Add(animation);

                Storyboard.SetTarget(animation, fontAwesome);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(FontAwesome.RenderTransform).(RotateTransform.Angle)"));

                storyboard.Begin();
                fontAwesome.Resources.Add(StoryBoardName, storyboard);
            }
            else
            {
                var storyboard = fontAwesome.Resources[StoryBoardName] as Storyboard;

                if (storyboard == null) return;

                storyboard.Stop();

                fontAwesome.Resources.Remove(StoryBoardName);
            }
        }

    }
}
