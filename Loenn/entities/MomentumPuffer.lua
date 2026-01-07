local function createPuffer(displayname,rightw)
    local MomentumPufferEntity = {}
    
    MomentumPufferEntity.name = "AletrisSandbox/MomentumPuffer"
    MomentumPufferEntity.placements = {
        {
            name = displayname,
            data = {
                right = rightw
            }
        }
    }
    return MomentumPufferEntity
end

local MomentumPufferEntityLeft = createPuffer("Momentum Puffer (Left)",false)
local MomentumPufferEntityRight = createPuffer("Momentum Puffer (Right)",true)

return {MomentumPufferEntityLeft, MomentumPufferEntityRight}