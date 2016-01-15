module HackerNews.Utils

    open System

    let beautifyComment s           = System.Web.HttpUtility.HtmlDecode(s).Replace("<p>","\n")
    let unixTsToDatetime (x:int)    = DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(float x)


