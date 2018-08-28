// Learn more about F# at http://fsharp.org

open System
open FSharp.Data
open System.Text

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

[<EntryPoint>]
let main argv' =
    let argv = Array.toList argv'
    let url = argv |> List.head
    let indexes = argv |> List.tail
    let content = scrape indexes (Output.Link (url,""))
    System.IO.File.WriteAllLines("book.txt",content)
    0