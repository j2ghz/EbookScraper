// Learn more about F# at http://fsharp.org

open System
open EbookScraper.Common
open FSharp.Data
open System.Text

[<EntryPoint>]
let main argv' =
    let argv = Array.toList argv'
    let url = argv |> List.head
    let indexes = argv |> List.tail
    let content = Scraper.scrape indexes (Scraper.Output.Link (url,""))
    System.IO.File.WriteAllLines("book.txt",content)
    0