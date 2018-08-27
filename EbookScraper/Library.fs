namespace EbookScraper.Common

open FSharp.Data
module Scraper =

    type Selector = string

    type Url = string

    type Processor =
    | Root of Url
    | Index of Selector
    | Content of Selector

    type Output =
    | Link of Url * string
    | Page of HtmlDocument
    | ConcatContent of string

    let toProcessors argv =
        Root (List.head argv)::(argv|> List.tail|> List.rev |> List.tail |> List.rev |> List.map (fun str -> Index str) |> List.append [Content (List.last argv)] )

    let load link = HtmlDocument.Load(link.url)

    let load' links =
        List.map load links

    let extractText (node:HtmlNode) =
        {
            url = node.AttributeValue("href");
            name = node.Name();
        }

    let scrape selector (document:HtmlDocument) =
        document.CssSelect(selector)
        |> List.map extractText

    let scrape' selector documents = List.collect (scrape selector) documents

    let rec scrape'' selectors documents =
        match selectors with
        | [selector] -> scrape' selector documents |> List.fold (fun state link -> state + link.name) ""
        | selector::tail -> scrape' selector documents |> load' |> scrape'' tail
        | [] -> ""

    let getUrlorText (n:HtmlNode) =
        if n.AttributeValue("href") <> "" then
            Link (n.AttributeValue("href"), n.InnerText())
        else
            ConcatContent (n.InnerText())

    let rec scrape''' selectors o =
        match o with
        | Link (url,name) -> scrape''' selectors (Page (HtmlDocument.Load url))
        | Page page -> page.CssSelect(List.head selectors) |> List.map getUrlorText |> List.collect (scrape''' (List.tail selectors))
        | ConcatContent s -> [s]