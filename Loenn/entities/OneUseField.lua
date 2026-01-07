local drawableRectangle = require('structs.drawable_rectangle')
local utils = require("utils")

local OneUseField = {
    name = "AletrisSandbox/OneUseField",
    depth = -8500,
    placements = {
        {
            name = "One Use Field",
            data = {
                kill = false,
                depth = -8500,
                InactiveColor = "00ff00",
                InactiveBorderColor = "008800",
                ActivatingColor =  "ffff00",
                ActivatingBorderColor = "888800",
                ActiveColor = "ff0000",
                ActiveBorderColor = "880000",
                width = 8,
                height = 8
            },
            fieldOrder = {"x","y","kill","depth","InactiveColor","InactiveBorderColor","ActivatingColor","ActivatingBorderColor","ActiveColor","ActiveBorderColor","width","height"}
        },
    },
}

OneUseField.fieldInformation = {
    interactType = {
        options = {"Kill, Block"},
        editable = false
    },
    InactiveColor = {fieldType = "color",allowXNAColors = true},
    InactiveBorderColor = {fieldType = "color",allowXNAColors = true},
    ActivatingColor = {fieldType = "color",allowXNAColors = true},
    ActivatingBorderColor = {fieldType = "color",allowXNAColors = true},
    ActiveColor = {fieldType = "color",allowXNAColors = true},
    ActiveBorderColor = {fieldType = "color",allowXNAColors = true},
}

function OneUseField.sprite(room,entity)
    local success,r,g,b = utils.parseHexColor(entity.InactiveColor)
    local success,r2,g2,b2 = utils.parseHexColor(entity.InactiveBorderColor)
    return drawableRectangle.fromRectangle("bordered",entity.x,entity.y,entity.width,entity.height,{r,g,b},{r2,g2,b2})
end

return OneUseField