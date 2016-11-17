module Vector2Module
    open Microsoft.Xna.Framework

    let distance (v1: Vector2) (v2: Vector2) =
        sqrt((v1.X - v2.X)**2.f + (v1.Y - v2.Y)**2.f)
