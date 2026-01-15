local CircleTrigger = {}

CircleTrigger.name = "AletrisSandbox/CircleTrigger"

CircleTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    circleRadius = { fieldType = "integer", default = 6 },
    drawCircle = { fieldType = "boolean", default = true },
    disableOnLeave = { fieldType = "boolean", default = false },
}

CircleTrigger.fieldOrder = { "x", "y", "width", "height", "circleRadius", "drawCircle", "disableOnLeave" }

CircleTrigger.placements = {
    name = "Circle Madeline Trigger",
    data = {
        width = 8,
        height = 8,
        circleRadius = 6,
        drawCircle = true,
        disableOnLeave = false
    }
}

return CircleTrigger