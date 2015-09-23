module IsoTiles.Tiles

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics

type tile_name = string

type tile

val renderTile : SpriteBatch -> tile -> float32 -> float32 -> unit

type tiles

val loadTiles : ContentManager -> string -> tiles
val emptyTiles : tiles
val getTile : tiles -> tile_name -> tile
val tryGetTile : tiles -> tile_name -> tile option

type grid

val mkGrid : tiles -> int -> int -> tile_name -> grid

val setCell : grid -> int -> int -> tile -> unit

val getCell : grid -> int -> int -> tile

val renderGrid : SpriteBatch -> grid -> float32 -> float32 -> unit