local EntityHPController = {}
EntityHPController.name = "AletrisSandbox/EntityHPController"
EntityHPController.depth = -8500
EntityHPController.texture = "objects/AletrisSandbox/EntityHPController/idle00"

EntityHPController.fieldInformation = {
    HP = { fieldType = "integer", default = 0 },
    DamageIncrement = { fieldType = "integer", default = 0 },
    Boss = { fieldType = "boolean", default = false },
    flagOnKillAll = { fieldType = "string", default = "" },
    OnlyType = { fieldType = "string", default = "" },
    DeathSound = { fieldType = "string", default = "event:/aletris_sandbox/die" },
    HitSound = { fieldType = "string", default = "event:/aletris_sandbox/hit" },
    IFrames = { fieldType = "integer", default = 0 }
}

EntityHPController.fieldOrder = {"x","y","HP","DamageIncrement","flagOnKillAll","OnlyType","DeathSound","HitSound","IFrames","Boss"}

EntityHPController.placements = { 
    name = "Entity HP Controller",
    placementType = "point",
    data = {
        HP = 0,
        DamageIncrement = 0,
        Boss = false,
        flagOnkillAll = "",
        OnlyType = "",
        DeathSound = "",
        HitSound = "",
        IFrames = ""
    }
}

return EntityHPController