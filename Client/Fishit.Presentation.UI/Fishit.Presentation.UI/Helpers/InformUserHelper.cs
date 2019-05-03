using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views;

namespace Fishit.Presentation.UI.Helpers
{
    public class InformUserHelper<T>
    {
        public InformUserHelper(Response<T> response, IPageBase page)
        {
            Response = response;
            Page = page;
        }

        public Response<T> Response { get; set; }
        public IPageBase Page { get; set; }

        public void InformUserOfResponse()
        {
            InformUserOfResponse("");
        }

        public void InformUserOfResponse(string successMessage)
        {
            int statusCode = (int) Response.StatusCode;
            string statusMessage = Response.StatusCode.ToString();

            if (statusCode >= 400 && statusCode < 500)
            {
                DisplayErrorMessage("Client", statusCode, statusMessage, Response.Message);
            }
            if (statusCode > 500)
            {
                DisplayErrorMessage("Server", statusCode, statusMessage, Response.Message);
            }
            if (statusCode < 400 && !string.IsNullOrEmpty(successMessage))
            {
                Page.DisplayAlertMessage("Success", successMessage);
            }
        }

        private void DisplayErrorMessage(string type, int statusCode, string statusMessage, string responseMessage)
        {
            string errorMessage = "Oups, something went wrong :(\nError Code: " + statusCode + " - " + statusMessage +
                                  "\nError message: " + responseMessage;
            Page.DisplayAlertMessage(type + "-Error", errorMessage);
        }
    }
}