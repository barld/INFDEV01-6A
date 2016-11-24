module ListModule
    let length l =
        let rec _length acc l =
            match l with
            | [] -> acc
            | _::t -> _length (acc+1) t
        _length 0 l

    let map (f:'a -> 'b) (list: 'a list) =
        let rec _map (cont: 'b list -> 'b list) (f:'a -> 'b) (list: 'a list) : 'b list =
            match list with
            | [] -> cont([])
            | h::t -> _map (fun acc -> cont(f h::acc)) f t
        _map id f list

    let fromSeq s =
        [for item in s -> item]

    let rec fold f acc l =
        match l with
        | [] -> acc
        | h::t -> fold f (f h acc) t

    let reduce f l =
        match l with
        | [] -> failwith "empty list"
        | h::t -> fold f h t

    let toSeq l =
        seq{for item in l -> item }

    let rec take length list =
        if length = 0 then
            []
        else
            match list with
            | [] -> failwith "list not long enough"
            | h::t -> h :: take (length-1) t

    let rec skip items list =
        if items = 0 then
            list
        else
            match list with
            | [] -> failwith "list not long enough"
            | h::tail -> skip (items-1) tail
