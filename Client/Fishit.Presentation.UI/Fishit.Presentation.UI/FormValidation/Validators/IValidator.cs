using System;
using System.Collections.Generic;
using System.Text;

namespace Fishit.Presentation.UI.FormValidation.Validators
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}
