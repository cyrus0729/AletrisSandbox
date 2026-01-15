local sampleTrigger = {}

sampleTrigger.name = "AletrisSandbox/SampleTrigger"

sampleTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    sampleProperty = { fieldType = "integer", default = 0 },
}

sampleTrigger.fieldOrder = { "x", "y", "width", "height", "sampleProperty" }

sampleTrigger.placements = {
    name = "normal",
    data = {
        sampleProperty = 0
    },
}
return sampleTrigger