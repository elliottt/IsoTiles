
module IsoTiles.Camera

open Microsoft.Xna.Framework
open System

type Camera =
    new : unit -> Camera
    member X : float32 with get, set
    member Y : float32 with get, set
    member Zoom : float32 with set
    member TransformMatrix : Nullable<Matrix> with get