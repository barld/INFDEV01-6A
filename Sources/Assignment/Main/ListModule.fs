module ListModule
    let rec map f list =
        match list with
        | [] -> []
        | h::t -> f h :: map f t

    let fromSeq s =
        [for item in s -> item]

    let rec fold f acc l =
        match l with
        | [] -> acc
        | h::t -> f h (fold f acc t)

    let reduce f l =
        match l with
        | [] -> failwith "empty list"
        | h::t -> fold f h t

    let toSeq l =
        seq{for item in l -> item }
        