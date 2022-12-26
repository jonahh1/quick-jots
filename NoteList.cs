
class NoteList
{
  public static rect rec = rect.zero;
  public static bool menuOpen = false;
  public static void DrawNoteList()
  {
    rec = new rect(0,0,300, env.window.h);
    Draw.Rectangle(rec, env.theme.colors["note list bg"]);
    float buffer = 24;
    vec2 pos = rec.xy+buffer;
    vec2 size = new vec2(rec.w-buffer*3.5f, buffer);
    //foreach (Note note in notes)
    bool rightClickedOnNote = false;
    for (int i = 0; i < NoteManager.notes.Count; i++)
    {
      rect icon = new rect(pos.x, pos.y, buffer);
      rect text = new rect(pos.x + buffer*1.5f, pos.y, size.x, buffer);
      float curvature = buffer/4;
      rect totalRec = new rect(0, icon.y, rec.w, icon.h);
      //notes[i].select.rec = new rect(text.x+text.w, text.y, buffer);
      if (Utils.MouseOnRec(totalRec))
      {
        if (Input.IsRightMouse(ClickMode.pressed))
        {
          selectedNoteIndex = i;
          menuPos = env.mousePos;
          rcMenuTimer = 0;
          rcMenu = true;
          selectingNote = true;
          rightClickedOnNote = true;
        }
        else if (Input.IsLeftMouse(ClickMode.pressed))
        {
          NoteContent.selectedNoteIndex = i;

        }
        Draw.Rectangle(totalRec, NoteManager.notes[i].bannerCol+"22");
      }
      if (NoteContent.selectedNoteIndex == i)
      {
        Raylib.DrawRectangleGradientH(
          (int)(text.x+text.w-curvature), 
          (int)text.y+Window.menuBarHeight, 
          (int)(buffer+curvature), 
          (int)(text.h),Utils.HexToRGB(NoteManager.notes[i].bannerCol), Utils.HexToRGB(NoteManager.notes[i].bannerCol+"00")
        );
      }
      Draw.RectangleCurved(icon, new rect(curvature,0,curvature,0), NoteManager.notes[i].bannerCol);
      Draw.RectangleCurved(text, new rect(0,curvature,0,curvature), NoteManager.notes[i].bannerCol);
      
      Draw.Icon(NoteManager.notes[i].icon, icon.xy, 0, env.theme.colors["icons"]);
      Draw.TextAnchored(NoteManager.notes[i].title, new vec2(text.x + curvature, text.y + text.h/2), Anchor.middleLeft, 16, env.theme.colors["text"]);
      
      pos.y += buffer*1.5f;
    }
    if (!rightClickedOnNote && Input.IsRightMouse(ClickMode.pressed))
    {
      menuPos = env.mousePos;
      rcMenuTimer = 0;
      rcMenu = true;
      selectingNote = false;
    }
    if (!MenuBarOptions.menuOpen && Utils.MouseOnRec(rec))
    {
      RightClickMenu();
      EditNoteMenu();
    }
    menuOpen = rcMenu || editNoteMenu;
  }
  public static Func<rect, float, rect> MenuLambda = (r, t) => new rect(r.x, r.y, t, r.h);
  public static int selectedNoteIndex = -1;
  public static vec2 menuPos = vec2.zero;
  public static float rcMenuTimer = 0; // right click menu
  public static button rcBtnA = new button() {changeRect = true, rectRule = MenuLambda};
  public static button rcBtnB = new button() {changeRect = true, rectRule = MenuLambda};
  public static button rcBtnC = new button() {changeRect = true, rectRule = MenuLambda};
  public static Note? lastCopiedNote = null;
  
