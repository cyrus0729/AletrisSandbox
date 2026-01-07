logging.info("this shit even worknig?", "aletrisSandbox")

local _orig_func =  selectionItemUtils.addNodeToSelection

function selectionItemUtils.addNodeToSelection(room, layer, item)
    logging.info("congratulations you fuckin did the add node thing blahblah lorem ipsum dolor sit amet", "aletrisSandbox")
    _orig_func(room, layer, item)
end