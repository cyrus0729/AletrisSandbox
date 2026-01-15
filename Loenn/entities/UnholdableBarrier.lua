local drawableRectangle = require('structs.drawable_rectangle')
local utils = require("utils")

local UnholdableBarrier = {}

UnholdableBarrier.name = "AletrisSandbox/UnholdableBarrier"

UnholdableBarrier.depth = -8500

UnholdableBarrier.fieldInformation = {
    color = { fieldType = "color", allowXNAColors = true, default = "#a4911e"},
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
}

UnholdableBarrier.fieldOrder = { "x", "y", "width", "height", "color" }

UnholdableBarrier.placements = {
    name = "Unholdable Barrier",
    placementType = "rectangle",
    data = {
        color =  "#a4911e",
        width = 8,
        height = 8
    }
}

function UnholdableBarrier.sprite(room, entity)
    local success, r, g, b = utils.parseHexColor(entity.color)
    local C1 = { r - 0.2, g - 0.2, b - 0.2 }
    local C2 = { r, g, b }
    return drawableRectangle.fromRectangle("bordered", entity.x, entity.y, entity.width, entity.height, C1, C2)
end

return UnholdableBarrier