  public static bool editNoteMenu = false;
  public static bool rcMenu = false;
  public static bool selectingNote = false;
  public static void RightClickMenu()
  {
    if (!rcMenu)
    {
      rcMenuTimer = Math.Max(rcMenuTimer-Raylib.GetFrameTime() * 5, 0);
      return;
    }
    rcMenuTimer = Math.Min(rcMenuTimer+Raylib.GetFrameTime() * 5, 1);

    rect rec = new rect(menuPos.x,menuPos.y, 100,48+(selectingNote?24:0));
    Draw.RectangleShadow(rec, Utils.LerpCol("0000","0008",rcMenuTimer));
    Draw.Rectangle(rec,Utils.LerpCol(env.theme.colors["input menus"]+"00",env.theme.colors["input menus"],rcMenuTimer));
    Draw.RectangleOutline(rec, -1, false, env.theme.colors["accent"]);
    
    if (selectingNote)
    {
      bool copy = false;
      bool delete = false;
      #region edit button
        rcBtnA.rec = new rect(rec.x, rec.y, rec.w, 24);
        rcBtnA.max = rec.w;
        string editBtnAccentCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],rcBtnA.timer);
        editNoteMenu = Button.AnimatedAction(rcBtnA).state;
        Draw.Icon(MyIcons.penInCircle, rcBtnA.rec.xy, 0, editBtnAccentCol);
        Draw.TextAnchored("edit", new vec2(rcBtnA.rec.x+24,rcBtnA.rec.y+rcBtnA.rec.h/2), Anchor.middleLeft, 18, editBtnAccentCol);
      #endregion

