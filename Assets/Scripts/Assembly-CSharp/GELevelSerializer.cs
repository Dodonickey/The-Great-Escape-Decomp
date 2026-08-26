using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GELevelSerializer
{
    public static BinaryFormatter CreateFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
        formatter.Binder = new LegacyUnityBinder();
        return formatter;
    }

    public object DeSerializeLevel(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter formatter = CreateFormatter();
            return formatter.Deserialize(fileStream);
        }
    }

    public object DeSerializeUnityLevel(string path)
    {
        TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
        if (textAsset != null)
        {
            using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter formatter = CreateFormatter();
                return formatter.Deserialize(memoryStream);
            }
        }
        return null;
    }

    public void SerializeLevel(string path, object level)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            BinaryFormatter formatter = CreateFormatter();
            formatter.Serialize(fileStream, level);
        }
    }
}