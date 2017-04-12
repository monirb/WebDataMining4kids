using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net;
using Google.API.Search;

namespace backgroundLibrary
{
    public class WebFilter
    {
        AnalyseWebContent webAnalysis = new AnalyseWebContent();
        URLFiltering urlFil = new URLFiltering();

        //image search result and the list of authentic urls are passed in to the function
        public List<ResultWithScore> RankAndApplyFilterImages(IList<IImageResult> searchResult, List<string> list_auth_urls)
        {
            List<ResultWithScore> list_result = Classify(searchResult, list_auth_urls);
            return list_result;
        }

        public List<ResultWithScore> RankAndApplyFilterVideos(string searchExpression, IList<IVideoResult> searchResult, List<string> list_auth_urls)
        {
            List<ResultWithScore> list_result = ClassifyVideos(searchExpression, searchResult, list_auth_urls);
            return list_result;
        }

        //extracts plain texts from web page
        public string GetWebText(string url)
        {
            string actualText = "";
            try
            {
                WebClient clt = new WebClient();
                ConvertHtmlToText htmTxt = new ConvertHtmlToText(url);
                byte[] data = clt.DownloadData(url);
                string html = System.Text.Encoding.UTF8.GetString(data);
                actualText = htmTxt.ConvertHtml(html, url);
            }
            catch (Exception ex)
            { }
            return actualText;
        }

        // remove stop words and collect words
        public List<string> GetWords(string text)
        {
            List<string> list_words = new List<string>();
            try
            {
                if (text != "")
                {
                    KeywordAnalysis analysis = new KeywordAnalysis { Content = text };
                    int wordCount = 0;
                    var paragraphs = WordScraper.ScrapeToParagraphs(text, out wordCount);

                    //flatten list of words
                    List<Word> allWords = new List<Word>();
                    paragraphs.ForEach(p => p.Sentences.ForEach(s => allWords.AddRange(s.Words)));
                    list_words = allWords.Select(w => w.Text).ToList();
                }
            }
            catch (Exception ex)
            { }
            return list_words;
        }

        public List<ResultWithScore> Classify(IList<IImageResult> searchResult, List<string> auth_urls)
        {
            List<ResultWithScore> list_result = new List<ResultWithScore>();
            foreach (IImageResult ir in searchResult)
            {
                ResultWithScore res = new ResultWithScore();
                //check url authenticity first
                if (auth_urls.Any((ir.OriginalContextUrl).Contains)) // most authentic list entered from users
                {
                    res.imgResult = ir;
                    res.score = 1;
                    list_result.Add(res);
                }
                else if (urlFil.classifyUrl(ir.OriginalContextUrl) == 1) //check if it is an authentic domain
                { //analyse the web content
                    //extract web content from url
                    string webText = GetWebText(ir.OriginalContextUrl);
                    List<string> list_words = GetWords(webText);
                    if (list_words != null && list_words.Count > 0)
                    {
                        int score = webAnalysis.classifyText(list_words);
                        if (score >= -1) //is positive
                        {
                            res.imgResult = ir;
                            res.score = 1;
                            list_result.Add(res);
                        }
                    }
                }
            }
            return list_result;
        }

        public List<ResultWithScore> ClassifyVideos(string searchExpression, IList<IVideoResult> searchResult, List<string> auth_urls)
        {
            List<ResultWithScore> list_result = new List<ResultWithScore>();
            List<string> keywords = new List<string>() ;
            if(searchExpression != "")
            {
                keywords = searchExpression.Trim().Split(' ').ToList();
            }
            
            foreach (IVideoResult ir in searchResult)
            {
                ResultWithScore res = new ResultWithScore();
                //if none of the keyword is on the title of the video remove that video
                if (keywords.Any(ir.Title.ToLower().Contains))
                {
                    //check url authenticity first
                    if (auth_urls.Any((ir.PlayUrl).Contains)) // most authentic list entered from users
                    {
                        res.vidResult = ir;
                        res.score = 1;
                        list_result.Add(res);
                    }
                    else if (urlFil.classifyUrl(ir.PlayUrl) == 1) //check if it is an authentic domain
                    { //analyse the web content
                        //extract web content from url
                        //string webText = GetWebText(ir.OriginalContextUrl);
                        List<string> list_words = GetWords(ir.Content);
                        if (list_words != null && list_words.Count > 0)
                        {
                            int score = webAnalysis.classifyText(list_words);
                            if (score >= -1) //is positive
                            {
                                res.vidResult = ir;
                                res.score = 1;
                                list_result.Add(res);
                            }
                        }
                    }
                }
            }
            return list_result;
        }

    }

