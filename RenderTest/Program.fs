// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open IsoTiles.Tiles
open IsoTiles.Camera

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type TestGame() as x =
    inherit Game()

    let manager = new GraphicsDeviceManager(x)
    let mutable sprites = Unchecked.defaultof<SpriteBatch>
    let mutable tiles   = Unchecked.defaultof<Tiles>
    let mutable grid    = Unchecked.defaultof<IsoGrid>
    let mutable rect    = Unchecked.defaultof<Texture2D>
    let camera          = new Camera()

    override x.Initialize () =
        sprites <- new SpriteBatch(x.GraphicsDevice)
        base.Initialize()

    override x.LoadContent () =
        do tiles <- new Tiles(x.Content,"cityTiles_sheet.xml")
           grid  <- new IsoGrid(tiles,2,3)

        let tile = tiles.GetTile "cityTiles_001.png"
        do  set_cell grid 0 0 tile
            set_cell grid 1 0 tile

    override x.Update time =
        let amount = float32 time.TotalGameTime.Milliseconds / 1000.0f
        do  camera.X <- (MathHelper.Lerp(0.0f, -200.0f, amount))

    override x.Draw _ =
        x.GraphicsDevice.Clear(Color.CornflowerBlue)
        sprites.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise,
                null, camera.TransformMatrix)
        render grid sprites 10.0f 10.0f
        sprites.End()

let game = new TestGame() in
    game.Run ()