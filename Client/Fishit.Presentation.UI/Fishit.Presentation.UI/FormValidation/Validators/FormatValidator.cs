using System.Text.RegularExpressions;

namespace Fishit.Presentation.UI.FormValidation.Validators
{
    public class FormatValidator : IValidator
    {
        public string Format { get; set; }
        public string Message { get; set; } = "Invalid format";

        public bool Check(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Regex format = new Regex(Format);

                return format.IsMatch(value);
            }

            return false;
        }
    }
}