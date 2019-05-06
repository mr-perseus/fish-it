using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.MarkupExtensions
{
    // See Splashscreen for an example on how to use
    [ContentProperty("ResourceId")]
    internal class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ResourceId))
            {
                return null;
            }

            return ImageSource.FromResource(ResourceId);
        }
    }
}