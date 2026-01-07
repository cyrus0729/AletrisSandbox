local utils = require("utils")

local blackhole = {}

blackhole.name = "AletrisSandbox/ModifiedBlackHole"

blackhole.placements = {
    name = "Modified Black Hole",
    data = {
        SpeedModifier = 1.02,
        ForceModifier = 0.8,
        auraRadius = 48,
        holeRadius = 8,
    }
}

blackhole.texture = "AletrisSandbox/ModifiedBlackHole/LoennPreview"

function blackhole.selection(room, entity)
    return utils.rectangle(entity.x - 15, entity.y - 15, 30, 30)
end

function blackhole.scale(room,entity)
    return entity.auraRadius/48, entity.auraRadius/48
end

return blackhole