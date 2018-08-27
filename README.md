# EbookScraper
Produces a txt with markdown headings for ToC.

## Prerequisites
1. Install [.NET Core 2.1](https://www.microsoft.com/net/download)
2. Verify by running `dotnet --info`

## Usage
Example: `dotnet run -- "http://moonbunnycafe.com/demon-noble-girl-story-of-a-careless-demon/" ".entry-content a[href*=-chapter-]" ".entry-content p"`
### Parameters
- First is a starting url
- Then 0 or more index selectors. This selector should select tags that have a href attribute linking to the next index/content page
- Last is a content selector. Each element's inner text will be on a new line.
