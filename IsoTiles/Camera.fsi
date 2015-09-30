
module IsoTiles.Camera

open Microsoft.Xna.Framework
open System

type camera

val make_camera      : unit   -> camera
val transform_matrix : camera -> Nullable<Matrix>