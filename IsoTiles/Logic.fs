module IsoTiles.Logic

open IsoTiles.Base

open Microsoft.Xna.Framework


/// A single player-controlled character.
type Character () =

    // health state
    let mutable health = 10
    let mutable max_health = 10

    // position state
    let mutable x = 0
    let mutable y = 0

    // movement state
    let mutable speed = 1

    interface IHealth with
        member __.Health with get () = health
                          and set h  = health <- h

        member __.MaxHealth with get () = max_health
                             and set h  = max_health <- max_health

    interface IPosition with
        member __.X with get () = x
                     and set x' = x <- x'

        member __.Y with get () = y
                     and set y' = y <- y'