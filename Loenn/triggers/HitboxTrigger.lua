local HitboxTrigger = {}

HitboxTrigger.name = "AletrisSandbox/HitboxTrigger"

HitboxTrigger.placements = {
    {
        name = "Change Hitbox Trigger",
        data = {
            width = 8,
            height = 8,
            advancedMode = false,
            persistent = false,
            Hitbox = "8,11",
            duckHitbox = "8,6",
            featherHitbox = "8,8",
            Hurtbox = "8,9",
            duckHurtbox = "8,4",
            featherHurtbox = "6,6",

            HitboxOffset = "-4,-11",
            duckHitboxOffset = "-4,-6",
            featherHitboxOffset = "-4,-10",
            HurtboxOffset = "-4,-11",
            duckHurtboxOffset = "-4,-6",
            featherHurtboxOffset = "-3,-9",
        }
    }
}

return HitboxTrigger