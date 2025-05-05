using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace LINQToObjects.JSON;

public class XmlToJson
{
    public static void Convert(string xmlFilePath, string jsonFilePath)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            string rawJson = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

            var jsonObj = JObject.Parse(rawJson);

            jsonObj.Remove("?xml");

            JObject cleanedJson = (JObject)RemoveAtSigns(jsonObj);

            File.WriteAllText(jsonFilePath, cleanedJson.ToString(Formatting.Indented));

            Console.WriteLine("JSON successfully generated");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private static JToken RemoveAtSigns(JToken token)
    {
        if (token is JObject obj)
        {
            var newObj = new JObject();
            foreach (var property in obj.Properties())
            {
                string newName = property.Name.StartsWith("@") ? property.Name.Substring(1) : property.Name;
                newObj[newName] = RemoveAtSigns(property.Value);
            }

            return newObj;
        }
        else if (token is JArray array)
        {
            var newArray = new JArray();
            foreach (var item in array)
            {
                newArray.Add(RemoveAtSigns(item));
            }

            return newArray;
        }
        else
        {
            return token;
        }
    }
}
