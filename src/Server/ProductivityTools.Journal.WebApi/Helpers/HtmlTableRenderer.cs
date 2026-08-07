using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace ProductivityTools.Journal.WebApi.Helpers
{
    public static class HtmlTableRenderer
    {
        public static string RenderPageTable(Page page)
        {
            if (page == null)
            {
                return "<html><body><p>Page not found</p></body></html>";
            }

            var subject = HtmlEncoder.Default.Encode(page.Subject ?? "");
            var date = page.Date.ToString("yyyy-MM-dd HH:mm:ss");
            var plainText = HtmlEncoder.Default.Encode(page.PlainText ?? "");

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"utf-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine($"    <title>{subject}</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        body { font-family: -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, Helvetica, Arial, sans-serif; margin: 20px; background-color: #f8f9fa; color: #333; }");
            sb.AppendLine("        .container { max-width: 900px; margin: 0 auto; background: #fff; padding: 24px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }");
            sb.AppendLine("        table { width: 100%; border-collapse: collapse; margin-top: 12px; }");
            sb.AppendLine("        th, td { padding: 12px 16px; border: 1px solid #dee2e6; text-align: left; vertical-align: top; }");
            sb.AppendLine("        th { background-color: #f1f3f5; width: 140px; font-weight: 600; color: #495057; }");
            sb.AppendLine("        .content { white-space: pre-wrap; line-height: 1.6; word-break: break-word; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <table>");
            sb.AppendLine($"            <tr><th>Subject</th><td>{subject}</td></tr>");
            sb.AppendLine($"            <tr><th>Date</th><td>{date}</td></tr>");
            sb.AppendLine($"            <tr><th>Plain Text</th><td class=\"content\">{plainText}</td></tr>");
            sb.AppendLine("        </table>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string RenderPagesTable(List<Page> pages)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"utf-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine("    <title>Pages</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        body { font-family: -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, Helvetica, Arial, sans-serif; margin: 20px; background-color: #f8f9fa; color: #333; }");
            sb.AppendLine("        .container { max-width: 1200px; margin: 0 auto; background: #fff; padding: 24px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }");
            sb.AppendLine("        table { width: 100%; border-collapse: collapse; margin-top: 12px; }");
            sb.AppendLine("        th, td { padding: 10px 14px; border: 1px solid #dee2e6; text-align: left; vertical-align: top; }");
            sb.AppendLine("        th { background-color: #f1f3f5; font-weight: 600; color: #495057; }");
            sb.AppendLine("        tr:nth-child(even) { background-color: #fafafa; }");
            sb.AppendLine("        .content { white-space: pre-wrap; line-height: 1.5; word-break: break-word; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <table>");
            sb.AppendLine("            <thead>");
            sb.AppendLine("                <tr>");
            sb.AppendLine("                    <th style=\"width: 160px;\">Date</th>");
            sb.AppendLine("                    <th style=\"width: 240px;\">Subject</th>");
            sb.AppendLine("                    <th>Plain Text</th>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("            </thead>");
            sb.AppendLine("            <tbody>");

            if (pages != null)
            {
                foreach (var page in pages)
                {
                    var subject = HtmlEncoder.Default.Encode(page.Subject ?? "");
                    var date = page.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    var plainText = HtmlEncoder.Default.Encode(page.PlainText ?? "");

                    sb.AppendLine("                <tr>");
                    sb.AppendLine($"                    <td>{date}</td>");
                    sb.AppendLine($"                    <td>{subject}</td>");
                    sb.AppendLine($"                    <td class=\"content\">{plainText}</td>");
                    sb.AppendLine("                </tr>");
                }
            }

            sb.AppendLine("            </tbody>");
            sb.AppendLine("        </table>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
