
/// Base functionality for the engine.
module IsoTiles.Base

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// Things that can be rendered using a SpriteBatch.
[<Interface>]
type IRender =
    abstract Render : SpriteBatch -> float32 -> float32 -> unit

/// Utility function for invoking the Render method of a class that implements IRender.
let render (thing : IRender) (sb : SpriteBatch) x y = thing.Render sb x y


/// Things that have a world position
[<Interface>]
type IPosition =
    abstract X : int with get, set
    abstract Y : int with get, set

let get_x (thing : IPosition) = thing.X
let set_x x (thing : IPosition) = thing.X <- x

let get_y (thing : IPosition) = thing.Y
let set_y y (thing : IPosition) = thing.Y <- y

let move_up    (thing : IPosition) = thing.Y <- thing.Y - 1
let move_down  (thing : IPosition) = thing.Y <- thing.Y + 1
let move_left  (thing : IPosition) = thing.X <- thing.X - 1
let move_right (thing : IPosition) = thing.X <- thing.X + 1


/// Utility function to clamp a value between two extremes.
/// NOTE: specialized to int, as I'm not sure about the runtime performance of
/// comparable.
let clamp lo hi (x : int) = min hi (max lo x)

// Things that have HP
[<Interface>]
type IHealth =
    abstract MaxHealth : int with get, set
    abstract Health    : int with get, set

/// Apply a function to an IHealth.
let modify_health (f : int -> int) (thing : IHealth) =
    thing.Health <- clamp 0 thing.MaxHealth (f thing.Health)

/// Check to see if Health is positive.
let is_alive (thing : IHealth) = thing.Health > 0

/// Check to see if Health has dropped to zero.
let is_dead (thing : IHealth) = thing.Health <= 0

/// Add some damage to something that keeps track of HP.
let add_damage d (thing : IHealth) = modify_health (fun h -> h - d) thing