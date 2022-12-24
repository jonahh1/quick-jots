class Dialogs
{
  public static List<string> chooseFont()
  {
    FontDialog fontDialog = new FontDialog();

    if (fontDialog.ShowDialog() != DialogResult.Cancel)
    {
      var fontNameToFiles = new Dictionary<string, List<string>>();

      foreach (var fontFile in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)))
      {
        var fc = new System.Drawing.Text.PrivateFontCollection();

        if (File.Exists(fontFile)) fc.AddFontFile(fontFile);

        if ((!fc.Families.Any())) continue;

        var name = fc.Families[0].Name;

        // If you care about bold, italic, etc, you can filter here.
        if (!fontNameToFiles.TryGetValue(name, out var files))
        {
          files = new List<string>();
          fontNameToFiles[name] = files;
        }

        files.Add(fontFile);
      }

      if (!fontNameToFiles.TryGetValue(fontDialog.Font.Name, out var result)) return null;
      return result;
    }
    return null;
  }
  public static string OpenDialog()
  {
    string ret = null;
    OpenFileDialog dialog = new OpenFileDialog();
    dialog.Filter = "Quick-Jot files (*.qj)|*.qj|csv files (*.csv)|*.csv|All files (*.*)|*.*";
    if (dialog.ShowDialog() != DialogResult.Cancel) ret = dialog.FileName;
    return ret;
  }
  public static string SaveAsDialog()
  {
    string ret = null;
    SaveFileDialog dialog = new SaveFileDialog();
    dialog.Filter = "Quick-Jot files (*.qj)|*.qj|csv files (*.csv)|*.csv";
    if (dialog.ShowDialog() != DialogResult.Cancel) ret = dialog.FileName;
    return ret;
  }
}