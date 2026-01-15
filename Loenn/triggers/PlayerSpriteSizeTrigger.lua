local PlayerSpriteSizeTrigger = {}

PlayerSpriteSizeTrigger.name = "AletrisSandbox/PlayerSpriteSizeTrigger"

PlayerSpriteSizeTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Scale = { fieldType = "string", default = "1,1" },
    Flag = { fieldType = "string", default = "" },
    Persistent = { fieldType = "boolean", default = false },
}

PlayerSpriteSizeTrigger.fieldOrder = { "x", "y", "width", "height", "Scale", "Flag", "Persistent" }

PlayerSpriteSizeTrigger.placements = {
    name = "Player Sprite Size Trigger",
    data =  {
        width = 8,
        height = 8,
        Scale = "1,1",
        Flag = "",
        Persistent = false
    }
}

return PlayerSpriteSizeTrigger