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
    | Page of HtmlDocument
    | ConcatenatedContent of string

    type Link = {url:string; name:string}

    let toProcessors argv =
        Root (List.head argv)::(argv|> List.tail|> List.rev |> List.tail |> List.rev |> List.map (fun str -> Index str) |> List.append [Content (List.last argv)] )

    let load link = HtmlDocument.Load(link.url)

    let load' links =
        List.map load links

    let scrape selector (document:HtmlDocument) =
        document.CssSelect(selector)
        |> List.map (fun n -> {url = n.AttributeValue("href"); name=n.InnerText()})

    let scrape' selector documents = List.collect (scrape selector) documents

    let rec scrape'' selectors documents =
        match selectors with
        | [selector] -> scrape' selector documents |> List.fold (fun state link -> state + link.name) ""
        | selector::tail -> scrape' selector documents |> load' |> scrape'' tail
        | [] -> ""

