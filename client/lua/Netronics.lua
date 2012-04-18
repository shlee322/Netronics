Netronics = {}

local function TableCopy(t)
	local t2 = {}
	for k,v in pairs(t) do
		if type(v) == "table" then
			t2[k] = TableCopy(v)
		else
			t2[k] = v
		end
	end
	return t2
end

require 'Properties'
require 'PacketBuffer'

function Netronics:Create(properties)
	local new = TableCopy(Netronics)
	new.properties = properties
	return new
end

function Netronics:Start()
	self.cr = coroutine.create(function() self:Init() end)
	self:Update()
end

function Netronics:Update()
	coroutine.resume(self.cr)
end

function Netronics:Init()
	self.socket = require("socket").tcp()
	self.socket:settimeout(0, 't')
	self.socket:connect(self.properties.address,
		self.properties.port)
	self.buffer = Netronics.PacketBuffer:Create()

	self.properties.handler:Connected(self)
	self:Loop()
	self.properties.handler:DisConnected(self)
end

function Netronics:Loop()
	while true do
		coroutine.yield()
		local rdata = self.socket:receive()
		if rdata ~= nil then
			self.buffer:Write(rdata)
			while true do
				local data = self.properties.protocol.decoder:Decode(self, self.buffer)
				if data == nil then
					break
				end
				self.properties.handler:MessageReceive(self, data)
			end
		end
	end
end

function Netronics:Send(data)
	local buffer = self.properties.protocol.encoder:Encode(self, data)
	print(buffer._buffer)
	--self.socket:send(buffer._buffer)
end
