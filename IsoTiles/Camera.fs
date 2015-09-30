module IsoTiles.Camera

open Microsoft.Xna.Framework

type camera = {
    mutable position  : Vector2;
    mutable offset    : Vector2;
    mutable screen_pos: Vector2;
    mutable zoom      : Vector2;
    mutable rotation  : float32;
}

let make_camera () =
    { position   = Vector2.Zero;
      offset     = Vector2.Zero;
      screen_pos = Vector2.Zero;
      zoom       = Vector2.One;
      rotation   = 0.0f; }

(**
 * Given a camera, produce a transformation matrix suitable for use with
 * SpriteBatch.Begin()
 *)
let transform_matrix camera =
    let rot_origin = Vector3(camera.position + camera.offset, 0.0f)
    let screen_pos = Vector3(camera.screen_pos, 0.0f)
    System.Nullable<Matrix>(Matrix.CreateTranslation(- rot_origin)
        * Matrix.CreateScale(camera.zoom.X, camera.zoom.Y, 1.0f)
        * Matrix.CreateRotationZ(camera.rotation)
        * Matrix.CreateTranslation(screen_pos))