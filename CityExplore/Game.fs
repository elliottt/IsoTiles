module CityExplore.Game

open IsoTiles.Tiles

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type CityExploreGame() as x =
    inherit Game()

    let manager = new GraphicsDeviceManager(x)
    let mutable sprites = Unchecked.defaultof<SpriteBatch>
    let mutable tiles   = Unchecked.defaultof<Tiles>

    override x.Initialize() =
        sprites <- new SpriteBatch(x.GraphicsDevice)
        base.Initialize()

    override x.LoadContent() =
        do tiles <- Tiles(x.Content, @"cityTiles_sheetl.xml")

    override x.Update _ =
        ()

    override x.Draw _ =
        x.GraphicsDevice.Clear(Color.CornflowerBlue)