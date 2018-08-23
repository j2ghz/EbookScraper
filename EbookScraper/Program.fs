// Learn more about F# at http://fsharp.org

open System
open EbookScraper.Common
open FSharp.Data

[<EntryPoint>]
let main argv' =
    let argv = ["http://moonbunnycafe.com/demon-noble-girl-story-of-a-careless-demon/";".entry-content a[href*=-chapter-]";".entry-content"]
    let url = argv |> List.head
    let content = argv |> List.rev |> List.head
    let indexes = argv |> List.tail |> List.rev |> List.tail |> List.rev
    let root = Scraper.load {url=url;name="Root"}
    let lastIndexes = List.fold (fun docs selector -> Scraper.scrape' selector docs |> Scraper.load') [root] indexes
    let content = Scraper.scrape' content lastIndexes |> List.fold (fun r s -> r + s.name + "\n") ""
    printfn "%s" content
    0