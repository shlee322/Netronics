Netronics.PacketBuffer = {}

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

function Netronics.PacketBuffer:Create()
	local new = TableCopy(Netronics.PacketBuffer)
	new._buffer = ""
	new._position = 1
	return new
end

function Netronics.PacketBuffer:AvailableBytes()
	return self._buffer:len() - self._position
end

function Netronics.PacketBuffer:BeginBufferIndex()
	self._position = 1
end

function Netronics.PacketBuffer:EndBufferIndex()
	print(self._position)
	print(self._buffer:len())
	print("asdfasdfasdfsadfasdfsadfasdfs")
	self._buffer = self._buffer:sub(self._position)
	print(self._buffer:sub(2,7))
	self._position = 1
end

function Netronics.PacketBuffer:ResetBufferIndex()
	self._position = 1
end

function Netronics.PacketBuffer:Write(value)
	self._buffer = self._buffer..value
	self._position = self._buffer:len()
end

function Netronics.PacketBuffer:Read(value)
	self._position = self.position + 0 --읽은만큼
end

function Netronics.PacketBuffer:ReadByte()
	local byte = self._buffer:byte(self._position)
	self._position = self._position + 1
	return byte
end

function Netronics.PacketBuffer:ReadBytes(len)
	local data = self._buffer:sub(self._position, self._position + len)
	self._position = self._position + len
	
	return data
end

function Netronics.PacketBuffer:ReadUInt32()
	return self:ReadByte() * 0x1000000 + self:ReadByte() * 0x10000 + self:ReadByte() * 0x100 + self:ReadByte()
end
