local drawableSprite = require('structs.drawable_sprite')

local HitboxController = {}

HitboxController.name = "AletrisSandbox/HitboxController"

HitboxController.fieldInformation = {
    modifyHitbox = { fieldType = "boolean", default = true },
    Hitbox = { fieldType = "string", default = "R:8,11,-4,-11" },
    duckHitbox = { fieldType = "string", default =  "R:8,6,-4,-6" },
    featherHitbox = { fieldType = "string", default = "R:8,8,-4,-10" },
    Hurtbox = { fieldType = "string", default = "R:8,9,-4,-11" },
    duckHurtbox = { fieldType = "string", default = "R:8,4,-4,-6" },
    featherHurtbox = { fieldType = "string", default = "R:6,6,-3,-9" },
}

HitboxController.fieldOrder = {"x","y","Hitbox","duckHitbox","featherHitbox","Hurtbox","duckHurtbox","featherHurtbox","modifyHitbox"}

HitboxController.placements = {
    name = "Hitbox Controller",
    placementType = "point",
    data = {
        modifyHitbox = true ,
        Hitbox = "R:8,11,-4,-11" ,
        duckHitbox = "R:8,6,-4,-6",
        featherHitbox = "R:8,8,-4,-10" ,
        Hurtbox = "R:8,9,-4,-11" ,
        duckHurtbox = "R:8,4,-4,-6" ,
        featherHurtbox ="R:6,6,-3,-9",
    }
}

function HitboxController.texture(room, entity)
    if entity.modifyHitbox then
        return "objects/AletrisSandbox/HitboxController/catplushthebubble"
    else
        return "objects/AletrisSandbox/HitboxController/catplushthenuhble"
    end
end

return HitboxController