open System

#load "Scripts/load-project-debug.fsx"

let rnd = new Random()

let rl = [0..1000*1000] |> List.map rnd.Next

let rec sum acc l =
    match l with
    | [] -> acc
    | h::t -> sum (acc+h) t

let rec sum2 l =
    match l with
    | [] -> 0
    | h::t -> h + sum2 t

rl |> sum 0
rl |> sum2


#time
for i in [0..100] do
    Sort.mergeBy (fun x -> x) 0 rl |> ignore

#time

#time
for i in [0..100] do
    Sort.mergeBy (fun x -> x) 1 rl |> ignore

#time


let length l =
    let rec _length acc l =
        match l with
        | [] -> acc
        | _::t -> _length (acc+1) t
    _length 0 l

[0..1000 * 1000] |> length






let sleepWorkflow n  = async{
    printfn "Starting sleep %i workflow at %O" n DateTime.Now.TimeOfDay
    do! Async.Sleep 2000
    printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay

    return 1
    }

//Async.RunSynchronously sleepWorkflow  


let nestedWorkflow  = async{

    printfn "Starting parent"
    let! childWorkflow = Async.StartChild (sleepWorkflow 1)
    let! childWorkflow = Async.StartChild (sleepWorkflow 2)

    // give the child a chance and then keep working
    do! Async.Sleep 100
    printfn "Doing something useful while waiting "

    // block on the child
    let! result = childWorkflow

    // done
    printfn "Finished parent" 
    }

// run the whole workflow
Async.RunSynchronously nestedWorkflow  


open System

let rnd = new Random()

let insertionSort (ar:int[]) =
    let lngth = ar.Length
    for i in 1..(lngth-1) do
        let tmp = ar.[i]
        let mutable j = i-1
        while j >= 0 && ar.[j] > tmp do
            ar.[j+1] <- ar.[j]
            j <- j-1
        ar.[j+1] <- tmp
    ()

let rl = [for i in 0..10 -> rnd.Next()] |> List.toArray
rl |> insertionSort
printfn "%A" rl