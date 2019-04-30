using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views;

namespace Fishit.Presentation.UI.Helpers
{
    public class InformUserHelper<T>
    {
        public InformUserHelper(Response<T> response, IPageBase page, string successMessage)
        {
            Response = response;
            Page = page;
            SuccessMessage = successMessage;
        }

        public Response<T> Response { get; set; }
        public IPageBase Page { get; set; }
        public string SuccessMessage { get; set; }

        public void InformUserOfResponse()
        {
            int statusCode = (int) Response.StatusCode;
            string statusMessage = Response.StatusCode.ToString();

            if (statusCode >= 400 && statusCode < 500)
                DisplayErrorMessage("Client", statusCode, statusMessage, Response.Message);
            else if (statusCode > 500)
                DisplayErrorMessage("Server", statusCode, statusMessage, Response.Message);
            else
                Page.DisplayAlertMessage("Success", SuccessMessage);
        }

        private void DisplayErrorMessage(string type, int statusCode, string statusMessage, string responseMessage)
        {
            string errorMessage = "Oups, something went wrong :(\nError Code: " + statusCode + " - " + statusMessage +
                                  "\nError message: " + responseMessage;
            Page.DisplayAlertMessage(type + "-Error", errorMessage);
        }
    }
}