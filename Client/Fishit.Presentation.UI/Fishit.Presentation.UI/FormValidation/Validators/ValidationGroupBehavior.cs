using System;
using System.Collections.Generic;
using System.Text;
using Fishit.Presentation.UI.FormValidation.Behaviors;
using Xamarin.Forms;

namespace Fishit.Presentation.UI.FormValidation.Validators
{
    public class ValidationGroupBehavior : Behavior<View>
    {
        private IList<ValidationBehavior> _validationBehaviors;
        public static readonly BindableProperty IsValidProperty = 
            BindableProperty.Create("IsValid",
                                    typeof(bool),
                                    typeof(ValidationBehavior),
                                    false);

        public ValidationGroupBehavior()
        {
            _validationBehaviors = new List<ValidationBehavior>();
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

        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }
    }
}
