using EmailMarketing.Web.Areas.Member.Enums;

namespace EmailMarketing.Web.Areas.Member.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public string IconCssClass { get; set; }
        public string StyleCssClass { get; set; }

        public ResponseModel()
        {

        }

        public ResponseModel(string message, ResponseType type)
        {
            if (type == ResponseType.Success)
            {
                IconCssClass = "icon-checkmark4";
                StyleCssClass = "alert-success";
                Title = "Success!";
            }
            else if (type == ResponseType.Failure)
            {
                IconCssClass = "icon-blocked";
                StyleCssClass = "alert-danger";
                Title = "Error!";
            }

            Message = message;
        }
    }
}
