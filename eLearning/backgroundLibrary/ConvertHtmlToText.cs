using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace backgroundLibrary
{
    public class ConvertHtmlToText
    {
        public List<string> imgUrls = new List<string>();
        public string pgUrl = ""; //page url is needed to download images

        public ConvertHtmlToText(string pageUrl)
        {
            pgUrl = pageUrl;
        }
        
        public string Convert(string path)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(path);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public string ConvertHtml(string html, string pageUrl)
        {
            this.pgUrl = pageUrl;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            { 
                case HtmlNodeType.Comment:
                    //don't output comments
                    break;
                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;
                case HtmlNodeType.Text:
                    //script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;
                    
                    //get text
                    html = ((HtmlTextNode)node).Text;

                    //is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    //check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    { 
                        case "p":
                            //treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;   
                        case "img":
                            //imgUrls.Add(node.GetAttributeValue("src", null));
                            imgUrls.Add(GetActualImageUrl(node.GetAttributeValue("src", null)));
                            break;

                    }
                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

        private string GetActualImageUrl(string relativeUrl)//string imgSrc
        {
            HttpContext context = HttpContext.Current;
            var url = context.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;
            //BUILD AND RETURN ABSOLUTE URL
            return String.Format("{0}://{1}{2}{3}",
                   url.Scheme, url.Host, port, relativeUrl);

            //List<string> imageUrls = new List<string>();
            //HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            //document.Load(this.pgUrl);
            //foreach(HtmlNode image in document.DocumentNode.SelectNodes("//img[@src]"))
            //{
            //    //HtmlAttribute url = "src";
            //    //imageUrls.Add(url.Value);
            //}
            ////imageUrls now has all the image URLs

            //string imgUrl = "";
            //if (!imgSrc.StartsWith("http"))
            //{
            //    Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            //    MatchCollection mactches = regx.Matches(this.pgUrl);
            //    foreach (Match match in mactches)
            //    {
            //        string page = match.Value; // txt.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
            //    } 
    
            //}
            //else
            //    imgUrl = imgSrc;
                
            //return "";
        }
    }
}
