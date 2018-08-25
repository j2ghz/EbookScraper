// Learn more about F# at http://fsharp.org

open System
open EbookScraper.Common
open FSharp.Data
open System.Text

[<EntryPoint>]
let main argv' =
    let argv = Array.toList argv'
    let url = argv |> List.head
    let content = argv |> List.rev |> List.head
    let indexes = argv |> List.tail |> List.rev |> List.tail |> List.rev
    let root = Scraper.load {url=url;name="Root"}
    //let lastIndexes = List.fold (fun docs selector -> Scraper.scrape' selector docs |> Scraper.load') [root] indexes
    //let content = Scraper.scrape' content lastIndexes |> List.fold (fun total str -> total + str.name + " \n")  ""
    let content = Scraper.scrape'' argv.Tail [root]
    System.IO.File.WriteAllText("book.txt",content)    
    0