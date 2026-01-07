local ToggleHealthTrigger = {
    name = "AletrisSandbox/ToggleHealthTrigger",
    placements = {
        {
            name = "Enable Health Trigger",
            data = {
                width = 8,
                height = 8,
                Enable = true,
                DefaultHealth = 1000,
            },
            fieldOrder = {"x","y","width","height","enable","DefaultHealth"}
        },
    },
}

return ToggleHealthTrigger