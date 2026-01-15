local drawableRectangle = require('structs.drawable_rectangle')
local utils = require("utils")

local OneUseField = {}
OneUseField.name = "AletrisSandbox/OneUseField"
OneUseField.depth = -8500

OneUseField.fieldInformation = {
    width = { fieldType = "integer", default = 16 },
    height = { fieldType = "integer", default = 16 },
    depth = {fieldType = "integer", default = -8500 },
    interactType = { options = {"Kill, Block"}, editable = false, default = "Block" },
    InactiveColor = {fieldType = "color",allowXNAColors = true, default = "#00FF00" },
    InactiveBorderColor = {fieldType = "color",allowXNAColors = true, default = "#00CC00" },
    ActivatingColor = {fieldType = "color",allowXNAColors = true, default = "#FFFF00" },
    ActivatingBorderColor = {fieldType = "color",allowXNAColors = true, default = "#CCCC00" },
    ActiveColor = {fieldType = "color",allowXNAColors = true, default = "#FF0000" },
    ActiveBorderColor = {fieldType = "color",allowXNAColors = true, default = "#CC0000" },
}

OneUseField.fieldOrder = {"x","y","interactType","depth","InactiveColor","InactiveBorderColor","ActivatingColor","ActivatingBorderColor","ActiveColor","ActiveBorderColor","width","height"}

OneUseField.placements = {
    name = "One Use Field",
    placementType = "rectangle",
    data = {
        width = 8,
        height = 8,
        depth = -8500,
        interactType = "Block",
        InactiveColor = "#00FF00",
        InactiveBorderColor = "#00CC00",
        ActivatingColor = "#FFFF00",
        ActivatingBorderColor = "#CCCC00",
        ActiveColor = "#FF0000",
        ActiveBorderColor = "#CC0000"
    }
}

function OneUseField.sprite(room,entity)
    local success,r,g,b = utils.parseHexColor(entity.InactiveColor)
    local success,r2,g2,b2 = utils.parseHexColor(entity.InactiveBorderColor)
    return drawableRectangle.fromRectangle("bordered",entity.x,entity.y,entity.width,entity.height,{r,g,b},{r2,g2,b2})
end

return OneUseField