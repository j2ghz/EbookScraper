namespace EbookScraper.Common

open FSharp.Data
module Scraper =

    type Url = string

    type Output =
    | Link of Url * string
    | Page of HtmlDocument
    | ConcatContent of string

    let getUrlorText (n:HtmlNode) =
        if n.AttributeValue("href") <> "" then
            Link (n.AttributeValue("href"), n.InnerText())
        else
            ConcatContent (n.InnerText())

    let rec scrape selectors o =
        match o with
        | Link (url,name) -> (sprintf "# %s \n" name) :: scrape selectors (Page (HtmlDocument.Load url))
        | Page page -> page.CssSelect(List.head selectors) |> List.map getUrlorText |> List.collect (scrape (List.tail selectors))
        | ConcatContent s -> [s]