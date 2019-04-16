using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.Behaviors
{
    internal class LocationBehavior : Behavior<Entry>
    {
        private const string CharOnlyRegex = @"^([A-Za-z]|Ä|Ö|Ü|ä|ö|ü|-| )*$";
        public bool IsValid { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = false;
            IsValid = Regex.IsMatch(e.NewTextValue, CharOnlyRegex);
            ((Entry) sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}