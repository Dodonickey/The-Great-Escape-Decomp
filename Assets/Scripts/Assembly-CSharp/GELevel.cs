using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class GELevel : ILevel, ISerializable, IDeserializationCallback
{
    private string _name;

    private string _projectCode;

    private uint _levelIndex;

    private uint _chapterIndex;

    public List<EIC> items;

    public List<EIC> connections;

    public List<string> requiredResources;

    private EIC[] itemsFromDeserialization;

    private EIC[] connectionsFromDeserialization;

    public string name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public string projectCode
    {
        get
        {
            return _projectCode;
        }
        set
        {
            _projectCode = value;
        }
    }

    public uint levelIndex
    {
        get
        {
            return _levelIndex;
        }
        set
        {
            _levelIndex = value;
        }
    }

    public uint chapterIndex
    {
        get
        {
            return _chapterIndex;
        }
        set
        {
            _chapterIndex = value;
        }
    }

    public GELevel()
    {
        items = new List<EIC>();
        connections = new List<EIC>();
        requiredResources = new List<string>();
    }

    public GELevel(SerializationInfo info, StreamingContext ctxt)
    {
        name = (string)info.GetValue("name", typeof(string));
        projectCode = (string)info.GetValue("projectCode", typeof(string));

        // Store array references for OnDeserialization fixup stage
        itemsFromDeserialization = (EIC[])info.GetValue("items", typeof(EIC[]));
        connectionsFromDeserialization = (EIC[])info.GetValue("connections", typeof(EIC[]));

        try
        {
            chapterIndex = (uint)info.GetValue("chapterIndex", typeof(uint));
            levelIndex = (uint)info.GetValue("levelIndex", typeof(uint));
        }
        catch
        {
            chapterIndex = 0u;
            levelIndex = 0u;
        }
    }

    public void OnDeserialization(object sender)
    {
        // Runs AFTER .NET completes object graph fixups
        items = new List<EIC>();
        if (itemsFromDeserialization != null)
        {
            for (int i = 0; i < itemsFromDeserialization.Length; i++)
            {
                if (itemsFromDeserialization[i] != null)
                {
                    items.Add(itemsFromDeserialization[i]);
                }
            }
            itemsFromDeserialization = null;
        }

        connections = new List<EIC>();
        if (connectionsFromDeserialization != null)
        {
            for (int j = 0; j < connectionsFromDeserialization.Length; j++)
            {
                if (connectionsFromDeserialization[j] != null)
                {
                    connections.Add(connectionsFromDeserialization[j]);
                }
            }
            connectionsFromDeserialization = null;
        }
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("name", name);
        info.AddValue("chapterIndex", chapterIndex);
        info.AddValue("levelIndex", levelIndex);
        info.AddValue("projectCode", projectCode);
        info.AddValue("items", items != null ? items.ToArray() : new EIC[0]);
        info.AddValue("connections", connections != null ? connections.ToArray() : new EIC[0]);
    }
}