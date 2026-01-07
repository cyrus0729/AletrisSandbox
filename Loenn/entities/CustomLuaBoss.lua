local enums = require("consts.celeste_enums")

local CustomLuaBoss = {}
CustomLuaBoss.name = "AletrisSandbox/ModifiedLuaBadelineBoss"
CustomLuaBoss.depth = 0
CustomLuaBoss.nodeLineRenderType = "line"
CustomLuaBoss.nodeLimits = {0, -1}
CustomLuaBoss.texture = "characters/badelineBoss/charge00"
CustomLuaBoss.placements = {
    name = "Modified FrostHelper Lua Badeline Boss",
    data = {
        filename = "Assets/FrostHelper/LuaBoss/example",
        bossSprite = "badeline_boss",
        bulletTexture = "boss_bullet",
        lockCamera = false,
        cameraLockY = false,
        startHit = false,
        canChangeMusic = false,
        defaultWaveStrength = 0,
        IdleAudioVolume = 0,
        BossWaveFrequency = 0,
        ChangedMusicVolume = 0,
        ShotChargeVolume = 0,
        BeamChargeVolume = 0,
        ShotShootVolume = 0,
        BeamShootVolume = 0,
        ShatterVolume = 0,
        FacePlayer = true,
        FaceDirection = -1,
    }
}

return CustomLuaBoss
