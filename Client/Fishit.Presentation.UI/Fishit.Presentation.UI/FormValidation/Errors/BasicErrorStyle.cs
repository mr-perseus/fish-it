using Xamarin.Forms;

namespace Fishit.Presentation.UI.FormValidation.Errors
{
    public class BasicErrorStyle : IErrorStyle
    {
        public void ShowError(View view, string message)
        {
            if (view.Parent is StackLayout layout)
            {
                int viewIndex = layout.Children.IndexOf(view);

                if (viewIndex + 1 < layout.Children.Count)
                {
                    View sibling = layout.Children[viewIndex + 1];
                    string siblingStyleId = view.Id.ToString();
                    
                    if (sibling.StyleId == siblingStyleId)
                    {
                        if (sibling is Label errorLabel)
                        {
                            errorLabel.Text = message;
                            errorLabel.IsVisible = true;
                        }

                        return;
                    }
                }

                layout.Children.Insert(viewIndex + 1, new Label
                {
                    Text = message,
                    FontSize = 12,
                    StyleId = view.Id.ToString(),
                    TextColor = Color.Red
                });
            }
        }

        public void RemoveError(View view)
        {
            if (view.Parent is StackLayout layout)
            {
                int viewIndex = layout.Children.IndexOf(view);

                if (viewIndex + 1 < layout.Children.Count)
                {
                    View sibling = layout.Children[viewIndex + 1];
                    string siblingStyleId = view.Id.ToString();

                    if (sibling.StyleId == siblingStyleId)
                    {
                        sibling.IsVisible = false;
                    }
                }
            }
        }
    }
}