local drawableSprite = require('structs.drawable_sprite')
local enums = require("consts.celeste_enums")
local utils = require("utils")
local math = require("math")

local sideIndexLookup = {
    "Up",
    "Right",
    "Down",
    "Left",

    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4
}

local function createNeedle(truename,displayname,rotation,mini)
    local NeedleEntity = {}
    
    NeedleEntity.name = truename
    NeedleEntity.fieldInformation = {
        hitbox = { options = {"NeedleHelper", "Forgiving", "Unforgiving"}, editable = false, default = "NeedleHelper" },
        isMini = { fieldType = "boolean", default = false },
        Sprite = { fieldType = "string", default = (mini and "MiniNeedleEntity" or "NeedleEntity") },
        kill = { fieldType = "boolean", default = true }
    }

    NeedleEntity.fieldOrder = {"x","y","Sprite","hitbox","isMini","kill"}

    NeedleEntity.offset = function(room, entity) return entity.isMini and {0.5, 0.5} or {0, 0} end

    NeedleEntity.placements = {
        {
            name = displayname,
            placementType = "point",
            data = {
                hitbox = "NeedleHelper",
                isMini = false,
                Sprite = (mini and "MiniNeedleEntity" or "NeedleEntity"),
                kill = true
            }
        }
    }

    NeedleEntity.justification = (mini and {-0.5,-0.5} or {0,0})

    NeedleEntity.sprite = function(room,entity)
        local sprite 
        local spritepath = (entity.isMini and "/mini00" or "/needle00")
        sprite = drawableSprite.fromTexture("danger/AletrisSandbox/".."NeedleEntity"..spritepath, entity)
        sprite.rotation = math.rad(90*(rotation-1))
        return sprite
    end

    function NeedleEntity.rotate(room, entity, direction)
        local newFacing = utils.mod1(rotation + direction,4)
        entity._name = "AletrisSandbox/"..(entity.isMini and "MiniNeedle" or "Needle")..sideIndexLookup[newFacing]
        return true
    end


    return NeedleEntity
end

local NeedleEntityUp = createNeedle("AletrisSandbox/NeedleUp","Needle (Up)",1,false)
local NeedleEntityDown = createNeedle("AletrisSandbox/NeedleDown","Needle (Down)",3,false)
local NeedleEntityRight = createNeedle("AletrisSandbox/NeedleLeft","Needle (Left)",4,false)
local NeedleEntityLeft =  createNeedle("AletrisSandbox/NeedleRight","Needle (Right)",2,false)

local miniNeedleEntityUp = createNeedle("AletrisSandbox/MiniNeedleUp","Mini Needle (Up)",1,true)
local miniNeedleEntityDown = createNeedle("AletrisSandbox/MiniNeedleDown","Mini Needle (Down)",3,true)
local miniNeedleEntityRight = createNeedle("AletrisSandbox/MiniNeedleLeft","Mini Needle (Left)",4,true)
local miniNeedleEntityLeft =  createNeedle("AletrisSandbox/MiniNeedleRight","Mini Needle (Right)",2,true)


return {
    NeedleEntityUp,
    NeedleEntityDown,
    NeedleEntityLeft,
    NeedleEntityRight,
    miniNeedleEntityUp,
    miniNeedleEntityDown,
    miniNeedleEntityRight,
    miniNeedleEntityLeft}