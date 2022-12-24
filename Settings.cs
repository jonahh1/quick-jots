class settingsProfile : Settings
{
  public int tabSize {get; set;}
  public bool textMatchesBannerColor {get; set;}
  public int fontSpacing {get; set;}
  public string currentCollection {get; set;}
  public List<string> recentCollections {get; set;}
}

class SettingsManager
{
  public static settingsProfile profile;
  public static void LoadSettings()
  {
    profile = JsonSerializer.Deserialize<settingsProfile>(File.ReadAllText("assets/json/settings.json"))!;
  }
  public static void SaveSettings()
  {
    string jsonString = JsonSerializer.Serialize(profile,new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("assets/json/settings.json", jsonString);
  }
}