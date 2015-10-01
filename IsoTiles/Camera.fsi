
module IsoTiles.Camera

open Microsoft.Xna.Framework
open System

type camera

val make_camera      : unit   -> camera
val transform_matrix : camera -> Nullable<Matrix>

val set_x   : float32 -> camera -> camera
val set_y   : float32 -> camera -> camera
val set_rot : float32 -> camera -> camera

val set_zoom: float32 -> camera -> camera