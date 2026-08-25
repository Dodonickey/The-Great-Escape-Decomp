using System.Collections.Generic;

public class ConnectionSlot
{
	public ConnectionSlotType m_connectionSlotType;

	public List<GEConnectionC> m_connections;

	public GEControlledValue m_value;

	public int m_index;

	public bool m_triggered;

	public ConnectionSlot(ConnectionSlotType _connectionSlotType, int _index)
	{
		m_connectionSlotType = _connectionSlotType;
		m_connections = new List<GEConnectionC>();
		m_value = new GEControlledValue();
		m_index = _index;
		m_triggered = false;
	}
}
