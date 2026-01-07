local HitboxTrigger = {}

HitboxTrigger.name = "AletrisSandbox/HitboxTrigger"

HitboxTrigger.placements = {
    {
        name = "Change Hitbox Trigger",
        data = {
            width = 8,
            height = 8,
            modifyHitbox = true,
            Hitbox = "R:8,11,-4,-11",
            duckHitbox = "R:8,6,-4,-6",
            featherHitbox = "R:8,8,-4,-10",
            Hurtbox = "R:8,9,-4,-11",
            duckHurtbox = "R:8,4,-4,-6",
            featherHurtbox = "R:6,6,-3,-9",
        },
        fieldOrder = {"x","y","width","height","Hitbox","duckHitbox","featherHitbox","Hurtbox","duckHurtbox","featherHurtbox"}
    }
}

return HitboxTrigger