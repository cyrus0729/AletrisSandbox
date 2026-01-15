local enums = require("consts.celeste_enums")

local CustomLuaBoss = {}

CustomLuaBoss.name = "AletrisSandbox/ModifiedLuaBadelineBoss"

CustomLuaBoss.depth = 0
CustomLuaBoss.nodeLineRenderType = "line"
CustomLuaBoss.nodeLimits = {0, -1}

CustomLuaBoss.texture = "characters/badelineBoss/charge00"

CustomLuaBoss.fieldInformation = {
    filename = { fieldType = "string", default = "Assets/FrostHelper/LuaBoss/example" },
    bossSprite = { fieldType = "string", default = "badeline_boss" },
    bulletTexture = { fieldType = "string", default = "boss_bullet" },
    lockCamera = { fieldType = "boolean", default = false },
    cameraLockY = { fieldType = "boolean", default = false },
    startHit = { fieldType = "boolean", default = false },
    canChangeMusic = { fieldType = "boolean", default = false },
    defaultWaveStrength = { fieldType = "integer", default = 0 },
    IdleAudioVolume = { fieldType = "number", default = 0 },
    BossWaveFrequency = { fieldType = "number", default = 0 },
    ChangedMusicVolume = { fieldType = "number", default = 0 },
    ShotChargeVolume = { fieldType = "number", default = 0 },
    BeamChargeVolume = { fieldType = "number", default = 0 },
    ShotShootVolume = { fieldType = "number", default = 0 },
    BeamShootVolume = { fieldType = "number", default = 0 },
    ShatterVolume = { fieldType = "number", default = 0 },
    FacePlayer = { fieldType = "boolean", default = true },
    FaceDirection = { fieldType = "integer", default = -1 },
}

CustomLuaBoss.fieldOrder = {
    "filename", "bossSprite", "bulletTexture", "lockCamera", "cameraLockY",
    "startHit", "canChangeMusic", "defaultWaveStrength", "IdleAudioVolume",
    "BossWaveFrequency", "ChangedMusicVolume", "ShotChargeVolume",
    "BeamChargeVolume", "ShotShootVolume", "BeamShootVolume", "ShatterVolume",
    "FacePlayer", "FaceDirection"
}

CustomLuaBoss.placements = {
    {
        name = "Modified FrostHelper Lua Badeline Boss",
        placementType = "point",
        data = {
            filename = "Assets/FrostHelper/LuaBoss/example",
            bossSprite = "badeline_boss" ,
            bulletTexture = "boss_bullet" ,
            lockCamera = false ,
            cameraLockY =  false,
            startHit = false,
            canChangeMusic = false,
            defaultWaveStrength = 0,
            IdleAudioVolume = 0,
            BossWaveFrequency =  0,
            ChangedMusicVolume = 0,
            ShotChargeVolume = 0,
            BeamChargeVolume = 0,
            ShotShootVolume = 0,
            BeamShootVolume = 0,
            ShatterVolume = 0,
            FacePlayer = true,
            FaceDirection = -1 
        }
    }
}

return CustomLuaBoss