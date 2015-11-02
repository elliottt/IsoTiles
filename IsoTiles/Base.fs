
/// Base functionality for the engine.
module IsoTiles.Base

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// Things that can be rendered using a SpriteBatch.
[<Interface>]
type IRender =
    abstract Render : SpriteBatch -> float32 -> float32 -> unit

/// Utility function for invoking the Render method of a class that implements IRender.
let Render (thing : IRender) (sb : SpriteBatch) x y = thing.Render sb x y

 
 /// A world position, assuming that the origin is in the top-left, and extends
 /// positively to the right and down.
[<StructuralEquality;StructuralComparison>]
type Position = { X: int; Y: int; }

let MoveUp    pos = { pos with Y = pos.Y - 1 }
let MoveDown  pos = { pos with Y = pos.Y + 1 }
let MoveLeft  pos = { pos with X = pos.X - 1 }
let MoveRight pos = { pos with X = pos.X + 1 }

let Neighborhood pos =
    seq {
      for x in pos.X - 1 .. pos.X + 1 do
        for y in pos.Y - 1 .. pos.Y + 1 do
          let pos' = { X=x; Y=y }
          if pos' <> pos then yield pos'
    } |> Seq.toList

let InNeighborhood pos =
    let ns = Neighborhood pos
    in fun pos' -> ns |> List.exists (fun p -> p = pos')

/// Treating the first point as a basis, reflect the point through.
let OppositeAbout center pos =
    { X = center.X - (pos.X - center.X);
      Y = center.Y - (pos.Y - center.Y); }

/// Things that have a world position
[<Interface>]
type IPosition =
    abstract Position : Position with get, set

let ModifyPosition (f: Position -> Position) (thing: 'a :> IPosition) =
    thing.Position <- f thing.Position;
    thing

let Position (thing: IPosition) = thing.Position

/// Utility function to clamp a value between two extremes.
/// NOTE: specialized to int, as I'm not sure about the runtime performance of
/// comparable.
let clamp lo hi (x : int) = min hi (max lo x)

// Things that have HP
[<Interface>]
type IHealth =
    abstract MaxHealth : int with get, set
    abstract Health    : int with get, set

/// Apply a function to an IHealth.
let ModifyHealth (f : int -> int) (thing : IHealth) =
    thing.Health <- clamp 0 thing.MaxHealth (f thing.Health)

/// Check to see if Health is positive.
let IsAlive (thing : IHealth) = thing.Health > 0

/// Check to see if Health has dropped to zero.
let IsDead (thing : IHealth) = thing.Health <= 0

/// Add some damage to something that keeps track of HP.
let AddDamage d (thing : IHealth) = ModifyHealth (fun h -> h - d) thing