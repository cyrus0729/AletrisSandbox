local drawableRectangle = require('structs.drawable_rectangle')
local utils = require("utils")

local BulletBlocker = {}
BulletBlocker.name = "AletrisSandbox/BulletBlocker"
BulletBlocker.depth = -8500

BulletBlocker.fieldInformation = {
    width = { fieldType = "integer", default = 16 },
    height = { fieldType = "integer", default = 16 },
    Invisible = {fieldType = "boolean", default = true },
}

BulletBlocker.fieldOrder = {"x","y","width","height","Invisible"}

BulletBlocker.placements = {
    name = "Bullet Block Field",
    placementType = "rectangle",
    data = {
        width = 8,
        height = 8,
        Invisible = true,
    }
}

function BulletBlocker.sprite(room,entity)
    return drawableRectangle.fromRectangle("fill",entity.x,entity.y,entity.width,entity.height,{r,g,b},{r2,g2,b2})
end

return BulletBlocker