
module Sort
    open System  

    let mergeBy (by: 'a -> 'b when 'b : comparison) (col: seq<'a>) =
        let rec _merge (l1: 'a list) (l2: 'a list) =
            match l1, l2 with
            | [], [] -> []
            | [], h::t -> h::_merge [] t
            | h::t, [] -> h::_merge t []
            | h1::t1, h2::t2 -> 
                if  (by h1) < (by h2) then
                    h1 :: _merge t1 l2
                else
                    h2 :: _merge l1 t2

        let branches = col |> ListModule.fromSeq |> ListModule.map (fun item -> [item]) 
        branches |> ListModule.reduce _merge |> List.toSeq

    //for C#
    let MergeBy (by: System.Func<'a, 'b>) col =
        let by = fun x -> by.Invoke(x)
        mergeBy by col

