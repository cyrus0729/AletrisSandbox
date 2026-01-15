local IWBTGJumpTrigger = {}

IWBTGJumpTrigger.name = "AletrisSandbox/IWBTGJumpTrigger"

IWBTGJumpTrigger.fieldInformation = {
    width = { fieldType = "integer", default = 8 },
    height = { fieldType = "integer", default = 8 },
    Enable = { fieldType = "boolean", default = true },
}

IWBTGJumpTrigger.fieldOrder = { "x", "y", "width", "height", "Enable" }

IWBTGJumpTrigger.placements = {
    name = "IWBTG Jump Physics Trigger",
    data = {
        width = 8,
        height = 8,
        Enable = true
    }
}

return IWBTGJumpTrigger