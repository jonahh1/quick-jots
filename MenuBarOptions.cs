class MenuBarOptions
{
  
  static Func<rect, float, rect> lambda = (r, t) => new rect(r.x, r.y, r.w+t, r.h);
  public static button fileBtn = new button() {changeRect = true, rectRule = lambda};
  public static button settingsBtn = new button() {changeRect = true, rectRule = lambda};

  public static bool menuOpen = false;
  static bool fileMenuOpen = false;
  static bool settingsMenuOpen = false;

  //public static string currentCollection = "collection.qj";
  public static void DrawMenuBarOptions()
  {
    //Draw.Rectangle(0,-24,24,24,env.theme.colors["menu bar"]);
    fileBtn.rec = new rect(24,-24,24);
    fileBtn.max = Utils.TextSize("file", 18).x+12;
    if (Button.AnimatedAction(fileBtn, "0000","0000").state)
    {
      fileMenuOpen = true;
      settingsMenuOpen = false;
    }
    string recCol = Utils.LerpCol(env.theme.colors["action btn off"], env.theme.colors["action btn on"], fileBtn.timer);
    Draw.RectangleCurved(fileBtn.currentState.visualRec, new rect(fileBtn.rec.h/2), recCol);
    Draw.Icon(MyIcons.file, fileBtn.rec.xy,0,env.theme.colors["icons"]);
    string textCol = Utils.LerpCol(env.theme.colors["text"]+"00", env.theme.colors["text"], fileBtn.timer);
    Draw.TextAnchored("file", fileBtn.rec.xy+new vec2(24,fileBtn.rec.h/2), Anchor.middleLeft, 18, textCol);

    settingsBtn.rec = new rect(fileBtn.currentState.visualRec.w + 24,-24,24);
    settingsBtn.max = Utils.TextSize("settings", 18).x+12;
    if (Button.AnimatedAction(settingsBtn, "0000","0000").state)
    {
      settingsMenuOpen = true;
      fileMenuOpen = false;
    }
    recCol = Utils.LerpCol(env.theme.colors["action btn off"], env.theme.colors["action btn on"], settingsBtn.timer);
    Draw.RectangleCurved(settingsBtn.currentState.visualRec, new rect(settingsBtn.rec.h/2), recCol);
    Draw.Icon(MyIcons.smallGear, settingsBtn.rec.xy,0,env.theme.colors["icons"]);
    textCol = Utils.LerpCol(env.theme.colors["text"]+"00", env.theme.colors["text"], settingsBtn.timer);
    Draw.TextAnchored("settings", settingsBtn.rec.xy+new vec2(24,settingsBtn.rec.h/2), Anchor.middleLeft, 18, textCol);
    if (Utils.MouseOnRec(new rect(0,-24,settingsBtn.currentState.visualRec.x+settingsBtn.currentState.visualRec.w,24))) Window.isMovable = false;
    else Window.isMovable = true;
    
    menuOpen = fileMenuOpen || settingsMenuOpen;
    DrawFileMenu();
  }

  static Func<rect, float, rect> downLambda = (r, t) => new rect(r.x, r.y, r.w, t);
  static Func<rect, float, rect> leftLambda = (r, t) => new rect(r.x+r.w - t, r.y, t, r.h);
  static button cancelBtn = new button() {changeRect = true, rectRule = downLambda};
  static button newBtn = new button() {changeRect = true, rectRule = downLambda};
  static button openBtn = new button() {changeRect = true, rectRule = downLambda};
  static button saveBtn = new button() {changeRect = true, rectRule = downLambda};
  static button saveAsBtn = new button() {changeRect = true, rectRule = downLambda};
  static void DrawFileMenu()
  {
    if (!fileMenuOpen) return;
    vec2 size = new vec2(248, 150);
    rect rec = new rect((env.window.wh-size)/2, size.x,size.y);
    Draw.RectangleShadow(rec, "0008");
    Draw.Rectangle(rec, env.theme.colors["input menus"]);
    Draw.RectangleOutline(rec,-1, false, env.theme.colors["accent"]);
    Draw.Icon(MyIcons.file, rec.xy, 0, env.theme.colors["icons"]);
    Draw.TextAnchored("file / collection menu", new vec2(rec.x+24,rec.y+12), Anchor.middleLeft, 18, env.theme.colors["text"]);
    #region cancel button
      cancelBtn.rec = new rect(rec.x+rec.w-24, rec.y, 24);
      cancelBtn.max = 24;
      string cancelBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent B"],cancelBtn.timer);
      string cancelBtnTextCol = Utils.LerpCol(env.theme.colors["accent B"]+"00",env.theme.colors["accent B"],cancelBtn.timer);
      bool cancel = Button.AnimatedAction(cancelBtn).state;
      Draw.Icon(MyIcons.xInCircle, cancelBtn.rec.xy, 0, cancelBtnIconCol);
      Draw.TextAnchored("cancel", new vec2(cancelBtn.rec.x+30,cancelBtn.rec.y+12), Anchor.middleLeft, 18, cancelBtnTextCol);
    #endregion

    #region collection option
      #region new
        newBtn.rec = new rect(rec.x, rec.y+24, 24);
        newBtn.max = 24;
        string newBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],newBtn.timer);
        string newBtnTextCol = Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],newBtn.timer);
        bool newCollection = Button.AnimatedAction(newBtn).state;
        Draw.Icon(MyIcons.starInFile, newBtn.rec.xy, 0, newBtnIconCol);
        Draw.TextAnchored("new", new vec2(newBtn.rec.x+30,newBtn.rec.y+12), Anchor.middleLeft, 18, newBtnTextCol);
      #endregion
      #region open
        openBtn.rec = new rect(rec.x+24+
          (12+Utils.TextSize("new",18).x)*newBtn.timer,
          rec.y+24, 24
        );
        openBtn.max = 24;
        string openBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],openBtn.timer);
        string openBtnTextCol = Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],openBtn.timer);
        bool openCollection = Button.AnimatedAction(openBtn).state;
        Draw.Icon(MyIcons.fileInOpenFolder, openBtn.rec.xy, 0, openBtnIconCol);
        Draw.TextAnchored("open", new vec2(openBtn.rec.x+30,openBtn.rec.y+12), Anchor.middleLeft, 18, openBtnTextCol);
      #endregion
      #region save
        saveBtn.rec = new rect(
          rec.x+48+
          (12+Utils.TextSize("new",18).x)*newBtn.timer+
          (12+Utils.TextSize("open",18).x)*openBtn.timer,
          rec.y+24, 24
        );
        saveBtn.max = 24;
        string SaveBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],saveBtn.timer);
        string SaveBtnTextCol = Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],saveBtn.timer);
        bool saveCollection = Button.AnimatedAction(saveBtn).state;
        Draw.Icon(MyIcons.floppyDisk, saveBtn.rec.xy, 0, SaveBtnIconCol);
        Draw.TextAnchored("save", new vec2(saveBtn.rec.x+30,saveBtn.rec.y+12), Anchor.middleLeft, 18, SaveBtnTextCol);
      #endregion
      #region save as
        saveAsBtn.rec = new rect(
          rec.x+72+
          (12+Utils.TextSize("new",18).x)*newBtn.timer+
          (12+Utils.TextSize("open",18).x)*openBtn.timer+
          (12+Utils.TextSize("save",18).x)*saveBtn.timer,
          rec.y+24, 24
        );
        saveAsBtn.max = 24;
        string SaveAsBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],saveAsBtn.timer);
        string SaveAsBtnTextCol = Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],saveAsBtn.timer);
        bool saveCollectionAs = Button.AnimatedAction(saveAsBtn).state;
        Draw.Icon(MyIcons.floppyDiskAndStar, saveAsBtn.rec.xy, 0, SaveAsBtnIconCol);
        Draw.TextAnchored("save as", new vec2(saveAsBtn.rec.x+30,saveAsBtn.rec.y+12), Anchor.middleLeft, 18, SaveAsBtnTextCol);
      #endregion
    #endregion
    Draw.Line(new vec2(rec.x, rec.y+24.5f), new vec2(rec.x+rec.w, rec.y+24.5f), 1, env.theme.colors["accent"]);
    
    float y = rec.y+48;
    foreach (string recentCollection in SettingsManager.profile.recentCollections)
    {
      string name = Path.GetFileName(recentCollection);
      rect recentCRec = new rect(rec.x, y, rec.w, 24);
      string iconCol = env.theme.colors["icons"];
      if (Utils.MouseOnRec(recentCRec))
      {
        recentCRec.x += 6;
        recentCRec.w -= 12;
        iconCol = env.theme.colors["accent"];

        Draw.RectangleOutline(recentCRec, 1, false, env.theme.colors["accent"]);

        if (Input.IsLeftMouse(ClickMode.pressed))
        {
          LoadCollection(recentCollection);
          cancel = true;
        }
      }
      Draw.Icon(MyIcons.minimize, recentCRec.xy, -90, iconCol);
      Draw.TextAnchored(name, new vec2(recentCRec.x+24, recentCRec.y+12), Anchor.middleLeft, 16, env.theme.colors["text"]);
      y += 24;
    }

    Draw.Line(new vec2(rec.x, rec.y+48.5f), new vec2(rec.x+rec.w, rec.y+48.5f), 1, env.theme.colors["accent"]);
    if (newCollection)
    {
      New();
      cancel = true;
    }
    else if (openCollection)
    {
      Open();
      cancel = true;
    }
    else if (saveCollection)
    {
      Save();
      cancel = true;
    }
    else if (saveCollectionAs)
    {
      SaveAs();
      cancel = true;
    }

    if (cancel) fileMenuOpen = false;
  }
  public static void New()
  {
    Save();
    NoteManager.newCollection();
    SettingsManager.profile.currentCollection = "";
  }
  public static void Open()
  {
    string result = Dialogs.OpenDialog();
    if (result != null) LoadCollection(result);
  }
  public static void LoadCollection(string path)
  {
    Save();
    SettingsManager.profile.currentCollection = path;
    NoteManager.LoadNotes();
  }
  public static void Save()
  {
    if (!SettingsManager.profile.recentCollections.Contains(SettingsManager.profile.currentCollection) && SettingsManager.profile.currentCollection != "")
      SettingsManager.profile.recentCollections.Add(SettingsManager.profile.currentCollection);
    if (!File.Exists(SettingsManager.profile.currentCollection) || SettingsManager.profile.currentCollection == "") SaveAs();
    else NoteManager.SaveNotes();
  }
  public static void SaveAs()
  {
    if (NoteManager.notes.Count == 0) return;

    string result = Dialogs.SaveAsDialog();

    if (result == null) return;
    
    SettingsManager.profile.currentCollection = result;
    NoteManager.SaveNotes();
  }
}