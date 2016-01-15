/// Test code 

#I @"..\packages\"
#r @"FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
#r @"..\HackerNews\bin\Debug\HackerNews.dll"

open FSharp.Data
open System
open HackerNews.Utils

fsi.ShowDeclarationValues <- true

#time "on"

type Questions = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

//let csQuestions = """https://api.stackexchange.com/2.2/questions?site=stackoverflow&tagged=C%23"""
//Questions.Load(csQuestions).Items |> Seq.iter (fun q -> printfn "%s" q.Title)

let questionQuery = """https://api.stackexchange.com/2.2/questions?site=stackoverflow"""
let tagged tags query =
    // join the tags in a ; separated string
    let joinedTags = tags |> String.concat ";"
    sprintf "%s&tagged=%s" query joinedTags

let page p query = sprintf "%s&page=%i" query p

let pageSize s query = sprintf "%s&pagesize=%i" query s

let extractQuestions (query:string) = Questions.Load(query).Items

let ``C#`` = "C%23"
let ``F#`` = "F%23"

let fsSample =
    questionQuery
    |> tagged [``F#``]
    |> pageSize 100
    |> extractQuestions

let csSample =
    questionQuery
    |> tagged [``C#``]
    |> pageSize 100
    |> extractQuestions

let analyzeTags (qs:Questions.Item seq) =
    qs
    |> Seq.collect (fun question -> question.Tags)
    |> Seq.countBy id
    |> Seq.filter (fun (_,count) -> count > 2)
    |> Seq.sortBy (fun (_,count) -> -count)
    |> Seq.iter (fun (tag,count) -> printfn "%s,%i" tag count)

analyzeTags fsSample
analyzeTags csSample



add 42 1



// Recursive Types Testing


type Book = {title: string; price: decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType: ChocolateType ; price: decimal}

type WrappingPaperStyle = 
    | HappyBirthday
    | HappyHolidays
    | SolidColor

type Gift =
    | Book of Book
    | Chocolate of Chocolate 
    | Wrapped of Gift * WrappingPaperStyle
    | Boxed of Gift 
    | WithACard of Gift * message:string

// a Book
let wolfHall = {title="Wolf Hall"; price=20m}

// a Chocolate
let yummyChoc = {chocType=SeventyPercent; price=5m}

// A Gift

// A Gift
let christmasPresent = Wrapped (Boxed (Chocolate yummyChoc), HappyHolidays)
//  Wrapped (
//    Boxed (
//      Chocolate {chocType = SeventyPercent; price = 5M}),
//    HappyHolidays)

let birthdayPresent2 = WithACard (Boxed (Book wolfHall), "You suck it")

let christmasPresent2 = WithACard (Boxed (Wrapped (Book wolfHall, HappyHolidays)), "what the fuck")
let birthdayPresent = WithACard (Wrapped (Book wolfHall, HappyBirthday), "Happy Birthday")
//  WithACard (
//    Wrapped (
//      Book {title = "Wolf Hall"; price = 20M},
//      HappyBirthday),
//    "Happy Birthday")


let rec description gift =
    match gift with 
    | Book book -> 
        sprintf "'%s with a cost of %A'" book.title book.price
    | Chocolate choc -> 
        sprintf "%A chocolate with a cost of %A" choc.chocType choc.price
    | Wrapped (innerGift,style) -> 
        sprintf "%s wrapped in %A paper" (description innerGift) style
    | Boxed innerGift -> 
        sprintf "%s in a box" (description innerGift) 
    | WithACard (innerGift,message) -> 
        sprintf "%s with a card saying '%s'" (description innerGift) message

birthdayPresent |> description  
// "'Wolf Hall' wrapped in HappyBirthday paper with a card saying 'Happy Birthday'"

christmasPresent |> description  
// "SeventyPercent chocolate in a box wrapped in HappyHolidays paper"

birthdayPresent2 |> description
// "'Wolf Hall' in a box with a card saying 'You suck it'"

christmasPresent2 |> description 
//  "'Wolf Hall with a cost of 20M' wrapped in HappyHolidays paper in a box with a card saying 'what the fuck'"

let rec totalCost gift =
    match gift with 
    | Book book -> 
        book.price
    | Chocolate choc -> 
        choc.price
    | Wrapped (innerGift,style) -> 
        (totalCost innerGift) + 0.5m
    | Boxed innerGift -> 
        (totalCost innerGift) + 1.0m
    | WithACard (innerGift,message) -> 
        (totalCost innerGift) + 2.0m


birthdayPresent |> totalCost 
// 22.5m

christmasPresent |> totalCost 
// 6.5m

birthdayPresent2 |> totalCost
// 23.0m


let rec whatsInside gift =
    match gift with 
    | Book book -> 
        sprintf "A book worth %A and titled %s" book.price book.title
    | Chocolate choc -> 
        sprintf "Some %A chocolate worth %A" choc.chocType choc.price
    | Wrapped (innerGift,style) -> 
        whatsInside innerGift
    | Boxed innerGift -> 
        whatsInside innerGift
    | WithACard (innerGift,message) -> 
        whatsInside innerGift

birthdayPresent |> whatsInside 
// "A book worth 20M and titled Wolf Hall"

christmasPresent |> whatsInside 
// "Some SeventyPercent chocolate worth 5M"




// Parameterizing the code

let rec cataGift fBook fChocolate fWrapped fBox fCard gift =
    match gift with 
    | Book book -> 
        fBook book
    | Chocolate choc -> 
        fChocolate choc
    | Wrapped (innerGift,style) -> 
        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
        fWrapped (innerGiftResult,style)
    | Boxed innerGift -> 
        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
        fBox innerGiftResult 
    | WithACard (innerGift,message) -> 
        let innerGiftResult = cataGift fBook fChocolate fWrapped fBox fCard innerGift
        fCard (innerGiftResult,message) 

let totalCostUsingCata gift =
    let fBook (book:Book) = 
        book.price
    let fChocolate (choc:Chocolate) = 
        choc.price
    let fWrapped  (innerCost,style) = 
        innerCost + 0.5m
    let fBox innerCost = 
        innerCost + 1.0m
    let fCard (innerCost,message) = 
        innerCost + 2.0m
    // call the catamorphism
    cataGift fBook fChocolate fWrapped fBox fCard gift

birthdayPresent |> totalCostUsingCata 
// 22.5m

let descriptionUsingCata gift =
    let fBook (book:Book) = 
        sprintf "'%s'" book.title 
    let fChocolate (choc:Chocolate) = 
        sprintf "%A chocolate" choc.chocType
    let fWrapped (innerText,style) = 
        sprintf "%s wrapped in %A paper" innerText style
    let fBox innerText = 
        sprintf "%s in a box" innerText
    let fCard (innerText,message) = 
        sprintf "%s with a card saying '%s'" innerText message
    // call the catamorphism
    cataGift fBook fChocolate fWrapped fBox fCard gift

birthdayPresent |> descriptionUsingCata  
// "'Wolf Hall' wrapped in HappyBirthday paper with a card saying 'Happy Birthday'"

christmasPresent |> descriptionUsingCata  
// "SeventyPercent chocolate in a box wrapped in HappyHolidays paper"