using System;
using System.Collections.Generic;
using System.Text;

namespace Fishit.Presentation.UI.FormValidation.Validators
{
    public class PositiveDoubleValidator : IValidator
    {
        public string Message { get; set; }
        public int MaxValue { get; set; }
        public string MaxValueMessage { get; set; }

        public bool Check(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return true;
            }

            try
            {
                double.TryParse(value, out double number);
                if (number < MaxValue)
                {
                    Message = "Number must be bigger than zero.";
                    return number > 0;
                }

                Message = MaxValueMessage;
                return false;
            }
            catch (FormatException)
            {
                Message = "Please enter a valid number.";
                return false;
            }
            catch (OverflowException)
            {
                Message = "Something's not right...";
                return false;
            }
        }
    }
}