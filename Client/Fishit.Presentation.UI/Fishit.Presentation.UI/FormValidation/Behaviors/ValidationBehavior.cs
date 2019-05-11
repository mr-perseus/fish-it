using System.Collections.ObjectModel;
using System.ComponentModel;
using Fishit.Presentation.UI.FormValidation.Errors;
using Fishit.Presentation.UI.FormValidation.Validators;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.FormValidation.Behaviors
{
    public class ValidationBehavior : Behavior<View>
    {
        private readonly IErrorStyle _style = new BasicErrorStyle();
        private View _view;
        public string PropertyName { get; set; }
        public ObservableCollection<IValidator> Validators { get; set; } = new ObservableCollection<IValidator>();
        public ValidationGroupBehavior Group { get; set; }
        public View RowView { get; set; }

        public bool Validate()
        {
            bool isValid = true;
            string errorMessage = "";

            foreach (IValidator validator in Validators)
            {
                bool result = validator.Check(_view.GetType()
                    .GetProperty(PropertyName)
                    ?.GetValue(_view) as string);

                if (!result)
                {
                    isValid = false;
                    errorMessage = validator.Message;
                }
            }

            if (!isValid)
            {
                _style.ShowError(RowView, errorMessage);
                return false;
            }

            _style.RemoveError(RowView);
            return true;
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            _view = bindable as View;
            if (RowView == null)
            {
                RowView = _view;
            }

            if (_view != null)
            {
                _view.PropertyChanged += OnPropertyChanged;
                _view.Unfocused += OnUnFocused;
            }

            Group?.Add(this);
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);

            _view.PropertyChanged -= OnPropertyChanged;
            _view.Unfocused -= OnUnFocused;

            Group?.Remove(this);
        }

        private void OnUnFocused(object sender, FocusEventArgs e)
        {
            Validate();

            Group?.Update();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
            {
                Validate();
            }
        }
    }
}