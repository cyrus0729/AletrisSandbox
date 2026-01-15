local ToggleHealthTrigger = {}

ToggleHealthTrigger.name = "AletrisSandbox/ToggleHealthTrigger"

ToggleHealthTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Enable = { fieldType = "boolean", default = true },
    DefaultHealth = { fieldType = "integer", default = 1000 },
}

ToggleHealthTrigger.fieldOrder = { "x", "y", "width", "height", "Enable", "DefaultHealth" }

ToggleHealthTrigger.placements = {
    name = "Enable Health Trigger",
    data = {
        width = 8,
        height = 8,
        Enable = true,
        DefaultHealth = 1000
    }
}

return ToggleHealthTrigger