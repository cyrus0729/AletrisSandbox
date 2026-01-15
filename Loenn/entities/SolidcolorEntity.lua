local utils = require("utils")
local drawable = require("structs.drawable_rectangle")

local SolidcolorEntity = {}

SolidcolorEntity.name = "AletrisSandbox/SolidcolorEntity"
SolidcolorEntity.minimumSize = {8,8}
SolidcolorEntity.Depth = -8500
SolidcolorEntity.fieldInformation = {
    width = { fieldType = "integer", default = 16 },
    height = { fieldType = "integer", default = 16 },
    Color = { fieldType = "color",allowXNAColors = true, default = "#FFFFFF" },
    LineColor = { fieldType = "color",allowXNAColors = true, default = "#000000" },
    Interaction = { options = {"None", "Deadly", "Solid"} , editable = false, default = "Solid" },
    DrawType = { options = {"Line","Fill","Bordered"} , editable = false, default = "Bordered" }
}

SolidcolorEntity.fieldOrder = {"x","y","width","height","Color","LineColor","DrawType","Interaction","Depth"}

SolidcolorEntity.placements = {
    name = "Solid Color Entity",
    placementType = "rectangle",
    data = {
        width = 16,
        height = 16,
        Color = "#FFFFFF",
        LineColor = "#000000",
        Interaction = "Solid",
        DrawType = "Bordered"
    }
 }

function SolidcolorEntity.sprite(room, entity)
    local x, y = entity.x or 0, entity.y or 0
    local width, height = entity.width or 8, entity.height or 8
    local color = entity.Color --custom
    local bordercolor = entity.LineColor
    local rect = drawable.fromRectangle(string.lower(entity.DrawType), x, y, width, height, color, bordercolor) --"line","fill","bordered"
    return rect
end

return SolidcolorEntity