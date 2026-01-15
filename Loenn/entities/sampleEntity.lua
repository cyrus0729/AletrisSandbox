local sampleEntity = {}

sampleEntity.name = "AletrisSandbox/SampleEntity"
sampleEntity.depth = -8500
sampleEntity.texture = "objects/AletrisSandbox/sampleEntity/idle00"

sampleEntity.fieldInformation = {
    sampleProperty = { fieldType = "integer", default = 0 }
}

sampleEntity.fieldOrder = { "x", "y", "sampleProperty" }

sampleEntity.placements = {
    name = "normal",
    placementType = "point",
    data = {
        sampleProperty = 0
    }
}

function sampleEntity.sprite(room, entity)
    return "objects/AletrisSandbox/sampleEntity/idle00"
end

return sampleEntity