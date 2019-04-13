using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.Behaviors
{
    class TemperatureBehavior : Behavior<Entry>
    {
        public bool IsValid { get; set; }
        private const string TemperatureRegex = @"^-?([0-9]|([1-9][0-9]))(\.[0-9])?$";

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, TemperatureRegex));
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
