local MouseController = {}

MouseController.name = "AletrisSandbox/MouseController"
MouseController.depth = -8500
MouseController.texture = "objects/AletrisSandbox/MouseController/idle00"

MouseController.fieldInformation = {
    Enabled = { fieldType = "boolean", default = true }
}

MouseController.fieldOrder = { "x", "y", "Enabled" }

MouseController.placements = {
    name = "Mouse Control ler",
    placementType = "point",
    data = {
        Enabled = true
    }
}

function MouseController.texture(room, entity)
    return "objects/AletrisSandbox/MouseController/idle00"
end

return MouseController