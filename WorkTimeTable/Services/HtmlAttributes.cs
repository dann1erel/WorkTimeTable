using Microsoft.Extensions.Primitives;

namespace WorkTimeTable.Services
{
    public class HtmlAttributes
    {
        public static string GetSortingCaretType(StringValues requestParameter, string column)
        {
            string caretType;
            string order = requestParameter.ToString() ?? "";

            if (order == $"{column}_desc")
            {
                caretType = "caret-down-fill";
            }
            else if (order == $"{column}_asc")
            {
                caretType = "caret-up-fill";
            }
            else
            {
                caretType = "caret-down";
            }

            return caretType;
        }

        //public static string 
    }
}
