module IsoTiles.Tiles

open System
open FSharp.Data
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics


type tile_name = string

type IRender =
    abstract Render : SpriteBatch -> float32-> float32 -> unit

let render (r : IRender) sprites x y = r.Render sprites x y

type TextureAtlas = XmlProvider<"""
<TextureAtlas imagePath="foo.png" width="100" height="100">
  <SubTexture name="region1" x="0"  y="0"  width="10" height="10" />
  <SubTexture name="region2" x="10" y="10" width="20" height="20" offset-x="2" />
  <SubTexture name="region3" x="10" y="10" width="20" height="20" offset-y="3" />
  <SubTexture name="region4" x="10" y="10" width="20" height="20" offset-x="2" offset-y="3" />
</TextureAtlas>
""">

let option (def:'b) (f:'a -> 'b) (opt:'a option) : 'b =
    match opt with
    | Some a -> f a
    | None   -> def

type Tile(width, height, tex : Texture2D, sub : TextureAtlas.SubTexture) =
    let offsetX = option 0.0f float32 sub.OffsetX
    let offsetY = option 0.0f float32 sub.OffsetY
    let texture = tex
    let sourceRect = Nullable<Rectangle>(Rectangle(int sub.X,int sub.Y,int sub.Width,int sub.Height))

    interface IRender with
        member __.Render sprites x y =
            let real_x = x + offsetX
            let real_y = y + offsetY
            sprites.Draw(tex,Vector2 (real_x,real_y), sourceRect, Color.White)

type Tiles(content : ContentManager, path : string) =
    let atlas = TextureAtlas.Load(new IO.StreamReader(path))
    let img   = content.Load<Texture2D>(atlas.ImagePath)
    let fw    = float32 atlas.Width
    let fh    = float32 atlas.Height
    let tiles = seq { for child in atlas.SubTextures do
                          let tile = new Tile(fw,fh,img,child)
                          yield child.Name, tile } |> Map.ofSeq

    member __.Width  with get () = fw
    member __.Height with get () = fh

    member __.GetTile str = Map.find str tiles
    member __.TryGetTile str = Map.tryFind str tiles

type IGrid =
    abstract SetCell   : int -> int -> Tile -> unit
    abstract GetCell   : int -> int -> Tile option
    abstract ClearCell : int -> int -> unit

let set_cell   (g : IGrid) x y t = g.SetCell   x y t
let get_cell   (g : IGrid) x y   = g.GetCell   x y
let clear_cell (g : IGrid) x y   = g.ClearCell x y

type IsoGrid(tiles : Tiles, width, height) =
    let grid = Array2D.create width height (None : Tile option)

    interface IGrid with
        member __.SetCell   x y tile = Array2D.set grid x y (Some tile)
        member __.GetCell   x y      = Array2D.get grid x y
        member __.ClearCell x y      = Array2D.set grid x y None

    interface IRender with
        member __.Render sprites grid_x grid_y =

            // cache some grid sizing information
            let w2 = tiles.Width  / 2.0f
            let h2 = tiles.Height / 2.0f
            let xw = Array2D.length1 grid - 1
            let fxw = float32 xw
            let yw = Array2D.length2 grid - 1

            // figure out where the top of the diamond will be
            let x0 = grid_x + w2 * float32 (Array2D.length1 grid) - w2
            let y0 = grid_y

            // iterate down through the rows, back to front
            for y = Array2D.base2 grid to yw do
                for x = xw downto Array2D.base1 grid do
                    Array2D.get grid x y |> Option.iter (fun tile ->
                        // adjust x to be relative to the top-right corner of the grid
                        let x'       = xw - x
                        let screen_x = x0 - float32 (x' - y) * w2
                        let screen_y = y0 + float32 (x' + y) * h2
                        render tile sprites screen_x screen_y)