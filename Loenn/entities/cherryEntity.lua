local drawableSprite = require('structs.drawable_sprite')
local utils = require("utils")

local CherryEntity = {}

CherryEntity.name = "AletrisSandbox/CherryEntity"
CherryEntity.fieldInformation = {
    unforgivingHitbox = {
        fieldType = "boolean"
    },
    animatedHitbox = {
        fieldType = "boolean"
    },
    bigHitbox = {
        fieldType = "boolean"
    },
    color = {
        fieldType = "color",
        allowXNAColors = true
    },
    animationRate = {
        fieldType = "integer"
    }
}

CherryEntity.texture = function(room, entity) return "danger/AletrisSandbox/CherryEntity/"..(entity.bigHitbox and "bigidle00" or "idle00") end

CherryEntity.placements = {
    {
        name = "Delicious Fruit",
        data = {
            animatedHitbox = false,
            unforgivingHitbox = false,
            bigHitbox = false,
            color = "ff0000",
            animationRate = 30
        }
    }
}

return CherryEntity