    public class AnalyseWebContent
    {
        private String pathToSWN = ConfigurationSettings.AppSettings["sentiWordNetFile"];
        //" D:\\Uni\\Dissertation related\\SentiWordNet_3.0.0\\home\\swn\\www\\admin\\dump\\new_SentiWordNet.txt"; //"C:\\Users\\MyName\\Desktop\\SentiWordNet_3.0.0\\home\\swn\\www\\admin\\dump\\SentiWordNet_3.0.0.txt";
        private Dictionary<String, Double> _dict;
               
        public AnalyseWebContent()
        {
            CollectWordsAndScore();
        }

        //collects words and their scores from the file specified in 'pathToSWN'
        public void CollectWordsAndScore()
        {
            _dict = new Dictionary<String, Double>();
            Dictionary<String, List<Double>> _temp = new Dictionary<String, List<Double>>();
            try
            {
                StreamReader csv = new StreamReader(pathToSWN);
                String line = "";
                while ((line = csv.ReadLine()) != null)
                {
                    try
                    {
                        String[] data = line.Split('\t');
                        Double score = Double.Parse(data[2]) - Double.Parse(data[3]);
                        String[] words = data[4].Split(' ');
                        foreach (String w in words)
                        {
                            String[] w_n = w.Split('#');
                            w_n[0] += "#" + data[0];
                            int index = Int32.Parse(w_n[1]) - 1;
                            if (_temp.ContainsKey(w_n[0]))
                            {
                                try
                                {
                                    List<Double> v = _temp[w_n[0]];
                                    if (index > v.Count())
                                        for (int i = v.Count(); i < index; i++)
                                            v.Add(0.0);
                                    v.Add(score);
                                    //_temp.Add(w_n[0], v); //this code addds new key value pair
                                    _temp[w_n[0]] = v; //update value of the same key
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                try
                                {
                                    List<Double> v = new List<Double>();
                                    for (int i = 0; i < index; i++)
                                        v.Add(0.0);
                                    v.Add(score);
                                    _temp.Add(w_n[0], v);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                List<String> temp = _temp.Keys.ToList();
                //for (Iterator<String> iterator = temp.iterator(); iterator.hasNext();) 
                foreach (string word in temp)
                {
                    //String word = (String) iterator.next();
                    List<Double> v = _temp[word];
                    double score = 0.0;
                    double sum = 0.0;
                    for (int i = 0; i < v.Count(); i++)
                        score += ((double)1 / (double)(i + 1)) * v[i];
                    for (int i = 1; i <= v.Count(); i++)
                        sum += (double)1 / (double)i;
                    score /= sum;
                    _dict.Add(word, score);
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }
        }

        public Double extract(String word)
        {
            Double total = new Double();
            if (_dict.ContainsKey(word + "#n"))
                total = _dict[word + "#n"] + total;
            if (_dict.ContainsKey(word + "#a"))
                total = _dict[word + "#a"] + total;
            if (_dict.ContainsKey(word + "#r"))
                total = _dict[word + "#r"] + total;
            if (_dict.ContainsKey(word + "#v"))
                total = _dict[word + "#v"] + total;
            return total;
        }

        public int classifyText(List<string> word_list)//string desc
        {
            //String[] words = desc.Split(' ');
            double totalScore = 0;
            foreach (string w in word_list)
            {
                string word = w.Replace("([^a-zA-Z\\s])", "");
                if (word == "")  // (extract(word) == null)
                    continue;
                totalScore += extract(word);
            }

            Double AverageScore = totalScore;

            if (totalScore >= 0.75)
                return 2; // "strong positive";
            else if (totalScore > 0.25 && totalScore < 0.5)
                return 1; // "positive";
            else if (totalScore >= 0 && totalScore <= 0.25)
                return 1; // "weak positive";
            else if (totalScore < 0 && totalScore >= -0.25)
                return -1; // "weak negative";
            else if (totalScore < -0.25 && totalScore >= -0.5)
                return -2; //"negative";
            else if (totalScore <= -0.75)
                return -3; //"very negative";
            return 0; // "neutral";
            
        }

    }

    public class URLFiltering
    {
        private String pathToDomain = ConfigurationSettings.AppSettings["listOfDomainFile"];
        //" D:\\Uni\\Dissertation related\\SentiWordNet_3.0.0\\home\\swn\\www\\admin\\dump\\trustedDomains.txt";
        List<string> listDom;

        public URLFiltering()
        {
            try
            {
                listDom = new List<string>();
                StreamReader csv = new StreamReader(pathToDomain);
                String line = "";
                while ((line = csv.ReadLine()) != null)
                {
                    String[] data = line.Split('\t');
                    listDom.Add("." + data[0]);
                }
            }
            catch (Exception ex)
            { }
            //return listDom;
        }

        public int classifyUrl(string url)
        {
            bool b = listDom.Any(url.Contains);
            if (b == true)
                return 1;
            else
                return 0;
        }
    }

    public class ResultWithScore
    {
        public IImageResult imgResult {get;set;}
        public IVideoResult vidResult { get; set; }
        public double score {get; set;}
    }
}
