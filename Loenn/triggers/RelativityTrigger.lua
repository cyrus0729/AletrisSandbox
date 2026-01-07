local RelativityTrigger = {
    name = "AletrisSandbox/RelativityTrigger",
    placements = {
        {
            name = "Relativistic Physics Toggle",
            data = {
                width = 8,
                height = 8,
                Enable = true,
                DisableOnLeave = false,
            },
            fieldOrder = {"x","y","width","height","Enable","DisableOnLeave"}
        },
    },
}

return RelativityTrigger