using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using backgroundLibrary;
using GoogleSearchAPI.Query;
using System.Data;
using Google.API.Search;
using DAL;
//using System.Windows.Forms;

namespace eLearning
{
    public partial class _Default : System.Web.UI.Page
    {
        //private NumericUpDown nudNumOfResults;
        //private NumericUpDown nudStartPosition;
        private Size imageSize = new Size(150, 150);
        private readonly int startHeight = 70;
        List<string> list_auth_url = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            list_auth_url = GetAllAuthenticUrls();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string story = "";
            if (txtStory.Text != "")
            {
                story = txtStory.Text;
            }
            else
            {
                if (fileUpload != null)
                {
                    var allowedExtensions = new[] { ".txt" };
                    var extension = Path.GetExtension(fileUpload.FileName);
                    if (extension == ".txt")
                    {
                        //Need To code For Excel Files Reading
                        StreamReader fileReader = new StreamReader(fileUpload.FileContent);
                        do
                        {
                            story = story + fileReader.ReadLine();
                        }
                        while (fileReader.Peek() != -1);
                        fileReader.Close();
                    }
                }
            }
            string wordInfo = "";
            string searchExpression = "";
            KeywordAnalyzer ka = new KeywordAnalyzer();
            string[] wordCol = story.Split(' ');
            if (wordCol.Count() <= 5)
            {
                searchExpression = story;
            }
            else
            {

                var g = ka.Analyze(story);
                foreach (var key in g.Keywords)
                {
                    wordInfo = wordInfo + "   key: " + key.Word + ";   " + "rank: " + key.Rank + "<br>";
                }

                //lblKeywords.Text = wordInfo;

                //wordInfo = wordInfo + "Keywords are:" + "<br>";
                var gty = (from n in g.Keywords select n).Take(10);

                //find if there are any multi-word in keyword list
                string phrase = "";
                for (int i = 0; i < gty.Count()/2; i++)
                {
                    if (isMultipleWords(gty.ToList()[i].Word))
                    {
                        phrase = gty.ToList()[i].Word;
                        break;
                    }
                }

                if (phrase == "")
                {
                    try
                    { //if not multi-word is found take first three words
                        searchExpression = gty.ToList()[0].Word + " " + gty.ToList()[1].Word + " " + gty.ToList()[2].Word + " " + gty.ToList()[3].Word;
                    }
                    catch (Exception ex)
                    { }
                }
                else
                { //else take two more high rank word from the keyword list which is not already on the multi-word keyword
                    int count = 0;
                    for (int i = 0; i < gty.Count(); i++)
                    {
                        
                        if (phrase.Contains(gty.ToList()[i].Word))
                        {
                            continue;
                        }
                        else
                        {
                            searchExpression = searchExpression + " " + gty.ToList()[i].Word;
                            count++;
                            if (count > 1)
                                break;
                        }
                    }
                }
                searchExpression = searchExpression + " " + phrase;  // " for children"; // adding this phrase improves the search result
            }

            lblKeywords.Text = " <br> search exp: " + searchExpression;//wordInfo + " <br> search exp: " + searchExpression;//
            BindImageIntoDataList(searchExpression);
            BindVideoIntoDataList(searchExpression);
            
            //lblSearchResult.Text = result;
            //saveWebContent(resultSet);
            
        }

        public bool isMultipleWords(string st)
        {
            int wordCount = 0;
            string[] words = st.Split(' ');
            foreach (var wd in words)
                wordCount++;
            if (wordCount > 1)
                return true;
            else
                return false;
        }

        //public void saveWebContent(List<Search.SearchResult> result)
        //{
        //    string filePath = "D:\\extrafiles\\";
        //    ConvertHtmlToText htmTxt;

        //    foreach (Search.SearchResult sr in result)
        //    {
        //        HttpWebRequest webReq = WebRequest.Create(sr.url) as HttpWebRequest;
        //        webReq.Method = "POST";
        //        WebClient clt = new WebClient();
        //        htmTxt = new ConvertHtmlToText(sr.url);
        //        try
        //        {
        //            byte[] data = clt.DownloadData(sr.url);
        //            string html = System.Text.Encoding.UTF8.GetString(data);
        //            string fileName = filePath + DateTime.Now.ToString("yyyyMMddHHmmssfff")+".txt";

                    
        //            string actualText = htmTxt.ConvertHtml(html, sr.url);
        //            WriteTextFile(fileName, actualText);
        //        }
        //        catch (Exception ex)
        //        { }
                
        //    }
        //}

        public bool WriteTextFile(string fileName, string t)
        {
            TextWriter textWriter;
            try
            {
                textWriter = new StreamWriter(fileName);
            }
            catch(Exception ex)
            {
                return false;
            }
            try
            {
                textWriter.WriteLine(t);
            }
            catch (Exception ex)
            {
                return false;
            }
            textWriter.Close();
            return true;
        }

        #region --comment out
        //private System.Drawing.Image getImage(string url)
        //{
        //    System.Drawing.Image im = null;
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "GET";
        //        request.Timeout = 15000;
        //        request.ProtocolVersion = HttpVersion.Version11;

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (Stream responseStream = response.GetResponseStream())
        //            {
        //                im = System.Drawing.Image.FromStream(responseStream);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Exception in getThumbnail. Url: " + url + ". Info: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace);
        //    }
        //    return im;
        //}

