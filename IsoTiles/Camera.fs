module IsoTiles.Camera

open Microsoft.Xna.Framework

type Camera() =
    let mutable position  = Vector2.Zero
    let mutable offset    = Vector2.Zero
    let mutable screen_pos= Vector2.Zero
    let mutable zoom      = Vector2.One
    let mutable rotation  = 0.0f

    member __.X
        with get () = position.X
        and  set x  = position.X <- x

    member __.Y
        with get () = position.Y
        and  set y  = position.Y <- y

    member __.Rot
        with get () = rotation
        and  set r  = rotation <- r

    member __.Zoom
        with set z =
                do zoom.X <- z
                   zoom.Y <- z

    (**
     * Given a camera, produce a transformation matrix suitable for use with
     * SpriteBatch.Begin()
     *)
    member __.TransformMatrix
        with get () =
            let rot_origin = Vector3(position + offset, 0.0f)
            let screen_pos = Vector3(screen_pos, 0.0f)
            System.Nullable<Matrix>(Matrix.CreateTranslation(- rot_origin)
                * Matrix.CreateScale(zoom.X, zoom.Y, 1.0f)
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateTranslation(screen_pos))