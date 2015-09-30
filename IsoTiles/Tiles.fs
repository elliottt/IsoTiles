module IsoTiles.Tiles

open System
open FSharp.Data
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics


type tile_name = string

type tile = {
    width:   float32;
    height:  float32;
    offsetX: float32;
    offsetY: float32;
    texture: Texture2D;
    sourceRect: Nullable<Rectangle>;
}

(* Render a tile to the given coordinate *)
let renderTile (sprites : SpriteBatch) tile x y =
    let real_x = x + tile.offsetX
    let real_y = y + tile.offsetY
    sprites.Draw(tile.texture,Vector2 (real_x,real_y), tile.sourceRect, Color.White)

type tiles = {
    tiles:  Map<string,tile>
    width:  float32;
    height: float32;
}

type TextureAtlas = XmlProvider<"""
<TextureAtlas imagePath="foo.png" width="100" height="100">
  <SubTexture name="region1" x="0"  y="0"  width="10" height="10" />
  <SubTexture name="region2" x="10" y="10" width="20" height="20" offset-x="2" />
  <SubTexture name="region3" x="10" y="10" width="20" height="20" offset-y="3" />
  <SubTexture name="region4" x="10" y="10" width="20" height="20" offset-x="2" offset-y="3" />
</TextureAtlas>
""">

let emptyTiles = { tiles = Map.empty; width = 0.0f; height = 0.0f }

let option (def:'b) (f:'a -> 'b) (opt:'a option) : 'b =
    match opt with
    | Some a -> f a
    | None   -> def

let loadTile width height tex (child : TextureAtlas.SubTexture) =
    { width   = width;
      height  = height;
      offsetX = option 0.0f float32 child.OffsetX;
      offsetY = option 0.0f float32 child.OffsetY;
      texture = tex;
      sourceRect = Nullable<Rectangle>(Rectangle(int child.X,int child.Y,int child.Width,int child.Height)) }

(* NOTE: null can creep into the sequene expression that defines tiles *)
let loadTiles (content : ContentManager) (path:string) =
    let atlas = TextureAtlas.Load(new IO.StreamReader(path))
    let img   = content.Load<Texture2D>(atlas.ImagePath)
    let fw    = float32 atlas.Width
    let fh    = float32 atlas.Height
    let tiles = seq { for child in atlas.SubTextures do
                          let tile = loadTile fw fh img child
                          yield child.Name, tile }
    { tiles = Map.ofSeq tiles; width = fw; height = fh }


let getTile tiles str = Map.find str tiles.tiles

let tryGetTile tiles str = Map.tryFind str tiles.tiles

type grid = {
    tiles: tiles;
    grid: tile option [,];
}

let mkGrid tiles width height =
    let grid = Array2D.create width height None
    { tiles = tiles; grid = grid }

let clearCell grid x y =
    Array2D.set grid.grid x y None

let setCell grid x y tile =
    Array2D.set grid.grid x y (Some tile)

let setCellByName grid x y tile_name =
    Array2D.set grid.grid x y (Some (getTile grid.tiles tile_name))

let getCell grid x y =
    Array2D.get grid.grid x y

(* Draw top-right to bottom-left *)
let renderGrid (sprites:SpriteBatch) grid (grid_x:float32) (grid_y:float32) =

    // cache some grid sizing information
    let w2 = grid.tiles.width  / 2.0f
    let h2 = grid.tiles.height / 2.0f
    let xw = Array2D.length1 grid.grid - 1
    let fxw = float32 xw
    let yw = Array2D.length2 grid.grid - 1

    // figure out where the top of the diamond will be
    let x0 = grid_x + w2 * float32 (Array2D.length1 grid.grid) - w2
    let y0 = grid_y

    // iterate down through the rows, back to front
    for y = Array2D.base2 grid.grid to yw do
        for x = xw downto Array2D.base1 grid.grid do
            getCell grid x y |> Option.iter (fun tile ->
                // adjust x to be relative to the top-right corner of the grid
                let x'       = xw - x
                let screen_x = x0 - float32 (x' - y) * w2
                let screen_y = y0 + float32 (x' + y) * h2
                renderTile sprites tile screen_x screen_y)