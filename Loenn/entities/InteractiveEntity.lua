local InteractiveEntity = {}

InteractiveEntity.name = "AletrisSandbox/InteractiveEntity"

InteractiveEntity.depth = -8500

InteractiveEntity.fieldInformation = {
    TalkSound = { fieldType = "string", default = "" },
    TalkEndSound = { fieldType = "string", default = "" },
    TalkDuration = { fieldType = "integer", default = 2 },
    MakeHairInvisible = { fieldType = "boolean", default = false },
    Spacing = { fieldType = "integer", default = 0 },
    ShowIndicator = { fieldType = "boolean", default = true },
    playerAnimation = { fieldType = "string", default = "plrPetting" },
    talkAnimation = { fieldType = "string", default = "sunnyPetting" },
    idleAnimation = { fieldType = "string", default = "sunnyIdle" },
    flag = { fieldType = "string", default = "" },
}

InteractiveEntity.fieldOrder = {
    "x", "y", "TalkSound", "TalkEndSound", "TalkDuration", "MakeHairInvisible", "Spacing",
    "playerAnimation", "talkAnimation", "idleAnimation", "flag", "ShowIndicator"
}

InteractiveEntity.placements = {
    name = "Interactive Entity",
    placementType = "point",
    data = {
        TalkSound =  "",
        TalkEndSound = "",
        TalkDuration = 2,
        MakeHairInvisible = false,
        Spacing =  0,
        ShowIndicator = true,
        playerAnimation = "plrPetting",
        talkAnimation = "sunnyPetting",
        idleAnimation = "sunnyIdle",
        flag = ""
    }
}

function InteractiveEntity.texture(room, entity)
    local sprite = "characters/AletrisSandbox/InteractiveEntity/sunny/idle00"
    return sprite
end

return InteractiveEntity