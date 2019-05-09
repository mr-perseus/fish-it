using System.Collections.Generic;
using Fishit.Presentation.UI.FormValidation.Behaviors;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.FormValidation.Validators
{
    public class ValidationGroupBehavior : Behavior<View>
    {
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid",
                typeof(bool),
                typeof(ValidationBehavior),
                false);

        private readonly IList<ValidationBehavior> _validationBehaviors;

        public ValidationGroupBehavior()
        {
            _validationBehaviors = new List<ValidationBehavior>();
        }

        public bool IsValid
        {
            get => (bool) GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public void Add(ValidationBehavior validationBehavior)
        {
            _validationBehaviors.Add(validationBehavior);
        }

        public void Remove(ValidationBehavior validationBehavior)
        {
            _validationBehaviors.Remove(validationBehavior);
        }

        public void Update()
        {
            bool isValid = true;

            foreach (ValidationBehavior validationItem in _validationBehaviors)
            {
                isValid = isValid && validationItem.Validate();
            }

            IsValid = isValid;
        }
    }
}