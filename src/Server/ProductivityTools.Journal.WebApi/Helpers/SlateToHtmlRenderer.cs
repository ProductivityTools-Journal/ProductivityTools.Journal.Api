using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ProductivityTools.Meetings.WebApi.Helpers
{
    public static class SlateToHtmlRenderer
    {
        public static string RenderToHtml(string content, string fallbackPlainText = null)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                if (!string.IsNullOrWhiteSpace(fallbackPlainText))
                {
                    return $"<div class=\"slate-plain-text\">{HtmlEncoder.Default.Encode(fallbackPlainText)}</div>";
                }
                return string.Empty;
            }

            try
            {
                using var doc = JsonDocument.Parse(content);
                var sb = new StringBuilder();
                RenderNode(doc.RootElement, sb);
                return sb.ToString();
            }
            catch
            {
                var textToRender = !string.IsNullOrWhiteSpace(fallbackPlainText) ? fallbackPlainText : content;
                return $"<div class=\"slate-plain-text\">{HtmlEncoder.Default.Encode(textToRender)}</div>";
            }
        }

        private static void RenderNode(JsonElement element, StringBuilder sb)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    RenderNode(item, sb);
                }
                return;
            }

            if (element.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            // If it's a text leaf node
            if (element.TryGetProperty("text", out var textProp) && textProp.ValueKind == JsonValueKind.String)
            {
                RenderTextLeaf(element, textProp.GetString(), sb);
                return;
            }

            // Element node with type and children
            string type = element.TryGetProperty("type", out var typeProp) && typeProp.ValueKind == JsonValueKind.String
                ? typeProp.GetString()?.ToLowerInvariant()
                : "p";

            bool hasChildren = element.TryGetProperty("children", out var childrenProp) && childrenProp.ValueKind == JsonValueKind.Array;

            switch (type)
            {
                case "title":
                case "h1":
                case "headingone":
                    sb.Append("<h1>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h1>");
                    break;

                case "h2":
                case "headingtwo":
                    sb.Append("<h2>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h2>");
                    break;

                case "h3":
                case "headingthree":
                    sb.Append("<h3>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h3>");
                    break;

                case "h4":
                    sb.Append("<h4>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h4>");
                    break;

                case "h5":
                    sb.Append("<h5>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h5>");
                    break;

                case "h6":
                    sb.Append("<h6>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</h6>");
                    break;

                case "ul":
                case "unorderedlist":
                    sb.Append("<ul>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</ul>");
                    break;

                case "ol":
                case "orderedlist":
                    sb.Append("<ol>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</ol>");
                    break;

                case "li":
                case "list-item":
                    sb.Append("<li>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</li>");
                    break;

                case "lic":
                case "list-item-text":
                    // Wrapper inside li, render children directly
                    if (hasChildren) RenderNode(childrenProp, sb);
                    break;

                case "p":
                case "paragraph":
                    sb.Append("<p>");
                    if (hasChildren)
                    {
                        var startLength = sb.Length;
                        RenderNode(childrenProp, sb);
                        if (sb.Length == startLength)
                        {
                            sb.Append("<br />");
                        }
                    }
                    else
                    {
                        sb.Append("<br />");
                    }
                    sb.Append("</p>");
                    break;

                case "blockquote":
                    sb.Append("<blockquote>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</blockquote>");
                    break;

                case "code_block":
                case "code-block":
                    sb.Append("<pre class=\"slate-code-block\"><code>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</code></pre>");
                    break;

                case "code_line":
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("\n");
                    break;

                case "a":
                case "link":
                    string url = null;
                    if (element.TryGetProperty("url", out var urlProp) && urlProp.ValueKind == JsonValueKind.String)
                    {
                        url = urlProp.GetString();
                    }
                    else if (element.TryGetProperty("href", out var hrefProp) && hrefProp.ValueKind == JsonValueKind.String)
                    {
                        url = hrefProp.GetString();
                    }

                    if (!string.IsNullOrEmpty(url))
                    {
                        var safeUrl = HtmlEncoder.Default.Encode(url);
                        sb.Append($"<a href=\"{safeUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">");
                    }
                    else
                    {
                        sb.Append("<a>");
                    }

                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</a>");
                    break;

                case "img":
                case "image":
                    string imgSrc = null;
                    if (element.TryGetProperty("url", out var imgUrlProp) && imgUrlProp.ValueKind == JsonValueKind.String)
                    {
                        imgSrc = imgUrlProp.GetString();
                    }
                    else if (element.TryGetProperty("src", out var srcProp) && srcProp.ValueKind == JsonValueKind.String)
                    {
                        imgSrc = srcProp.GetString();
                    }

                    if (!string.IsNullOrEmpty(imgSrc))
                    {
                        var safeImgSrc = HtmlEncoder.Default.Encode(imgSrc);
                        sb.Append($"<div class=\"slate-image-wrapper\"><img src=\"{safeImgSrc}\" alt=\"\" /></div>");
                    }
                    break;

                case "table":
                    sb.Append("<table class=\"slate-inner-table\"><tbody>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</tbody></table>");
                    break;

                case "tr":
                case "table-row":
                    sb.Append("<tr>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</tr>");
                    break;

                case "td":
                case "table-cell":
                    sb.Append("<td>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</td>");
                    break;

                case "th":
                    sb.Append("<th>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</th>");
                    break;

                case "check_item":
                case "todo":
                case "action_item":
                    bool isChecked = element.TryGetProperty("checked", out var checkedProp) && checkedProp.ValueKind == JsonValueKind.True;
                    sb.Append($"<div class=\"slate-todo-item\"><input type=\"checkbox\" disabled {(isChecked ? "checked" : "")} /> <span>");
                    if (hasChildren) RenderNode(childrenProp, sb);
                    sb.Append("</span></div>");
                    break;

                default:
                    if (hasChildren)
                    {
                        RenderNode(childrenProp, sb);
                    }
                    break;
            }
        }

        private static void RenderTextLeaf(JsonElement element, string rawText, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(rawText))
            {
                return;
            }

            var text = HtmlEncoder.Default.Encode(rawText);

            bool isBold = element.TryGetProperty("bold", out var b) && b.ValueKind == JsonValueKind.True;
            bool isItalic = element.TryGetProperty("italic", out var it) && it.ValueKind == JsonValueKind.True;
            bool isUnderline = element.TryGetProperty("underline", out var u) && u.ValueKind == JsonValueKind.True;
            bool isStrike = (element.TryGetProperty("strikethrough", out var st) && st.ValueKind == JsonValueKind.True)
                         || (element.TryGetProperty("strike", out var s) && s.ValueKind == JsonValueKind.True);
            bool isCode = element.TryGetProperty("code", out var c) && c.ValueKind == JsonValueKind.True;

            var styleSb = new StringBuilder();
            if (element.TryGetProperty("color", out var colorProp) && colorProp.ValueKind == JsonValueKind.String)
            {
                styleSb.Append($"color: {HtmlEncoder.Default.Encode(colorProp.GetString())}; ");
            }
            if (element.TryGetProperty("bgColor", out var bgProp) && bgProp.ValueKind == JsonValueKind.String)
            {
                styleSb.Append($"background-color: {HtmlEncoder.Default.Encode(bgProp.GetString())}; ");
            }

            if (styleSb.Length > 0) sb.Append($"<span style=\"{styleSb}\">");
            if (isBold) sb.Append("<strong>");
            if (isItalic) sb.Append("<em>");
            if (isUnderline) sb.Append("<u>");
            if (isStrike) sb.Append("<s>");
            if (isCode) sb.Append("<code class=\"inline-code\">");

            sb.Append(text);

            if (isCode) sb.Append("</code>");
            if (isStrike) sb.Append("</s>");
            if (isUnderline) sb.Append("</u>");
            if (isItalic) sb.Append("</em>");
            if (isBold) sb.Append("</strong>");
            if (styleSb.Length > 0) sb.Append("</span>");
        }
    }
}
