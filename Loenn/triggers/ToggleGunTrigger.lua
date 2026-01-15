local ToggleGunTrigger = {}

ToggleGunTrigger.name = "AletrisSandbox/ToggleGunTrigger"

ToggleGunTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Enable = { fieldType = "boolean", default = true },
    Display = { fieldType = "boolean", default = true },
    Autofire = { fieldType = "boolean", default = false },
    MouseControl = { fieldType = "boolean", default = false },
    DestroyStuff = { fieldType = "boolean", default = false },
    InteractsWithStuff = { fieldType = "boolean", default = true },
    BulletsAllowed = { fieldType = "integer", default = 4 },
}

ToggleGunTrigger.fieldOrder = { "x", "y", "width", "height", "Enable", "Display", "Autofire", "MouseControl", "DestroyStuff", "InteractsWithStuff", "BulletsAllowed"}

ToggleGunTrigger.placements = {
    name = "Toggle Gun Trigger",
    data = {
        width = 8,
        height = 8,
        Enable = true,
        Display = true,
        Autofire = false,
        MouseControl = false,
        DestroyStuff = false,
        InteractsWithStuff = true,
        BulletsAllowed = 4,
    }
}

return ToggleGunTrigger