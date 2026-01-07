local drawableSprite = require('structs.drawable_sprite')
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

local function createDeathCube(truename,displayname,rotation,mini)
    local DeathCube = {}
    
    DeathCube.name = truename

    DeathCube.placements = {
        {
            name = displayname,
            data = {
                Sprite = "DeathCubeEntity",
                UnforgivingHitbox = false,
            }
        }
    }

    DeathCube.sprite = function(room,entity)
        local sprite 
        sprite = drawableSprite.fromTexture("danger/AletrisSandbox/DeathCubeEntity/idle00", entity)
        sprite.rotation = math.rad(90*(rotation-1))
        return sprite
    end

    function DeathCube.rotate(room, entity, direction)
        local newFacing = utils.mod1(rotation + direction,4)
        entity._name = "AletrisSandbox/".."DeathCube"..sideIndexLookup[newFacing]
        return true
    end

    return DeathCube
end

local DeathCubeUp = createDeathCube("AletrisSandbox/DeathCubeUp","Death Cube (Up)",1,false)
local DeathCubeDown = createDeathCube("AletrisSandbox/DeathCubeDown","Death Cube (Down)",3,false)
local DeathCubeRight = createDeathCube("AletrisSandbox/DeathCubeLeft","Death Cube (Left)",4,false)
local DeathCubeLeft =  createDeathCube("AletrisSandbox/DeathCubeRight","Death Cube (Right)",2,false)

return {DeathCubeUp, DeathCubeDown, DeathCubeRight, DeathCubeLeft}