        //private void pic_DoubleClick(object sender, EventArgs e)
        //{
        //    Cursor = Cursors.WaitCursor;

        //    PictureBox pic = sender as PictureBox;
        //    SearchResult result = pic.Tag as SearchResult;
        //    Image image = getImage(result.ImageUrl);
        //    if (null == image)
        //    {
        //        MessageBox.Show(this, "Unable to retrieve image: " + result.ImageUrl);
        //        return;
        //    }
        //    Form form = new Form();
        //    PictureBox fullImage = new PictureBox();
        //    fullImage.Image = image;
        //    fullImage.Height = image.Height;
        //    fullImage.Width = image.Width;
        //    fullImage.Dock = DockStyle.Fill;
        //    fullImage.SizeMode = PictureBoxSizeMode.StretchImage;
        //    Size sizeDiff = form.Size - form.ClientSize;
        //    form.Height = image.Height + sizeDiff.Height;
        //    form.Width = image.Width + sizeDiff.Width;
        //    form.Text = string.Format("{0} x {1} - {2} kb",
        //        result.ImageWidth.ToString(), result.ImageHeight.ToString(), ((int)(result.ImageSize / 1000.0)).ToString());
        //    form.Controls.Add(fullImage);
        //    form.KeyDown += new KeyEventHandler(form_KeyDown);
        //    form.ShowDialog(this);

        //    Cursor = Cursors.Default;
        //}
        #endregion

        private void BindImageIntoDataList(string searchExpression)
        {
            WebFilter webFilterObj = new WebFilter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("OriginalContextUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("Url", typeof(string)));
            GimageSearchClient client = new GimageSearchClient("www.c-sharpcorner.com");
            
            // IList<IImageResult> results = webFilterObj.RankAndApplyFilterImages(client.Search(searchExpression, 10), list_auth_url); //applyFilterToImages(client.Search(searchExpression, 10));
            List<ResultWithScore> result_Score = webFilterObj.RankAndApplyFilterImages(client.Search(searchExpression, 20), list_auth_url);
            foreach (ResultWithScore result in result_Score)
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = result.imgResult.Title.ToString();
                dr["OriginalContextUrl"] = result.imgResult.OriginalContextUrl;
                dr["Url"] = result.imgResult.Url;
                dt.Rows.Add(dr);
            }
            dlImageSearch.DataSource = dt;
            dlImageSearch.DataBind();
        }

        private void BindVideoIntoDataList(string searchExpression)
        {
            WebFilter webFilterObj = new WebFilter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("PlayUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("Url", typeof(string)));
            dt.Columns.Add(new DataColumn("Duration", typeof(string))); //
            GvideoSearchClient client = new GvideoSearchClient("www.c-sharpcorner.com");
            IList<IVideoResult> results = client.Search(searchExpression, 10);
            List<ResultWithScore> result_Score = webFilterObj.RankAndApplyFilterVideos(searchExpression, results, list_auth_url);

            foreach (ResultWithScore result in result_Score)
            {
                DataRow dr = dt.NewRow();
                dr["Title"] = result.vidResult.Title.ToString();
                dr["PlayUrl"] = result.vidResult.PlayUrl;
                //dr["Url"] = result.Url;
                dr["Url"] = result.vidResult.TbImage;
                //convert seconds to hour and minute //--
                TimeSpan t = TimeSpan.FromSeconds(result.vidResult.Duration);
                string time = string.Format("{0:D2}h:{1:D2}m", t.Hours, t.Minutes);
                dr["Duration"] = time; //--
                dt.Rows.Add(dr);
            }
            dlVideoSearch.DataSource = dt;
            dlVideoSearch.DataBind();
        }

        //AnalyseWebContent webAnalyse = new AnalyseWebContent();
       // URLFiltering urlFil = new URLFiltering();

        private IList<IImageResult> applyFilterToImages(IList<IImageResult> srcResult)
        {
            //List<string> list_urls = GetAllAuthenticUrls();
            //foreach (IImageResult ir in srcResult)
            //{
            //    if (list_urls.Any((ir.Url).Contains)) // most authentic list entered from users
            //        continue;
            //    else
            //    {
            //        if (webAnalyse.classifyText(ir.Title) >= 0)
            //            continue;
            //        else if ((webAnalyse.classifyText(ir.Title) < 0) && (urlFil.classifyUrl(ir.Url) == 0))
            //            srcResult.Remove(ir);
            //    } 
            //}
            return srcResult;
        }

        private IList<IVideoResult> applyFilterToVideos(IList<IVideoResult> srcResult)
        {
            //List<string> list_urls = GetAllAuthenticUrls();
            //foreach (IVideoResult ir in srcResult)
            //{
            //    if (list_urls.Any((ir.PlayUrl).Contains)) // most authentic list entered from users
            //        continue;
            //    else
            //    {
            //        if (webAnalyse.classifyText(ir.Title) >= 0)
            //            continue;
            //        else if ((webAnalyse.classifyText(ir.Title) < 0) && (urlFil.classifyUrl(ir.PlayUrl) == 0))
            //            srcResult.Remove(ir);
            //    }
            //}
            return srcResult;
        }

        private List<string> GetAllAuthenticUrls()
        {
            DataManager dmObj = new DataManager();
            List<string> list_url = dmObj.SelectAllAuthenticUrls();
            return list_url;
        }

    }
}
