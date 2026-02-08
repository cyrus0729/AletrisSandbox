local drawableSprite = require('structs.drawable_sprite')
local BulletRedirector = {}

BulletRedirector.name = "AletrisSandbox/BulletRedirector"
BulletRedirector.depth = -8500
BulletRedirector.texture = "objects/AletrisSandbox/BulletRedirector/idle00"

BulletRedirector.fieldInformation = {
    Rotation = { fieldType = "integer", default = 0 },
    Deadly = { fieldType = "boolean", default = false }
}

BulletRedirector.fieldOrder = { "x", "y", "Rotation", "Deadly" }

BulletRedirector.placements = {
    name = "Bullet Redirector",
    placementType = "point",
    data = {
        Rotation = 0,
        Deadly = false,
    }
}

function BulletRedirector.sprite(room, entity)
    local sprite
    sprite = drawableSprite.fromTexture("objects/AletrisSandbox/BulletRedirector/idle00", entity)
    sprite.rotation = math.rad(entity.Rotation)
    return sprite
end

function BulletRedirector.rotate(room, entity, direction)
    entity.Rotation += 90
    entity.Rotation = entity.Rotation % 360
    return true
end

return BulletRedirector