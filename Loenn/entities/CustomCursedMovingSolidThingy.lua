local utils = require("utils")
local drawableNinePatch = require("structs.drawable_nine_patch")
local drawableSprite = require("structs.drawable_sprite")
local drawableLine = require("structs.drawable_line")
local logging = require("logging")

local CustomCursedMovingSolidThingy = {}

CustomCursedMovingSolidThingy.name = "AletrisSandbox/CustomCursedMovingSolidThingy"

CustomCursedMovingSolidThingy.depth = -8500

CustomCursedMovingSolidThingy.nodeLineRenderType = "line"
CustomCursedMovingSolidThingy.nodeLimits = { 0, -1 }
CustomCursedMovingSolidThingy.fieldInformation = {
	width = { fieldType = "integer", default = 8 },
	height = { fieldType = "integer", default = 8 },
	XOffset = { fieldType = "integer", default = 0 },
	YOffset = { fieldType = "integer", default = 0 },
	nonCollidable = { fieldType = "boolean", default = false },
	pathColor = { fieldType = "color", allowXNAColors = true, default = "#000000" },
	sprite = { fieldType = "string", default = "objects/AletrisSandbox/CustomCursedMovingSolidThingy/" },
}

CustomCursedMovingSolidThingy.fieldOrder =
	{ "x", "y", "width", "height", "XOffset", "YOffset", "pathColor", "sprite", "nonCollidable" }

CustomCursedMovingSolidThingy.warnBelowSize = { 16, 16 }

CustomCursedMovingSolidThingy.placements = {
	name = "Custom Cursed Moving Solid Thingy",
	placementType = "rectangle",
	data = {
		width = 8,
		height = 8,
		XOffset = 0,
		YOffset = 0,
		nonCollidable = false,
		pathColor = "#000000",
		sprite = "objects/AletrisSandbox/CustomCursedMovingSolidThingy/",
	},
}

local ninePatchOptions = {
	mode = "fill",
	borderMode = "repeat",
	fillMode = "repeat",
}

local function generateSprite(sprites, entity, node)
	local x,y
	if node then
		x,y = node.x or 0, node.y or 0
	else
		x,y = entity.x , entity.y or 0
	end
	local width, height = entity.width or 16, entity.height or 16
	local ninePatched = drawableNinePatch.fromTexture(entity.sprite.."solid", ninePatchOptions, x, y, width, height)

	for _, sprite in ipairs(ninePatched:getDrawableSprite()) do
		table.insert(sprites, sprite)
	end

	return sprites
end

local function generateLine(sprites, entity, p1, p2)
	local hw,hh = math.floor(entity.width/2), math.floor(entity.height/2)
	local points = { p1.x + hw, p1.y + hh, p2.x + hw, p2.y + hh }
	local leftLine = drawableLine.fromPoints(points, entity.pathColor, 1)
	local rightLine = drawableLine.fromPoints(points, entity.pathColor, 1)

	leftLine:setOffset(0, 4.5)
	rightLine:setOffset(0, -4.5)

	leftLine.depth = 5000
	rightLine.depth = 5000

	for _, sprite in ipairs(leftLine:getDrawableSprite()) do
		table.insert(sprites, sprite)
	end

	for _, sprite in ipairs(rightLine:getDrawableSprite()) do
		table.insert(sprites, sprite)
	end

end

function CustomCursedMovingSolidThingy.sprite(room, entity)
	local sprites = {}
	generateSprite(sprites, entity, false)
	if #entity.nodes >= 1 then
		generateLine(sprites, entity, entity, entity.nodes[1])
		if #entity.nodes >= 2 then
			for i =#entity.nodes, 2, -1 do
				generateLine(sprites, entity, entity.nodes[i], entity.nodes[i - 1])
			end
		end
		for i = 1,#entity.nodes do
			generateSprite(sprites,entity,entity.nodes[i])
		end
	end

	return sprites
end

function CustomCursedMovingSolidThingy.nodeSprite(room, entity, node)
	local sprites = {}
	generateSprite(sprites, entity, node)
	return sprites
end

function CustomCursedMovingSolidThingy.nodeRectangle(room,entity,node)
	return utils.rectangle(node.x,node.y,entity.width,entity.height)
end

return CustomCursedMovingSolidThingy