      #region copy button
        rcBtnB.rec = new rect(rec.x, rec.y+24, rec.w, 24);
        rcBtnB.max = rec.w;
        string copyBtnAccentCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],rcBtnB.timer);
        copy = Button.AnimatedAction(rcBtnB).state;
        Draw.Icon(MyIcons.twoSquares, rcBtnB.rec.xy, 0, copyBtnAccentCol);
        Draw.TextAnchored("copy", new vec2(rcBtnB.rec.x+24,rcBtnB.rec.y+rcBtnB.rec.h/2), Anchor.middleLeft, 18, copyBtnAccentCol);
      #endregion

      #region delete button
        rcBtnC.rec = new rect(rec.x, rec.y+48, rec.w, 24);
        rcBtnC.max = rec.w;
        string deleteBtnAccentCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent B"],rcBtnC.timer);
        delete = Button.AnimatedAction(rcBtnC).state;
        Draw.Icon(MyIcons.xInCircle, rcBtnC.rec.xy, 0, deleteBtnAccentCol);
        Draw.TextAnchored("delete", new vec2(rcBtnC.rec.x+24,rcBtnC.rec.y+rcBtnC.rec.h/2), Anchor.middleLeft, 18, deleteBtnAccentCol);
      #endregion
      if (editNoteMenu)
      {
        //menuPos = env.mousePos;
        editMenuTimer = 0;
        editNoteMenu = true;
        newNote = NoteManager.notes[selectedNoteIndex];
        titlePointer = newNote.title.Length-1;
        bannerPointer = newNote.bannerCol.Length-1;
        rcMenu = false;
      }
      if (copy)
      {
        lastCopiedNote = NoteManager.notes[selectedNoteIndex];
        rcMenu = false;
      }
      if (delete)
      {
        NoteManager.notes.RemoveAt(selectedNoteIndex);
        rcMenu = false;
      }
    }
    else
    {
      bool paste = false;
      #region new note button
        rcBtnA.rec = new rect(rec.x, rec.y, rec.w, 24);
        rcBtnA.max = rec.w;
        string newNoteBtnAccentCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],rcBtnA.timer);
        editNoteMenu = Button.AnimatedAction(rcBtnA).state;
        Draw.Icon(MyIcons.plusInCircle, rcBtnA.rec.xy, 0, newNoteBtnAccentCol);
        Draw.TextAnchored("new note", new vec2(rcBtnA.rec.x+24,rcBtnA.rec.y+rcBtnA.rec.h/2), Anchor.middleLeft, 18, newNoteBtnAccentCol);
      #endregion

      #region paste button
        rcBtnB.rec = new rect(rec.x, rec.y+24, rec.w, 24);
        rcBtnB.max = rec.w;
        string copyBtnAccentCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],rcBtnB.timer);
        paste = Button.AnimatedAction(rcBtnB).state;
        Draw.Icon(MyIcons.twoSquares, rcBtnB.rec.xy, 0, copyBtnAccentCol);
        Draw.TextAnchored("paste", new vec2(rcBtnB.rec.x+24,rcBtnB.rec.y+rcBtnB.rec.h/2), Anchor.middleLeft, 18, copyBtnAccentCol);
      #endregion
      if (editNoteMenu)
      {
        //menuPos = env.mousePos;
        NoteManager.notes.Add(new Note());
        editMenuTimer = 0;
        editNoteMenu = true;
        newNote = NoteManager.notes.Last();
        selectedNoteIndex = NoteManager.notes.Count-1;
        titlePointer = newNote.title.Length-1;
        bannerPointer = newNote.bannerCol.Length-1;
        rcMenu = false;
      }
      if (paste && lastCopiedNote.HasValue)
      {
        NoteManager.notes.Add(lastCopiedNote.Value);
        rcMenu = false;
      }
    }
    if (!Utils.MouseOnRec(rec) && Input.IsLeftMouse(ClickMode.pressed)) rcMenu = false;
  }

  static float editMenuTimer = 0;
  static Func<rect, float, rect> EditMenuLambda = (r, t) => new rect(r.x, r.y, r.w, t);
  static Func<rect, float, rect> EditMenuTabLambda = (r, t) => new rect(r.x+r.w - t, r.y, t, r.h);
  static button acceptBtn = new button() {changeRect = true, rectRule = EditMenuLambda};
  static button cancelBtn = new button() {changeRect = true, rectRule = EditMenuLambda};

  static button editIconTab = new button() {changeRect = true, rectRule = EditMenuTabLambda};
  static button editTitleTab = new button() {changeRect = true, rectRule = EditMenuTabLambda};
  static button editBannerTab = new button() {changeRect = true, rectRule = EditMenuTabLambda};
  static button editFontTab = new button() {changeRect = true, rectRule = EditMenuTabLambda};
    static button pickFontBtn = new button() {changeRect = false, rectRule = EditMenuTabLambda};
    static List<string> fontPickerOptions = null;
    static List<Font> fontPickerOptionsFonts = null;

  static int editMode = 1;
  static Note newNote = new Note();
  static int titlePointer = 0;
  static int bannerPointer = 0;
  public static void EditNoteMenu()
  {
    if (!editNoteMenu)
    {
      editMenuTimer = Math.Max(editMenuTimer-Raylib.GetFrameTime() * 5, 0);
      return;
    }
    editMenuTimer = Math.Min(editMenuTimer+Raylib.GetFrameTime() * 5, 1);
    vec2 size = new vec2(NoteList.rec.w + 48, 72+Window.iconSheet.height+24);
    rect rec = new rect((env.window.wh-size)/2, size.x, size.y);

    Draw.RectangleShadow(rec, Utils.LerpCol("0000","0006",editMenuTimer));
    Draw.Rectangle(rec,Utils.LerpCol(env.theme.colors["input menus"]+"00",env.theme.colors["input menus"],editMenuTimer));
    Draw.RectangleOutline(rec, -1, false, env.theme.colors["accent"]);
    Draw.Icon(MyIcons.paintersPalette, rec.xy, 0, env.theme.colors["icons"]);
    Draw.TextAnchored("note appearance editor", new vec2(rec.x+36,rec.y+12), Anchor.middleLeft, 18, env.theme.colors["text"]);
    Draw.Line(new vec2(rec.x, rec.y+24.5f), new vec2(rec.x+rec.w, rec.y+24.5f), 1, env.theme.colors["accent"]);
    Draw.TextAnchored("preview", new vec2(rec.x + 3, rec.y+rec.h-48), Anchor.middleLeft, 16, env.theme.colors["accent"]);
    Draw.Line(new vec2(rec.x + Utils.TextSize("preview", 16).x+6, rec.y+rec.h-48.5f), new vec2(rec.x+rec.w, rec.y+rec.h-48.5f), 1, env.theme.colors["accent"]);
    bool accept = false;
    bool cancel = false;
    #region accept button
      acceptBtn.rec = new rect(rec.x+rec.w-48, rec.y, 24);
      acceptBtn.max = 24;
      string acceptBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],acceptBtn.timer);
      string acceptBtnTextCol = Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],acceptBtn.timer);
      accept = Button.AnimatedAction(acceptBtn).state;
      Draw.Icon(MyIcons.tickInCircle, acceptBtn.rec.xy, 0, acceptBtnIconCol);
      Draw.TextAnchored("accept", new vec2(acceptBtn.rec.x+12,acceptBtn.rec.y), Anchor.bottomCenter, 18, acceptBtnTextCol);
    #endregion

    #region cancel button
      cancelBtn.rec = new rect(rec.x+rec.w-24, rec.y, 24);
      cancelBtn.max = 24;
      string cancelBtnIconCol = Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent B"],cancelBtn.timer);
      string cancelBtnTextCol = Utils.LerpCol(env.theme.colors["accent B"]+"00",env.theme.colors["accent B"],cancelBtn.timer);
      cancel = Button.AnimatedAction(cancelBtn).state;
      Draw.Icon(MyIcons.xInCircle, cancelBtn.rec.xy, 0, cancelBtnIconCol);
      Draw.TextAnchored("cancel", new vec2(cancelBtn.rec.x+30,cancelBtn.rec.y+12), Anchor.middleLeft, 18, cancelBtnTextCol);
    #endregion
    Draw.Line(new vec2(rec.x+rec.w-24.5f, rec.y+24), new vec2(rec.x+rec.w-24.5f, rec.y+rec.h-48), 1, env.theme.colors["accent"]);

    #region tabs
      #region edit icon tab
        editIconTab.rec = new rect(rec.x+rec.w-24, rec.y+24, 24);
        editIconTab.max = 24;
        if (Button.AnimatedAction(editIconTab).state) editMode = 0;
        Draw.Icon(MyIcons.iconsInCircle, editIconTab.rec.xy, 0, Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],editIconTab.timer));
        Draw.TextAnchored("icon", new vec2(editIconTab.rec.x+30,editIconTab.rec.y+12), Anchor.middleLeft, 18, Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],editIconTab.timer));
      #endregion
      #region edit title tab
        editTitleTab.rec = new rect(rec.x+rec.w-24, rec.y+48, 24);
        editTitleTab.max = 24;
        if (Button.AnimatedAction(editTitleTab).state) editMode = 1;
        Draw.Icon(MyIcons.TInCircle, editTitleTab.rec.xy, 0, Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],editTitleTab.timer));
        Draw.TextAnchored("title", new vec2(editTitleTab.rec.x+30,editTitleTab.rec.y+12), Anchor.middleLeft, 18, Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],editTitleTab.timer));
      #endregion
      #region edit banner tab
        editBannerTab.rec = new rect(rec.x+rec.w-24, rec.y+72, 24);
        editBannerTab.max = 24;
        if (Button.AnimatedAction(editBannerTab).state) editMode = 2;
        Draw.Icon(MyIcons.paintBucketInCircle, editBannerTab.rec.xy, 0, Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],editBannerTab.timer));
        Draw.TextAnchored("banner color", new vec2(editBannerTab.rec.x+30,editBannerTab.rec.y+12), Anchor.middleLeft, 18, Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],editBannerTab.timer));
      #endregion
      #region edit font tab
        editFontTab.rec = new rect(rec.x+rec.w-24, rec.y+96, 24);
        editFontTab.max = 24;
        if (Button.AnimatedAction(editFontTab).state) editMode = 3;
        Draw.Icon(MyIcons.AInCircle, editFontTab.rec.xy, 0, Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],editFontTab.timer));
        Draw.TextAnchored("font", new vec2(editFontTab.rec.x+30,editFontTab.rec.y+12), Anchor.middleLeft, 18, Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],editFontTab.timer));
      #endregion
    #endregion
   
    if (editMode == 0) // icon
    {
      vec2 pos = new vec2(rec.x + ((rec.w-24)-Window.iconSheet.width)/2,rec.y+36);
      for (float y = pos.y; y < pos.y + Window.iconSheet.height; y+=24)
      {
        for (float x = pos.x; x < pos.x + Window.iconSheet.width; x+=24)
        {
          rect r = new rect(x,y,24);
          if (Utils.MouseOnRec(r))
          {
            if (Input.IsLeftMouse(ClickMode.pressed)) newNote.icon = r.xy-pos;
            Draw.Rectangle(r, env.theme.colors["action btn on"]);
          }
        }
      }
      Draw.Texture(Window.iconSheet, pos, 0, env.theme.colors["icons"]);
    }
    else if (editMode == 1) // title
    {
      MyInput.TextBox(new vec2(rec.x+12, rec.y+56), ref newNote.title, ref titlePointer, 30);
    }
    else if (editMode == 2) // banner col
    {
      MyInput.TextBox(new vec2(rec.x+12, rec.y+56), ref newNote.bannerCol, ref bannerPointer, 9);
      //if (newNote.bannerCol.Replace("#","").Length == 7) newNote.bannerCol += newNote.bannerCol.Substring(7,1);
    }
    else if (editMode == 3) // font
    {
      pickFontBtn.rec = new rect(rec.x+6,rec.y+30, 24);
      pickFontBtn.max = 24;
      if (Button.AnimatedAction(pickFontBtn).state)
      {
        fontPickerOptions = Dialogs.chooseFont();
        if (fontPickerOptions != null)
        {
          fontPickerOptionsFonts = new List<Font>();
          foreach (string path in fontPickerOptions)
          {
            fontPickerOptionsFonts.Add(Raylib.LoadFont(path));
            Raylib.SetTextureFilter(fontPickerOptionsFonts.Last().texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
          }
        }
      }
      Draw.Icon(MyIcons.fntInFile, pickFontBtn.rec.xy, 0, Utils.LerpCol(env.theme.colors["icons"],env.theme.colors["accent"],pickFontBtn.timer));
      Draw.TextAnchored("pick font", new vec2(pickFontBtn.rec.x+30,pickFontBtn.rec.y+12), Anchor.middleLeft, 18, Utils.LerpCol(env.theme.colors["accent"]+"00",env.theme.colors["accent"],pickFontBtn.timer));
      string previewText = "preview font : AaBbYyZz";
      Raylib.DrawTextEx(newNote.font, previewText, ~new vec2(rec.x-30+(rec.w-Utils.TextSize(newNote.font, previewText, 18, 1).x), rec.y+30+Window.menuBarHeight), 18, 1, Utils.HexToRGB(env.theme.colors["text"]));
        
      if (fontPickerOptions != null)
      {
        //fontPickerOptionsFonts = new List<Font>();
        Draw.TextAnchored("options:", new vec2(rec.x+6,rec.y+60), Anchor.topLeft, 18, env.theme.colors["text"]);
        float x = rec.x+6;
        for (int i = 0; i < fontPickerOptionsFonts.Count; i++)
        {
          rect r = new rect(x, rec.y+84, 24,24);
          if (Utils.MouseOnRec(r))
          {
            if (Input.IsLeftMouse(ClickMode.pressed))
            {
              newNote.fontPath = fontPickerOptions[i];
              newNote.font = fontPickerOptionsFonts[i];
            }
            Draw.Rectangle(r,env.theme.colors["action btn on"]);
          }
          Draw.RectangleOutline(r, -1, false, env.theme.colors["accent"]);
          //Raylib.SetTextureFilter(f.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
          //Draw.TextAnchored("Aa", r.center, Anchor.middleCenter, 18, env.theme.colors["text"]);
          Raylib.DrawTextEx(fontPickerOptionsFonts[i], "Aa", ~((r.center-(Utils.TextSize(fontPickerOptionsFonts[i], "Aa", 18, 1)/2)+new vec2(0,Window.menuBarHeight))), 18, 1, Utils.HexToRGB(env.theme.colors["text"]));
          
          //Raylib.UnloadFont(f);
          x += 30;
        }
      }
      else
      {
        Draw.TextAnchored("press the 'fnt file' button to pick a", new vec2(rec.x+6,rec.y+60), Anchor.topLeft, 18, env.theme.colors["text"]+"40");
        Draw.TextAnchored("new font from the preinstalled ones", new vec2(rec.x+6,rec.y+78), Anchor.topLeft, 18, env.theme.colors["text"]+"40");
      }
      //MyInput.TextBox(new vec2(rec.x+12, rec.y+56), ref newNote.fontPath, ref fontPointer, 30);
      //if (newNote.bannerCol.Replace("#","").Length == 7) newNote.bannerCol += newNote.bannerCol.Substring(7,1);
    }

    #region note preview
      vec2 previewPos = new vec2(rec.x+24, rec.y+rec.h-36);
      rect icon = new rect(previewPos.x, previewPos.y, 24);
      rect text = new rect(previewPos.x + 24*1.5f, previewPos.y, rec.w-84, 24);
      float curvature = 6;
      Draw.RectangleCurved(icon, new rect(curvature,0,curvature,0), newNote.bannerCol);
      Draw.RectangleCurved(text, new rect(0,curvature,0,curvature), newNote.bannerCol);
      
      Draw.Icon(newNote.icon, icon.xy, 0, env.theme.colors["icons"]);
      Draw.TextAnchored(newNote.title, new vec2(text.x + curvature, text.y + text.h/2), Anchor.middleLeft, 16, env.theme.colors["text"]);
    #endregion
    if (accept)
    {
      NoteManager.notes[selectedNoteIndex] = newNote;
    }
    if (cancel || accept)
    {
      editNoteMenu = false;
      editMode = 1;
      fontPickerOptions = null;
    }
    //if (!Utils.MouseOnRec(rec) && Input.IsLeftMouse(ClickMode.pressed) && editMenuTimer >= 1) editNoteMenu = false;
  }
}