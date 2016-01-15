
#I @"..\packages\"
#r @"FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
#r @"..\HackerNews\bin\Debug\HackerNews.dll"

open FSharp.Data
open System

//fsi.ShowDeclarationValues <- true

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
            
let jobsOfInterest = [|"fsharp"; "f#"; "functional"; "python"; "machine learning"; "c#"|]

let doesJobHaveAnyOfTheseKeywords keywords job =
    match keywords |> Array.filter (fun keyword -> jsonContains job "text" keyword || jsonContains job "title" keyword ) with
    | [||] -> false
    | _ -> true 

allJobs |> Array.filter (doesJobHaveAnyOfTheseKeywords jobsOfInterest) 

//Find Stories of Interest 
let allStory =
    story
    |> Array.take 50
    |> Array.map (storyJobUrlMaker >> JsonValue.Load)
 
let wordsOfInterest = [|"spacex"; "tesla"; "machine learning"; "f#";"fsharp"; "solar"; "ligo"; "gravitational"; "robotics"; "3d printing"|]

let doesStoryHaveAnyOfTheseKeywords keywords story = 
    match keywords |> Array.filter (fun keyword -> jsonContains story "title" keyword) with
    | [||] -> false
    | _ -> true

allStory |> Array.filter (doesStoryHaveAnyOfTheseKeywords wordsOfInterest)