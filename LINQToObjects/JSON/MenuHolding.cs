namespace LINQToObjects.JSON;

public static class MenuHolding
{
    public static void MakeHeader(string str) => Console.WriteLine(string.Concat(new string('-', 55),"\n\t\t\t", str, "\n", new string('-', 55)));

    public static void MenuHold()
    {
        Console.Clear();
        MenuHolding.MakeHeader("MENU");
        Console.WriteLine("What you want to do?\n1 - Save to JSON\t2 - Load from JSON\n3 - Example with JsonDocument\n4 - Example with JsonNode");
        switch (Console.ReadLine())
        {
            case "1":
                JsonSerialize.SaveToJson();
                break;
            case "2":
                JsonSerialize.LoadFromJson();
                break;
            case "3":
                JsonDoc.ReadHotelsJsonWithJsonDocument(JsonSerialize.filePath);
                break;
            case "4":
                JsonNodeExample.MenuJsonNode();
                break;
        }
        Console.ReadKey();
        Console.Clear();
    }
    
}