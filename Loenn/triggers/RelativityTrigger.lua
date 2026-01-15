local RelativityTrigger = {}

RelativityTrigger.name = "AletrisSandbox/RelativityTrigger"

RelativityTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Enable = { fieldType = "boolean", default = true },
    DisableOnLeave = { fieldType = "boolean", default = false },
}

RelativityTrigger.fieldOrder = { "x", "y", "width", "height", "Enable", "DisableOnLeave" }

RelativityTrigger.placements = {
    name = "Relativistic Physics Toggle",
    data = {
        width = 8,
        height = 8,
        Enable = true,
        DisableOnleave = false
    }
}

return RelativityTrigger