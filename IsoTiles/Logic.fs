module IsoTiles.Logic

open IsoTiles.Base

open Microsoft.Xna.Framework


/// A single player-controlled character.
type Character () =

    // health state
    let mutable health = 10
    let mutable max_health = 10

    // position state
    let mutable pos = { X=0; Y=0 }

    // movement state
    let mutable speed = 1

    interface IHealth with
        member __.Health with get () = health
                          and set h  = health <- h

        member __.MaxHealth with get () = max_health
                             and set h  = max_health <- max_health

    interface IPosition with
        member __.Position with get ()   = pos
                            and set pos' = pos <- pos'


type Party () =

    let mutable members = [] : Character list

    member __.AddMember char =
        members <- char :: members

    /// Returns the members that are flanking the current position, or the
    /// empty sequence if there are none.
    member __.Flanking (pos: Position) =
        let rec FindFlanking = function
                | n :: ns -> seq { let opposite = OppositeAbout pos (Position n)
                                   match List.tryFind (fun n' -> Position n' = opposite) ns with
                                   | Some n' -> yield n, n'
                                   | None    -> ()

                                   yield! FindFlanking ns
                                 }
                | [] -> Seq.empty

        let Candidate = InNeighborhood pos

        in members |> List.filter (fun c -> Candidate (Position c))
                   |> FindFlanking