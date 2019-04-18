using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.Behaviors
{
    internal class TemperatureBehavior : Behavior<Entry>
    {
        private const string TemperatureRegex = @"^-?([0-9]|([1-9][0-9]))(\.[0-9])?$";
        public bool IsValid { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = false;
            IsValid = Regex.IsMatch(e.NewTextValue, TemperatureRegex);
            ((Entry) sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}