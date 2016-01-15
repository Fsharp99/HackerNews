#I @"..\Projects\"
#r @"FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
#r @"..\HackerNews\bin\Debug\HackerNews.dll"
#r @"ServiceStack.Text.3.9.11\lib\net35\ServiceStack.Text.dll"
#r @"ServiceStack.Common.3.9.11\lib\net35\ServiceStack.Interfaces.dll"
#r @"ServiceStack.Common.3.9.11\lib\net35\ServiceStack.Common.dll"

open FSharp.Data
open System
open HackerNews.Utils
open ServiceStack.Text


fsi.ShowDeclarationValues <- true

#time "on"
type num = float

[<CLIMutable>]
type Comment = 
    {   by          : string
        id          : int
        kids        : int[]
        parent      : int
        text        : string
        time        : int
    }


let testString = 
    """{
      "by" : "cjensen",
      "id" : 10869500,
      "kids" : [ 10870030, 10870045 ],
      "parent" : 10869396,
      "text" : "Summary:<p>802.11ah: Better range, lower bandwidth<p>802.11ad: Better bandwidth. Doesn&#x27;t go through walls.<p>802.11ax: Successor to 11ac expected in 2019",
      "time" : 1452301129,
      "type" : "comment"
    } """

let testString2 = 
    """{
      "by" : "cjensen",
      "id" : 10869500,
      "parent" : 10869396,
      "text" : "Summary:<p>802.11ah: Better range, lower bandwidth<p>802.11ad: Better bandwidth. Doesn&#x27;t go through walls.<p>802.11ax: Successor to 11ac expected in 2019",
      "time" : 1452301129,
      "type" : "comment"
    } """


let x = ServiceStack.Text.JsonSerializer.DeserializeFromString<Comment>(testString)
let y = ServiceStack.Text.JsonSerializer.DeserializeFromString<Comment>(testString2)
        |> fun x -> if isNull x.kids then x.kids <- [||]


let x = Http.RequestString("https://hacker-news.firebaseio.com/v0/item/10869500.json?print=pretty")

[<Literal>]
let urlIds          = """https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"""
let itemUrlMaker    = sprintf "https://hacker-news.firebaseio.com/v0/item/%i.json?print=pretty"
type IdType         = JsonProvider<urlIds>
type Story          = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/8863.json?print=pretty""">
type Comment        = JsonProvider<"https://hacker-news.firebaseio.com/v0/item/10869500.json?print=pretty">

let ids = IdType.Load(urlIds)

let url = itemUrlMaker 10872852
let story1 = Story.Load(url)
let commentIds = story1.Kids
let loadComment (id:int) = Comment.Load(itemUrlMaker id)

story1.Kids

loadComment 10872852

//let x = loadComment 10872791
let y = loadComment 10872673
y.Kids




type Car = 
    {   color: string
        make: string
        model: string
        year: int
    }


let blueToyota = {Car.color = "blue"; make = "camry"; model = "SL"; year = 2014}
let redToyota = {Car.color = "red"; make = "camry"; model = "SL"; year = 2014}

let f (x:Car) = sprintf "this is the cars color: %s" x.color

f redToyota



let x = loadComment 10867446

//todo :don't understand this
let topLevelComments = 
    story1.Kids |> Array.map (loadComment >> fun x -> x.Text |> beautifyComment)



let whatever = Comment.Load(10867446 |> itemUrlMaker)

let p = new System.Diagnostics.ProcessStartInfo("chrome.exe", story1.Kids)
System.Diagnostics.Process.Start (p) |> ignore




let fsStory = """https://hacker-news.firebaseio.com/v0/item/8863.json&tagged=F%23"""


[<Literal>]
let sample = """{
  "by" : "dhouston",
  "descendants" : 71,
  "id" : 8863,
  "kids" : [ 8952, 9224, 8917, 8884, 8887, 8943, 8869, 8958, 9005, 9671, 8940, 9067, 8908, 9055, 8865, 8881, 8872, 8873, 8955, 10403, 8903, 8928, 9125, 8998, 8901, 8902, 8907, 8894, 8878, 8870, 8980, 8934, 8876 ],
  "score" : 111,
  "time" : 1175714200,
  "title" : "My YC app: Dropbox - Throw away your USB drive",
  "type" : "story",
  "url" : "http://www.getdropbox.com/u/2/screencast.html"
}"""

type HardCodedStory = JsonProvider<sample> 

[<Literal>]
let storyQuery = "https://hacker-news.firebaseio.com/v0/item/8863.json?print=pretty"

let hNStory = HardCodedStory.Load(storyQuery)


let ``F#`` = "F%23"

let fsSample =
    storyQuery
    |> tagged [``F#``]
    |> pageSize 100
    |> extractQuestions