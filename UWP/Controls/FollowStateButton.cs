using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace WeiPo.Controls
{
    public sealed class FollowStateButton : Control
    {


        public bool Following
        {
            get { return (bool)GetValue(FollowingProperty); }
            set { SetValue(FollowingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Following.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FollowingProperty =
            DependencyProperty.Register("Following", typeof(bool), typeof(FollowStateButton), new PropertyMetadata(false, OnPropertyChangedCallback));

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FollowStateButton).UpdateState();
        }

        private void UpdateState()
        {
            if (FollowMe && Following)
            {
                VisualStateManager.GoToState(this, "FollowTwoway", false);
            }
            else if (FollowMe)
            {
                VisualStateManager.GoToState(this, "FollowMeOnly", false);
            }
            else if (Following)
            {
                VisualStateManager.GoToState(this, "FollowingOnly", false);
            } 
            else
            {
                VisualStateManager.GoToState(this, "Initial", false);
            }
        }

        public bool FollowMe
        {
            get { return (bool)GetValue(FollowMeProperty); }
            set { SetValue(FollowMeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FollowMe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FollowMeProperty =
            DependencyProperty.Register("FollowMe", typeof(bool), typeof(FollowStateButton), new PropertyMetadata(false, OnPropertyChangedCallback));



        public FollowStateButton()
        {
            this.DefaultStyleKey = typeof(FollowStateButton);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateState();
        }
    }
}
