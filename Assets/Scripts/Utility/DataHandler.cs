using System.IO;
using System.Xml.Serialization;

public static class DataHandler
{

    public static void SaveData(string filePath, object obj)
    {
        SaveToXML(filePath, obj);
    }

    private static void SaveToXML(string filePath, object obj, bool appendFile = false)
    {
        var serializer = new XmlSerializer(obj.GetType());

        using (var stream = new StreamWriter(filePath, appendFile))
        {
            serializer.Serialize(stream, obj);
        }
    }

    public static T LoadData<T>(string filePath, string xmlRoot)
    {
        T result = LoadFromXML<T>(filePath, xmlRoot);
        return result;
    }

    private static T LoadFromXML<T>(string filePath, string xmlRoot)
    {
        T result;
        using (var reader = new StreamReader(filePath))
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T),
                new XmlRootAttribute(xmlRoot));
            result = (T)deserializer.Deserialize(reader);
        }
        return result;
    }
}