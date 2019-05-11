using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.MarkupExtensions
{
    [ContentProperty("ResourceId")]
    internal class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return string.IsNullOrWhiteSpace(ResourceId) ? null : ImageSource.FromResource(ResourceId);
        }
    }
}