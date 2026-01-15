local CustomTimingOshiro = {}

CustomTimingOshiro.name = "AletrisSandbox/CustomTimingOshiro"
CustomTimingOshiro.depth = -12500
CustomTimingOshiro.texture = "characters/oshiro/boss13"

CustomTimingOshiro.fieldInformation = {
    timings = { fieldType = "string", default = "" },
}

CustomTimingOshiro.fieldOrder = { "x", "y", "timings" }

CustomTimingOshiro.placements = {
    name = "CustomTimingOshiro",
    placementType = "point",
    data = {
        timings = "",
    }
}

return CustomTimingOshiro