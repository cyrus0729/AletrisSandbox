local MouseController = {}

MouseController.name = "AletrisSandbox/MouseController"
MouseController.depth = -8500
MouseController.texture = "objects/AletrisSandbox/MouseController/idle00"

MouseController.fieldInformation = {
    Forced = { fieldType = "boolean", default = false },
    Visible = { fieldType = "boolean", default = false }
}

MouseController.fieldOrder = { "x", "y", "Forced", "Visible" }

MouseController.placements = {
    name = "Mouse Controls Controller",
    placementType = "point",
    data = {
        Forced = false,
        Visible = true,
    }
}

function MouseController.texture(room, entity)
    return "objects/AletrisSandbox/MouseController/idle00"
end

return MouseController