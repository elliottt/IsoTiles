module IsoTiles.Tiles

open IsoTiles.Base

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics

type tile_name = string

[<Class>]
type Tile =
    interface IRender

type Tiles =
    new : ContentManager * string -> Tiles
    member GetTile    : tile_name -> Tile
    member TryGetTile : tile_name -> Tile option

[<Interface>]
type IGrid =
    abstract SetCell   : int -> int -> Tile -> unit
    abstract GetCell   : int -> int -> Tile option
    abstract ClearCell : int -> int -> unit

val set_cell   : IGrid -> int -> int -> Tile -> unit
val get_cell   : IGrid -> int -> int -> Tile option
val clear_cell : IGrid -> int -> int -> unit

type IsoGrid =
    new : Tiles * int * int -> IsoGrid
    interface IGrid
    interface IRender