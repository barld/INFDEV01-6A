
module Sort
    open System  

    

    let mergeBy (by: 'a -> 'b when 'b : comparison) (level:int) (col: seq<'a>) =
                
        let rec _merge (l1: 'a list) (l2: 'a list) =
            match l1, l2 with
            | [], [] -> []
            | [], _::_ -> l2
            | _::_, [] -> l1
            | h1::t1, h2::t2 -> 
                if  (by h1) < (by h2) then
                    h1 :: _merge t1 l2
                else
                    h2 :: _merge l1 t2

        let rec _branche (level:int) (l: 'a list) : 'a list =
            let length = l |> ListModule.length
            let left = ListModule.take (length/2) l
            let right = ListModule.skip (length - length/2) l
            match (left |> ListModule.length) , (right |> ListModule.length) with
            | 1, 1 -> _merge left right
            | 1, _ -> _merge left (_branche (level-1) right)
            | _, 1 -> _merge (_branche (level-1) left) right
            | _, _ -> 
                if level = 1 then
                    let a = Async.StartChild (async{ return (_branche (level-1) left)})
                    let b = Async.StartChild (async{ return (_branche (level-1) right)})
                    let ra = Async.RunSynchronously a |> Async.RunSynchronously
                    let rb = Async.RunSynchronously b |> Async.RunSynchronously
                    _merge ra rb
                else
                    _merge (_branche (level-1) left) (_branche (level-1) right)



        let branches = col |> ListModule.fromSeq    
        branches |> (_branche level) |> ListModule.toSeq

    //for C#
    let MergeBy (by: System.Func<'a, 'b>) col =
        let by = fun x -> by.Invoke(x)
        mergeBy by 1 col


