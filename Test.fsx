/// Test code 

#I @"..\packages\"
#r @"FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
#r @"..\HackerNews\bin\Debug\HackerNews.dll"

open FSharp.Data
open System
open HackerNews.Utils

fsi.ShowDeclarationValues <- true

#time "on"
type num = float

//  For Job Data
[<Literal>] 
let currentJobs      = """https://hacker-news.firebaseio.com/v0/jobstories.json?print=pretty"""
type JobIds          = JsonProvider<currentJobs>
// Can't use because not well defined Joson Docs:   type JobData = JsonProvider<"https://hacker-news.firebaseio.com/v0/item/10786628.json?print=pretty">
let jobs             = JobIds.Load(currentJobs)

//  For Top Story Data 
[<Literal>]
let currentStory     = """https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"""
type currentStoryIds = JsonProvider<currentStory>
// Can't use because not well defiend Json Docs: type StoryUrl = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/8863.json?print=pretty""">
let story            = currentStoryIds.Load(currentStory)

let storyJobUrlMaker = sprintf "https://hacker-news.firebaseio.com/v0/item/%i.json?print=pretty" 

let jsonContains (x:JsonValue) (propertyName:string) (keyword:string) = 
    match x.TryGetProperty(propertyName) with
    | Some s -> s.AsString().ToLower().Trim().Contains(keyword.ToLower().Trim())
    | _ -> false 

//Find Jobs of Interest
let allJobs = jobs |> Array.map (storyJobUrlMaker >> JsonValue.Load)
            
let findJobs keyword =    
    allJobs |> Array.filter (fun x -> jsonContains x "text" keyword  || jsonContains x "title" keyword )

let jobsOfInterest = [|"fsharp"; "f#"; "functional"; "python"; "c#"|]

let myJobs = jobsOfInterest |> Array.map findJobs 

//Find Stories of Interest 
let allStory =
    story
    |> Array.take 50
    |> Array.map (storyJobUrlMaker >> JsonValue.Load)
 
let wordsOfInterest = [|"open"; "spacex"; "tesla"; "machine learning"; "f#";"fsharp"; "solar"; "robotics"; "3d printing"|]

let doesStoryHaveAnyOfTheseKeywords keywords story = 
    match keywords |> Array.filter (fun keyword -> jsonContains story "title" keyword) with
    | [||] -> false
    | _ -> true

allStory |> Array.filter (doesStoryHaveAnyOfTheseKeywords wordsOfInterest)

allStory |> Array.filter (doesStoryHaveAnyOfTheseKeywords [|"fbi"|])
allStory |> Array.filter (doesStoryHaveAnyOfTheseKeywords [|"Neural Networks"|])


"Neural Networks    adasdas" .Contains(" Neural Networks")




type TreeBranch = 
    {   length: float
        branches: TreeBranch []
    }



let xs = jobs |> Array.map (storyJobUrlMaker >> JobData.Load)

let ys = 
    jobs
    |> Array.map (fun i -> JobData.AsyncLoad(storyJobUrlMaker i))
    |> Async.Parallel |> Async.RunSynchronously


ys |> Array.map (fun x -> x.Text)


jobs |> Array.map (storyJobUrlMaker >> Jobs.Load)

Jobs.Load("https://hacker-news.firebaseio.com/v0/item/10786628.json?print=pretty")




findJobs "javascript"
findJobs "scala "
findJobs "python"
findJobs "c#"
                            
z.TryGetProperty("text")



//This does not count towards my 30 min!!!! You're off the clock!!!


let job1 = Jobs.Load(jobUrl)


[<Literal>]
let urlIds = """https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"""
type IdType  = JsonProvider<urlIds>
type Story   = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/8863.json?print=pretty""">
type Comment = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/10869500.json?print=pretty""">
type Ask     = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/121003.json?print=pretty""">
type Job     = JsonProvider<"""https://hacker-news.firebaseio.com/v0/item/192327.json?print=pretty""">


type C = Circle of int | Rectangle of int * int

let c = Circle 99                              // "construct"
match c with                                   
| Circle c1 -> printf "circle of radius %i" c1 // "deconstruct"
| Rectangle (c2,c3) -> printf "%i %i" c2 c3    // "deconstruct"



let ids = IdType.Load(urlIds)

let itemUrlMaker = sprintf "https://hacker-news.firebaseio.com/v0/item/%i.json?print=pretty"

let url = itemUrlMaker 10866884
let story1 = Story.Load(url)
let commentIds = story1.Kids

let loadComment (id:int) = Comment.Load(itemUrlMaker id)

let x = loadComment 10867446
x.By

let topLevelComments = 
    story1.Kids |> Array.map (loadComment >> fun x -> x.Text |> beautifyComment)



let whatever = Comment.Load(10867446 |> itemUrlMaker)

//let p = new System.Diagnostics.ProcessStartInfo("chrome.exe", story1.Kids)
//System.Diagnostics.Process.Start (p) |> ignore

// Job Search

let job = Job.GetSample()

let urlForJob = itemUrlMaker 192327
let job1 = Job.Load(urlForJob)
let textJobListing = job1.Text 

let searchAllJobs = Job.Load().Items |> Seq.iter (fun q -> printfn "%s" q.Title)

let maxItem = """https://hacker-news.firebaseio.com/v0/maxitem"""



let fsStory = """https://hacker-news.firebaseio.com/v0/item/8863.json&tagged=F%23"""

q
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



// testing building a tree


#r "System.Runtime.Serialization.dll"

open System.Runtime.Serialization
open System.Runtime.Serialization.Json

let fromJson<'a> str = 
    let serializer = new DataContractJsonSerializer(typeof<'a>)
    let encoding = System.Text.UTF8Encoding()
    use stream = new System.IO.MemoryStream(encoding.GetBytes(s=str))
    let obj = serializer.ReadObject(stream) 
    obj :?> 'a


