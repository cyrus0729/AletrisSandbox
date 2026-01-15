local function createPuffer(displayname, rightw)
    local MomentumPufferEntity = {}

    MomentumPufferEntity.name = "AletrisSandbox/MomentumPuffer"

    MomentumPufferEntity.fieldInformation = {
        right = { fieldType = "boolean", default = rightw },
    }

    MomentumPufferEntity.fieldOrder = { "x", "y", "right" }

    MomentumPufferEntity.placements = {
        name = displayname,
        placementType = "point",
        data = {
            right = rightw
        }
    }

    return MomentumPufferEntity
end

local MomentumPufferEntityLeft = createPuffer("Momentum Puffer (Left)", false)
local MomentumPufferEntityRight = createPuffer("Momentum Puffer (Right)", true)

return { MomentumPufferEntityLeft, MomentumPufferEntityRight }