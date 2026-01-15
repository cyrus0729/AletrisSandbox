local HitboxTrigger = {}

HitboxTrigger.name = "AletrisSandbox/HitboxTrigger"

HitboxTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Hitbox = { fieldType = "string", default = "R:8,11,-4,-11" },
    duckHitbox = { fieldType = "string", default = "R:8,6,-4,-6" },
    featherHitbox = { fieldType = "string", default = "R:8,8,-4,-10" },
    Hurtbox = { fieldType = "string", default = "R:8,9,-4,-11" },
    duckHurtbox = { fieldType = "string", default = "R:8,4,-4,-6" },
    featherHurtbox = { fieldType = "string", default = "R:6,6,-3,-9" },
    modifyHitbox = { fieldType = "boolean", default = true },
}

HitboxTrigger.fieldOrder = { "x", "y", "width", "height", "Hitbox", "duckHitbox", "featherHitbox", "Hurtbox", "duckHurtbox", "featherHurtbox", "modifyHitbox" }

HitboxTrigger.placements = {
    name = "Change Hitbox Trigger",
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
return HitboxTrigger