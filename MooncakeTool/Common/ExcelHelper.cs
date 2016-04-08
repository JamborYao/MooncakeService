using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MooncakeTool.Models;

namespace MooncakeTool.Common
{
    public class ExcelHelper
    {
        public static BaseThread ConvertCSVToForumThreads(string[] headers, string[] item, BaseThread thread)
        {
            if (headers.Count() != item.Count()) { throw new Exception("header.count not equal item.count"); }
            int num = 0;
            foreach (var cell in item)
            {
                CheckHeader(headers[num], item[num], thread);
                num++;
            }
            num = 0;
            return thread;
        }

        public static void CheckHeader(string header, string value, BaseThread thread)
        {
            switch (header)
            {
                case "Rank":
                    if (value != "")
                        thread.Rank = Convert.ToInt16(value);
                    break;
                case "Weighted Impact":
                    if (value != "")
                        thread.WeightImpact = Convert.ToDecimal(value);
                    break;
                case "Tech category ":
                    thread.TechCategory = (string)value;
                    break;

                case "Issue Type":
                    thread.IssueType = (string)value;
                    break;
                case "Thread Title":
                    thread.ThreadTitle = (string)value;
                    break;
                case "Thread Url":
                    thread.ThreadUrl = (string)value;
                    break;
                case "Avg Views per Day":
                    if (value != "")
                        thread.AvgPageView = Convert.ToDecimal(value);
                    break;
                case "Views":
                    if (value != "")
                        thread.Views = Convert.ToInt16(value);
                    break;
                case "Replies":
                    if (value != "")
                        thread.Replies = Convert.ToInt16(value);
                    break;
                case "Unique Posters":
                    if (value != "")
                        thread.UniquePoster = Convert.ToInt16(value);
                    break;
                case "Subscribers":
                    if (value != "")
                        thread.Subscriber = Convert.ToInt16(value);
                    break;
                case "Age":
                    if (value != "")
                        thread.Age = Convert.ToInt16(value);
                    break;
                case "Created On":
                    if (value != "" && value != "-")
                        try
                        {
                            thread.CreateOn = Convert.ToDateTime(value);
                        }
                        catch
                        {
                            throw new Exception(value + "is not a correct data format ");
                        }

                    break;
                case "Time to Initial Response":
                    if (value != "")
                        thread.IRT = Convert.ToDecimal(value);
                    break;
                case "Last Reply":
                    if (value != "" && value != "-")
                        try
                        {
                            thread.LastReply = Convert.ToDateTime(value);
                        }
                        catch
                        {
                            throw new Exception(value + "is not a correct data format ");
                        }
                    break;
                case "Answered":
                    if (value != "")
                        thread.Answered = value.ToLower() == "yes" ? true : false;
                    break;
                case "Answered By":
                    thread.AnsweredBy = (string)value;
                    break;
                case "Thread Type":
                    thread.ThreadType = (string)value;
                    break;
                case "Trend":
                    thread.Trend = (string)value;
                    break;
                case "Trend Url":
                    thread.TrendUrl = (string)value;
                    break;
                case "What CSS have done (Choose one item)":
                    thread.CSSDone = (string)value;
                    break;
                case "What exactly Customer are looking for (VOC)":
                    thread.CustomerNeeded = (string)value;
                    break;
                case "Encountered difficulties":
                    thread.Difficult = (string)value;
                    break;
            }
        }

    }
}