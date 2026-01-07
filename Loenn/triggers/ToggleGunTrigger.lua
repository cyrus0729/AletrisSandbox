local ToggleGunTrigger = {
    name = "AletrisSandbox/ToggleGunTrigger",
    placements = {
        {
            name = "Toggle Gun Trigger",
            data = {
                width = 8,
                height = 8,
                Enable = true,
                Autofire = false,
                MouseControl = false,
                DestroyStuff = false,
                InteractsWithStuff = false,
                BulletsAllowed = 4,
            },
            fieldOrder = {"x","y","width","height","Enable","Autofire","MouseControl","DestroyStuff","InteractsWithStuff","BulletsAllowed"}
        },
    },
}

return ToggleGunTrigger