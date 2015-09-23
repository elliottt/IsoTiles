﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open IsoTiles.Tiles

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type TestGame() as x =
    inherit Game()

    let manager = new GraphicsDeviceManager(x)
    let mutable sprites = Unchecked.defaultof<SpriteBatch>
    let mutable tiles   = emptyTiles
    let mutable grid    = Unchecked.defaultof<grid>
    let mutable rect    = Unchecked.defaultof<Texture2D>

    override x.Initialize () =
        sprites <- new SpriteBatch(x.GraphicsDevice)
        printfn "initialized!"
        base.Initialize()

    override x.LoadContent () =
        do printfn "loading content!"
           printfn "i'm at %s" (System.IO.Directory.GetCurrentDirectory ())
           printfn "content loaded from %s" x.Content.RootDirectory
           tiles <- loadTiles x.Content "cityTiles_sheet.xml"
           grid  <- mkGrid tiles 2 3 "cityTiles_000.png"

        let tile = getTile tiles "cityTiles_001.png"
        do setCell grid 0 0 tile
           setCell grid 1 0 tile

    override x.Update _ =
        ()

    override x.Draw _ =
        x.GraphicsDevice.Clear(Color.CornflowerBlue)
        sprites.Begin()
        renderGrid sprites grid 10.0f 10.0f
        sprites.End()

let game = new TestGame() in
    game.Run ()