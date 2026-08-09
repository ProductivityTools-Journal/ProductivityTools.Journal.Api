using ProductivityTools.Meetings.CoreObjects;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace ProductivityTools.Meetings.WebApi.Helpers
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
            var path = HtmlEncoder.Default.Encode(page.Path ?? "-");
            var pinned = page.Pinned ? "<span class=\"badge badge-pinned\">Yes</span>" : "No";
            var contentType = HtmlEncoder.Default.Encode(page.ContentType ?? "-");
            var publicHash = HtmlEncoder.Default.Encode(page.PublicHash ?? "-");
            var pageId = page.PageId?.ToString() ?? "-";
            var journalId = page.JournalId?.ToString() ?? "-";

            // Server-side conversion from Slate JSON to formatted HTML (with fallback to PlainText)
            var renderedHtmlContent = SlateToHtmlRenderer.RenderToHtml(page.Content, page.PlainText);

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"utf-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine($"    <title>{subject}</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine(GetSharedCss());
            sb.AppendLine("        .container { max-width: 950px; margin: 0 auto; background: #fff; padding: 32px; border-radius: 12px; box-shadow: 0 4px 16px rgba(0,0,0,0.08); }");
            sb.AppendLine("        .page-header { margin-bottom: 24px; border-bottom: 2px solid #0284c7; padding-bottom: 12px; }");
            sb.AppendLine("        .meta-table { width: 100%; border-collapse: collapse; margin-bottom: 28px; }");
            sb.AppendLine("        .meta-table th, .meta-table td { padding: 10px 14px; border: 1px solid #e2e8f0; text-align: left; vertical-align: top; }");
            sb.AppendLine("        .meta-table th { background-color: #f8fafc; width: 170px; font-weight: 600; color: #475569; font-size: 0.9rem; }");
            sb.AppendLine("        .content-section-title { font-size: 1.1rem; font-weight: 600; color: #334155; margin-bottom: 12px; border-bottom: 1px solid #e2e8f0; padding-bottom: 6px; }");
            sb.AppendLine("        .content-container { background: #fafafa; border: 1px solid #e2e8f0; border-radius: 8px; padding: 24px; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <div class=\"page-header\">");
            sb.AppendLine($"            <h1 style=\"margin:0;\">{subject}</h1>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <table class=\"meta-table\">");
            sb.AppendLine($"            <tr><th>Subject</th><td><strong>{subject}</strong></td></tr>");
            sb.AppendLine($"            <tr><th>Path</th><td class=\"path-crumb\">{path}</td></tr>");
            sb.AppendLine($"            <tr><th>Date</th><td>{date}</td></tr>");
            sb.AppendLine($"            <tr><th>Pinned</th><td>{pinned}</td></tr>");
            sb.AppendLine($"            <tr><th>Page ID / Journal ID</th><td>{pageId} / {journalId}</td></tr>");
            sb.AppendLine($"            <tr><th>Content Type</th><td>{contentType}</td></tr>");
            sb.AppendLine($"            <tr><th>Public Hash</th><td><code>{publicHash}</code></td></tr>");
            sb.AppendLine("        </table>");
            sb.AppendLine("        <div class=\"content-section-title\">Document Content</div>");
            sb.AppendLine("        <div class=\"content-container slate-rendered-content\">");
            sb.AppendLine(renderedHtmlContent);
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string RenderPagesTable(List<Page> pages)
        {
            var count = pages?.Count ?? 0;
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"utf-8\" />");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
            sb.AppendLine("    <title>Public Pages</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine(GetSharedCss());
            sb.AppendLine("        .container { max-width: 1400px; margin: 0 auto; background: #fff; padding: 32px; border-radius: 12px; box-shadow: 0 4px 16px rgba(0,0,0,0.08); }");
            sb.AppendLine("        .header-bar { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; border-bottom: 2px solid #0284c7; padding-bottom: 12px; }");
            sb.AppendLine("        .header-bar h1 { margin: 0; font-size: 1.6rem; color: #1a1a1a; }");
            sb.AppendLine("        .count-badge { background-color: #e0f2fe; color: #0369a1; padding: 4px 14px; border-radius: 16px; font-weight: 600; font-size: 0.9rem; }");
            sb.AppendLine("        .pages-table { width: 100%; border-collapse: collapse; margin-top: 12px; }");
            sb.AppendLine("        .pages-table th, .pages-table td { padding: 14px 16px; border: 1px solid #e2e8f0; text-align: left; vertical-align: top; }");
            sb.AppendLine("        .pages-table th { background-color: #f8fafc; font-weight: 600; color: #475569; font-size: 0.85rem; text-transform: uppercase; letter-spacing: 0.5px; }");
            sb.AppendLine("        .pages-table tr:nth-child(even) { background-color: #fcfdfe; }");
            sb.AppendLine("        .pages-table tr:hover { background-color: #f1f5f9; }");
            sb.AppendLine("        .date-cell { font-size: 0.88rem; color: #64748b; white-space: nowrap; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"container\">");
            sb.AppendLine("        <div class=\"header-bar\">");
            sb.AppendLine("            <h1>Public Pages</h1>");
            sb.AppendLine($"            <span class=\"count-badge\">Total pages: {count}</span>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <table class=\"pages-table\">");
            sb.AppendLine("            <thead>");
            sb.AppendLine("                <tr>");
            sb.AppendLine("                    <th style=\"width: 150px;\">Date</th>");
            sb.AppendLine("                    <th style=\"width: 200px;\">Path</th>");
            sb.AppendLine("                    <th style=\"width: 200px;\">Subject</th>");
            sb.AppendLine("                    <th style=\"width: 70px;\">Pinned</th>");
            sb.AppendLine("                    <th>Content</th>");
            sb.AppendLine("                </tr>");
            sb.AppendLine("            </thead>");
            sb.AppendLine("            <tbody>");

            if (pages != null)
            {
                foreach (var page in pages)
                {
                    var subject = HtmlEncoder.Default.Encode(page.Subject ?? "");
                    var date = page.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    var path = HtmlEncoder.Default.Encode(page.Path ?? "-");
                    var pinned = page.Pinned ? "<span class=\"badge badge-pinned\">Yes</span>" : "";

                    // Server-side conversion from Slate JSON to formatted HTML (with fallback to PlainText)
                    var renderedHtmlContent = SlateToHtmlRenderer.RenderToHtml(page.Content, page.PlainText);

                    sb.AppendLine("                <tr>");
                    sb.AppendLine($"                    <td class=\"date-cell\">{date}</td>");
                    sb.AppendLine($"                    <td class=\"path-crumb\">{path}</td>");
                    sb.AppendLine($"                    <td><strong>{subject}</strong></td>");
                    sb.AppendLine($"                    <td>{pinned}</td>");
                    sb.AppendLine($"                    <td class=\"slate-rendered-content\">{renderedHtmlContent}</td>");
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

        private static string GetSharedCss()
        {
            return @"
        body { font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; margin: 24px; background-color: #f4f6f8; color: #212529; line-height: 1.6; }
        .path-crumb { color: #0284c7; font-weight: 500; font-size: 0.9rem; }
        .badge { display: inline-block; padding: 2px 8px; font-size: 0.75rem; font-weight: 600; border-radius: 4px; }
        .badge-pinned { background-color: #fef3c7; color: #92400e; border: 1px solid #fde68a; }
        code { font-family: SFMono-Regular, Menlo, Monaco, Consolas, ""Liberation Mono"", ""Courier New"", monospace; font-size: 0.9em; background-color: #f1f5f9; padding: 2px 6px; border-radius: 4px; color: #0f172a; }

        /* Slate / Plate rendered content formatting */
        .slate-rendered-content { line-height: 1.6; color: #1e293b; }
        .slate-rendered-content h1 { font-size: 1.5rem; margin: 16px 0 8px 0; color: #0f172a; border-bottom: 1px solid #e2e8f0; padding-bottom: 4px; }
        .slate-rendered-content h2 { font-size: 1.3rem; margin: 14px 0 6px 0; color: #0f172a; }
        .slate-rendered-content h3 { font-size: 1.15rem; margin: 12px 0 4px 0; color: #1e293b; }
        .slate-rendered-content h4, .slate-rendered-content h5, .slate-rendered-content h6 { font-size: 1rem; margin: 10px 0 4px 0; color: #334155; }
        .slate-rendered-content p { margin: 6px 0; }
        .slate-rendered-content ul { list-style-type: disc; margin: 6px 0; padding-left: 24px; }
        .slate-rendered-content ul ul { list-style-type: circle; }
        .slate-rendered-content ul ul ul { list-style-type: square; }
        .slate-rendered-content ol { list-style-type: decimal; margin: 6px 0; padding-left: 24px; }
        .slate-rendered-content li { margin: 3px 0; }
        .slate-rendered-content a { color: #0284c7; text-decoration: none; }
        .slate-rendered-content a:hover { text-decoration: underline; }
        .slate-rendered-content blockquote { border-left: 4px solid #0284c7; margin: 10px 0; padding: 6px 14px; background-color: #f8fafc; color: #475569; font-style: italic; border-radius: 0 4px 4px 0; }
        .slate-rendered-content .slate-code-block { background-color: #1e293b; color: #f8fafc; padding: 12px 16px; border-radius: 6px; overflow-x: auto; margin: 8px 0; font-family: monospace; font-size: 0.9rem; }
        .slate-rendered-content .inline-code { background-color: #e2e8f0; color: #b91c1c; padding: 2px 5px; border-radius: 3px; font-family: monospace; font-size: 0.88em; }
        .slate-rendered-content .slate-inner-table { border-collapse: collapse; width: 100%; margin: 10px 0; }
        .slate-rendered-content .slate-inner-table th, .slate-rendered-content .slate-inner-table td { border: 1px solid #cbd5e1; padding: 8px 12px; text-align: left; }
        .slate-rendered-content .slate-inner-table th { background-color: #f1f5f9; font-weight: 600; }
        .slate-rendered-content .slate-todo-item { display: flex; align-items: center; gap: 8px; margin: 4px 0; }
        .slate-rendered-content .slate-plain-text { white-space: pre-wrap; word-break: break-word; font-family: inherit; }
        .slate-rendered-content .slate-image-wrapper { margin: 10px 0; text-align: center; }
        .slate-rendered-content .slate-image-wrapper img { max-width: 100%; height: auto; border-radius: 6px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
";
        }
    }
}
