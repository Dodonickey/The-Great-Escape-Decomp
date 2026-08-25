using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GELevelSerializer
{
	public void SerializeLevel(string _file, GELevel _levelData)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Binder = new VersionDeserializationBinder();
		Stream stream = File.Open(_file, FileMode.Create);
		binaryFormatter.Serialize(stream, _levelData);
		stream.Close();
	}

	public GELevel DeSerializeLevel(string _path)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Binder = new VersionDeserializationBinder();
		Stream stream = File.Open(_path, FileMode.Open);
		GELevel result = (GELevel)binaryFormatter.Deserialize(stream);
		stream.Close();
		return result;
	}

	public GELevel DeSerializeUnityLevel(string _path)
	{
		GELevel gELevel = null;
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Binder = new VersionDeserializationBinder();
		TextAsset textAsset = Resources.Load(_path) as TextAsset;
		if (textAsset != null)
		{
			Stream serializationStream = new MemoryStream(textAsset.bytes);
			return (GELevel)binaryFormatter.Deserialize(serializationStream);
		}
		Resources.UnloadAsset(textAsset);
		return null;
	}
}
