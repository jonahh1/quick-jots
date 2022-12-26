public class DictionaryResponse
{
  public string Word { get; set; }
  public Definition[] Definitions { get; set; }
}

public class Definition
{
  public string DefinitionText { get; set; }
  public string PartOfSpeech { get; set; }
}

class WordIntellisense
{
  public static string word;
  public static string jsonResponse;
  public static bool menuOpen = false;
  public static vec2 menuPos = new vec2(0);
  public static void DrawMenu()
  {
    if (!menuOpen) return;
    rect rec = new rect(menuPos, 200, 80);
    if (!Utils.MouseOnRec(rec) && Input.IsLeftMouse(ClickMode.pressed))
    {
      menuOpen = false;
      return;
    }
    Draw.Rectangle(rec, env.theme.colors["input menus"]);
    Draw.Text(word, rec.xy, 18, "fff");
    //var dictionaryAPI = JsonSerializer.Deserialize()
    if (jsonResponse == null) return;
    DictionaryResponse dictionaryAPI = JsonSerializer.Deserialize<DictionaryResponse>(jsonResponse)!;
    //dictionaryAPI.
  }

  public static async void GetDictionaryAPIResponse()
  {
    
    HttpClient client = new HttpClient();

    // Make the GET request
    HttpResponseMessage response = await client.GetAsync("https://api.dictionaryapi.dev/api/v2/entries/en/hello");

    // Check the status code of the response
    if (response.IsSuccessStatusCode)
    {
      // Read the content of the response
      jsonResponse = await response.Content.ReadAsStringAsync();
    }
    else
    {
      Console.WriteLine($"Request failed with status code: {response.StatusCode}");
    }
  }
}