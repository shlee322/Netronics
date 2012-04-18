Netronics.Properties = {}

function Netronics.Properties:Create(address, port, protocol, handler)
	return {address=address, port=port, protocol=protocol, handler=handler}
end
