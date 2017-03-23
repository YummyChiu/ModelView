using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SLS.Widgets.Table {
public class Table : UIBehaviour, ILayoutGroup {

  public enum SelectionMode {
    CELL,
    ROW }
  ;

  public Font font;
  public FontStyle fontStyle;
  public bool use2DMask;
  public Sprite fillerSprite;

  // GENERAL SETTINGS
  public int defaultFontSize = 12;
  public int scrollSensitivity = 5;
  public float leftMargin;
  public float rightMargin;
  public float horizontalSpacing;
  public Color bodyBackgroundColor = Color.white;
  public Color columnLineColor = Color.white;
  public int columnLineWidth = 1;
  public bool min100PercentWidth = true;
  public bool max100PercentWidth = false;
  public Sprite spinnerSprite;
  public Color spinnerColor = Color.white;
  public float rowAnimationDuration = 0.5f;
  public SelectionMode selectionMode = SelectionMode.CELL;
  public bool drawGizmos = false;
  public Color gizmoColor = new Color(0.4f, 0.4f, 0f, 0.6f);

  // HEADER SETTINGS
  public float minHeaderHeight = 50f;
  public float headerTopMargin;
  public float headerBottomMargin;
  public Color headerNormalColor = Color.white;
  public Color headerHoverColor = Color.white;
  public Color headerDownColor = Color.white;
  public Color headerBorderColor = Color.white;
  public Color headerTextColor = Color.black;
  public int headerIconWidth = 8;
  public int headerIconHeight = 16;

  // FOOTER SETTINGS
  public float minFooterHeight = 40f;
  public float footerTopMargin;
  public float footerBottomMargin;
  public Color footerBackgroundColor = Color.white;
  public Color footerBorderColor = Color.white;
  public Color footerTextColor = Color.black;

  // ROW SETTINGS
  public float minRowHeight = 40f;
  private float _minRowHeight = -1f;
  public float rowVerticalSpacing;
  public Color rowLineColor = Color.white;
  public int rowLineHeight = 1;
  public Color rowNormalColor = Color.black;
  public Color rowHoverColor = Color.black;
  public Color rowDownColor = Color.black;
  public Color rowSelectColor = Color.black;
  public Color cellHoverColor = new Color(0, 0, 0, 0f);
  public Color cellDownColor = new Color(0, 0, 0, 0f);
  public Color cellSelectColor = new Color(0, 0, 0, 0f);
  public Color rowTextColor = Color.white;
  public Action<RectTransform, string> tooltipHandler;
  public Action<PointerEventData, Datum> pointerDownHandler;
  public Action<PointerEventData, Datum> pointerUpHandler;

  // EXTRA TEXT
  // 0 - 1 (percent of frame extra text box should fill 1 == 100%)
  public float extraTextWidthRatio = 0.6f;
  public Color extraTextBoxColor = Color.black;
  public Color extraTextColor = Color.white;

  // SCROLLBAR
  public Color scrollBarBackround = new Color(0.5f, 0.5f, 0.5f, 0.1f);
  public Color scrollBarForeground = new Color(0.5f, 0.5f, 0.5f, 0.5f);


  //public Dictionary<string, Column> _columns;
  //public Dictionary<string, Column> columns { get { return this._columns; } }
  public List<Column> _columns;

  public List<Column> columns { get { return this._columns; } }

  [HideInInspector]
  public List<Row>
    rows;
  [HideInInspector]
  public TableDatumList
    data;
  public Action<Datum> selectionCallback;
  public Action<Datum, Column> selectionCallbackWithColumn;
  public Action<Datum, Column, RectTransform> selectionCallbackWithRT;
  public Func<Column, bool> headerActiveCallback;
  private Datum _selectedDatum;

  public Datum selectedDatum {
    get { return this._selectedDatum; }
    set { this.SetSelected(value); }
  }

  private Column _selectedColumn;

  public Column selectedColumn {
    get { return this._selectedColumn; }
  }

  [Obsolete("'setSelected' method is obsolete.  Please use 'SetSelected' method instead.")]
  public void setSelected(int x, int y, bool doCallback=true) {
    this.SetSelected(x, y, doCallback);
  }

  public void SetSelected(int x, int y, bool doCallback=true, bool animate=false) {
    Column column = _selectedColumn;
    Datum datum = _selectedDatum;
    if(x < _columns.Count && x >= 0) {
      column = _columns[x];
    }
    if(y < data.Count && y >= 0) {
      datum = data[y];
    }
    SetSelected(datum, column, doCallback, animate);
  }

  [Obsolete("'moveSelectionUp' method is obsolete.  Please use 'MoveSelectionUp' method instead.")]
  public void moveSelectionUp(bool doCallback=true) {
    this.MoveSelectionUp(doCallback);
  }

  public void MoveSelectionUp(bool doCallback=true) {
    int x = _columns.IndexOf(_selectedColumn);
    int y = data.IndexOf(_selectedDatum);
    y = y - 1;
    SetSelected(x, y, doCallback, true);
  }

  [Obsolete("'moveSelectionDown' method is obsolete.  Please use 'MoveSelectionDown' method instead.")]
  public void moveSelectionDown(bool doCallback=true) {
    this.MoveSelectionDown(doCallback);
  }

  public void MoveSelectionDown(bool doCallback=true) {
    int x = _columns.IndexOf(_selectedColumn);
    int y = data.IndexOf(_selectedDatum);
    y = y + 1;
    SetSelected(x, y, doCallback, true);
  }

  [Obsolete("'moveSelectionLeft' method is obsolete.  Please use 'MoveSelectionLeft' method instead.")]
  public void moveSelectionLeft(bool doCallback=true) {
    this.MoveSelectionLeft(doCallback);
  }

  public void MoveSelectionLeft(bool doCallback=true) {
    int x = _columns.IndexOf(_selectedColumn);
    int y = data.IndexOf(_selectedDatum);
    x = x - 1;
    SetSelected(x, y, doCallback);
  }

  [Obsolete("'moveSelectionRight' method is obsolete.  Please use 'MoveSelectionRight' method instead.")]
  public void moveSelectionRight(bool doCallback=true) {
    this.MoveSelectionRight(doCallback);
  }

  public void MoveSelectionRight(bool doCallback=true) {
    int x = _columns.IndexOf(_selectedColumn);
    int y = data.IndexOf(_selectedDatum);
    x = x + 1;
    SetSelected(x, y, doCallback);
  }

  [Obsolete("'setSelected' method is obsolete.  Please use 'SetSelected' method instead.")]
  public void setSelected(Datum d, Column c=null, bool doCallback=true) {
    this.SetSelected(d, c, doCallback);
  }

  public void SetSelected(Datum d, Column c=null, bool doCallback=true, bool animate=false) {
    Row row;
    this._selectedColumn = c;
    if(this._selectedDatum != d) {
      row = this.GetRowForDatum(this._selectedDatum);
      this._selectedDatum = d;
      if(row != null)
        row.SetColor();
    }
    row = this.GetRowForDatum(this._selectedDatum);
    if(row != null)
      row.SetColor();
    if(doCallback) {
      if(this.selectionCallback != null)
        this.selectionCallback(this._selectedDatum);
      if(this.selectionCallbackWithColumn != null)
        this.selectionCallbackWithColumn(this._selectedDatum, this._selectedColumn);
      if(this.selectionCallbackWithRT != null && row != null) {
        if(row != null && this._selectedColumn != null)
          this.selectionCallbackWithRT(this._selectedDatum, this._selectedColumn,
                                       row.cells[this._selectedColumn.idx].rt);
        else
          this.selectionCallbackWithRT(this._selectedDatum, null, null);
      }
    }
    if(animate)
      this.StartCoroutine(this.DoVertScrollUntilVisible(d));
  }

  private IEnumerator DoVertScrollUntilVisible(Datum d) {
    bool scrollUp = true;
    bool scrollDown = true;
    Vector3[] rowCorners = new Vector3[4];
    Vector3[] tabCorners = new Vector3[4];
    while(scrollUp || scrollDown && this.isActiveAndEnabled && d == this.selectedDatum) {
      scrollUp = false;
      scrollDown = false;
      Row r = this.GetRowForDatum(d);
      if(r == null) {
        if(this.rows != null && this.rows.Count > 0 && this.rows[0].datum != null) {
          if(this.data.IndexOf(this.rows[0].datum) > this.data.IndexOf(d))
            scrollUp = true;
          else
            scrollDown = true;
        }
      }
      else {
        r.rt.GetWorldCorners(rowCorners);
        this.bodyRect.rt.GetWorldCorners(tabCorners);
        //Debug.Log("MAX: " + rowCorners[1].y + " vs " + tabCorners[1].y);
        //Debug.Log("MIN: " + rowCorners[0].y + " vs " + tabCorners[0].y);
        if(rowCorners[1].y > tabCorners[1].y)
          scrollUp = true;
        if(rowCorners[0].y < tabCorners[0].y)
          scrollDown = true;
      }
      if(scrollUp)
        this.bodyScroller.verticalNormalizedPosition += 0.01f;
      else if(scrollDown)
        this.bodyScroller.verticalNormalizedPosition -= 0.01f;
      yield return new WaitForEndOfFrame();
    }
  }

  [Obsolete("'getSelectedElement' method is obsolete.  Please use 'GetSelectedElement' method instead.")]
  public Element getSelectedElement() {
    return this.GetSelectedElement();
  }

  public Element GetSelectedElement() {
    if(_selectedColumn != null && _selectedDatum != null) {
      int x = _columns.IndexOf(_selectedColumn);
      return _selectedDatum.elements[x];
    }
    return null;
  }

  [HideInInspector]
  public RectTransform
    root;
  private bool _hasHeader;

  public bool hasHeader { get { return this._hasHeader; } }

  private bool _hasHeaderIcons;

  public bool hasHeaderIcons { get { return this._hasHeaderIcons; } }

  private bool _hasFooter;

  public bool hasFooter { get { return this._hasFooter; } }

  private Datum headerDatum;
  private Datum footerDatum;
  [HideInInspector]
  public RectTransform
    headerRect;
  [HideInInspector]
  public Row
    headerRow;
  [HideInInspector]
  public RectTransform
    footerRect;
  [HideInInspector]
  public Row
    footerRow;
  [HideInInspector]
  public ScrollRect
    bodyScroller;
  [HideInInspector]
  public ScrollWatcher
    bodyScrollWatcher;
  [HideInInspector]
  public BodyRect
    bodyRect;
  [HideInInspector]
  public RectTransform
    bodySizer;
  [HideInInspector]
  public RectTransform
    horScrollerRt;
  [HideInInspector]
  public RectTransform
    verScrollerRt;
  [HideInInspector]
  public CanvasGroup
    loadingOverlay;
  private bool _hasColumnOverlay;

  public bool hasColumnOverlay { get { return this._hasColumnOverlay; } }

  [HideInInspector]
  public RectTransform
    columnOverlay;
  [HideInInspector]
  public RectTransform
    columnOverlayContent;
  [HideInInspector]
  public List<RectTransform>
    columnOverlayLines;
  private Factory factory;
  private Control control;
  [HideInInspector]
  public Column
    extraTextColumn;
  [HideInInspector]
  public InputCell
    inputCell;
  private bool _isRunning;

  public bool isRunning { get { return this._isRunning && this.isActiveAndEnabled; } }

  private bool _hasError;

  public bool hasError { get { return this._hasError; } }

  public void error(string message) {
    this._hasError = true;
    Debug.LogError(message);
  }

  private bool doingDirtyLater;
  private Vector2 lastRootSize;
  private Dictionary<string, Sprite> _sprites;

  public Dictionary<string, Sprite> sprites { get { return this._sprites; } }

  [Obsolete("'reset' method is obsolete.  Please use 'ResetTable' method instead.")]
  public void reset() {
    this.ResetTable();
  }

  public void ResetTable() {
    this._isRunning = false;
    this._hasError = false;
    this._hasHeader = false;
    this._hasFooter = false;
    this._hasColumnOverlay = false;
    this._hasHeaderIcons = false;
    if(this._columns != null)
      this._columns.Clear();
    else
      this._columns = new List<Column>();
    this._selectedColumn = null;
    this._selectedDatum = null;
    this.headerDatum = null;
    this.footerDatum = null;
    this.columnOverlayContent = null;
    this.columnOverlayLines = null;
    if(this.headerRect != null)
      Destroy(this.headerRect.gameObject);
    if(this.footerRect != null)
      Destroy(this.footerRect.gameObject);
    if(this.columnOverlay != null)
      Destroy(this.columnOverlay.gameObject);
    this.headerRect = null;
    this.footerRect = null;
    this.columnOverlay = null;
  }

  [Obsolete("'addTextColumn' method is obsolete.  Please use 'AddTextColumn' method instead.")]
  public Column addTextColumn(string header=null, string footer=null, float minWidth=-1, float maxWidth=-1) {
    return this.AddTextColumn(header, footer, minWidth, maxWidth);
  }

  public Column AddTextColumn(string header=null, string footer=null, float minWidth=-1, float maxWidth=-1) {
    return this.AddTextOrInputColumn(header, footer, minWidth, maxWidth, false);
  }

  [Obsolete("'addInputColumn' method is obsolete.  Please use 'AddInputColumn' method instead.")]
  public Column addInputColumn(Action<Datum, Column, string, string> changeCallback,
    string header=null, string footer=null, float minWidth=-1, float maxWidth=-1) {
    return this.AddInputColumn(changeCallback, header, footer, minWidth, maxWidth);
  }

  public Column AddInputColumn(Action<Datum, Column, string, string> changeCallback,
    string header=null, string footer=null, float minWidth=-1, float maxWidth=-1) {
    Column c = this.AddTextOrInputColumn(header, footer, minWidth, maxWidth, true);
    c.inputChangeCallback = changeCallback;
    return c;
  }

  private Column AddTextOrInputColumn(string header=null, string footer=null,
    float minWidth=-1, float maxWidth=-1, bool isInput=false) {
    if(this.headerDatum == null) {
      this.headerDatum = Datum.Header();
      this.headerDatum.table = this;
    }
    if(this.footerDatum == null) {
      this.footerDatum = Datum.Footer();
      this.footerDatum.table = this;
    }
    this.headerDatum.elements.Add();
    this.footerDatum.elements.Add();
    Column c = Column.TextColumn(this, this.columns.Count,
      minWidth, maxWidth, this.headerDatum, this.footerDatum, isInput);
    this.columns.Add(c);
    c.headerValue = header;
    c.footerValue = footer;
    return c;
  }

  [Obsolete("'addImageColumn' method is obsolete.  Please use 'AddImageColumn' method instead.")]
  public Column addImageColumn(string header=null, string footer=null, int imageWidth=32, int imageHeight=32) {
    return this.AddImageColumn(header, footer, imageWidth, imageHeight);
  }

  public Column AddImageColumn(string header=null, string footer=null, int imageWidth=32, int imageHeight=32) {
    if(this.headerDatum == null) {
      this.headerDatum = Datum.Header();
      this.headerDatum.table = this;
    }
    if(this.footerDatum == null) {
      this.footerDatum = Datum.Footer();
      this.footerDatum.table = this;
    }
    this.headerDatum.elements.Add();
    this.footerDatum.elements.Add();
    Column c = Column.ImageColumn(this, this.columns.Count,
      imageWidth, imageHeight, this.headerDatum, this.footerDatum);
    this.columns.Add(c);
    c.headerValue = header;
    c.footerValue = footer;
    return c;
  }

  [Obsolete("'initialize' method is obsolete.  Please use 'Initialize' method instead.")]
  public void initialize() {
    this.Initialize();
  }

  public void Initialize() {
    this.selectionCallback = null;
    this.selectionCallbackWithColumn = null;
    this.FinishInitialize();
  }

  [Obsolete("'initialize' method is obsolete.  Please use 'Initialize' method instead.")]
  public void initialize(Action<Datum, Column> selectionCallback,
                          Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
                          Action<Column, PointerEventData> headerClickCallback=null) {
    this.Initialize(selectionCallback, sprites, hasHeaderIcons, headerClickCallback);
  }

  public void Initialize(Action<Datum, Column> selectionCallback,
    Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
    Action<Column, PointerEventData> headerClickCallback=null) {
    this.selectionCallback = null;
    this.selectionCallbackWithColumn = selectionCallback;
    this.selectionCallbackWithRT = null;
    this.FinishInitialize(sprites, hasHeaderIcons, headerClickCallback);
  }

  [Obsolete("'initialize' method is obsolete.  Please use 'Initialize' method instead.")]
  public void initialize(Action<Datum> selectionCallback,
                         Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
                         Action<Column, PointerEventData> headerClickCallback=null) {
    this.Initialize(selectionCallback, sprites, hasHeaderIcons, headerClickCallback);
  }

  public void Initialize(Action<Datum> selectionCallback,
    Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
    Action<Column, PointerEventData> headerClickCallback=null) {
    this.selectionCallback = selectionCallback;
    this.selectionCallbackWithColumn = null;
    this.selectionCallbackWithRT = null;
    this.FinishInitialize(sprites, hasHeaderIcons, headerClickCallback);
  }

  [Obsolete("'initialize' method is obsolete.  Please use 'Initialize' method instead.")]
  public void initialize(Action<Datum, Column, RectTransform> selectionCallback,
                         Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
                         Action<Column, PointerEventData> headerClickCallback=null) {
    this.Initialize(selectionCallback, sprites, hasHeaderIcons, headerClickCallback);
  }

  public void Initialize(Action<Datum, Column, RectTransform> selectionCallback,
    Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
    Action<Column, PointerEventData> headerClickCallback=null) {
    this.selectionCallback = null;
    this.selectionCallbackWithColumn = null;
    this.selectionCallbackWithRT = selectionCallback;
    this.FinishInitialize(sprites, hasHeaderIcons, headerClickCallback);
  }

  private void FinishInitialize(Dictionary<string, Sprite> sprites=null, bool hasHeaderIcons=false,
    Action<Column, PointerEventData> headerClickCallback=null) {

    this._sprites = sprites;

    this._hasHeader = false;
    this._hasFooter = false;

    if(this.font == null) {
      this.font = Resources.GetBuiltinResource
        (typeof(Font), "Arial.ttf") as Font;
      //Debug.Log("FYI: Using Default Arial Font");
    }

    // gab this in a temp value that won't get updated
    if(this._minRowHeight == -1f)
      this._minRowHeight = this.minRowHeight;

    if(this.minRowHeight < 14) {
      this.error("Table Min Row Height must be >= 14 (set this as high as practical in order to reduce initialization overhead)");
    }

    if(this.defaultFontSize <= 0) {
      this.error("Table Default Font Size must be > 0");
    }

    // match our UI colors for row and cell effects if we aren't in cell-selection mode
    if(this.selectionMode == SelectionMode.ROW) {
      this.cellDownColor = this.rowDownColor;
      this.cellHoverColor = this.rowHoverColor;
      this.cellSelectColor = this.rowSelectColor;
    }

    // this handles the case on table re-use where the row height was inrecemented and needs to be reset
    this.minRowHeight = this._minRowHeight;

    if(this.rowVerticalSpacing + this.defaultFontSize > this.minRowHeight) {
      this.minRowHeight = this.rowVerticalSpacing + this.defaultFontSize;
    }

    if(this.headerTopMargin + this.headerBottomMargin + this.defaultFontSize > this.minHeaderHeight) {
      this.minHeaderHeight = this.headerTopMargin + this.headerBottomMargin + this.defaultFontSize;
    }

    if(this.footerTopMargin + this.footerBottomMargin + this.defaultFontSize > this.minFooterHeight) {
      this.minFooterHeight = this.footerTopMargin + this.footerBottomMargin + this.defaultFontSize;
    }

    if(this.loadingOverlay != null) {
      this.loadingOverlay.gameObject.SetActive(true);
      this.loadingOverlay.alpha = 1f;
      this.overlayIsHiding = false;
    }


    if(this.rows == null)
      this.rows = new List<Row>();

    for(int i = 0; i < columns.Count; i++) {
      Column c = columns[i];
      if(c.columnType == Column.ColumnType.IMAGE) {
        if(this.sprites == null)
          this.error("Cannot declare Image Column without spriteDict");
        this.minRowHeight = Mathf.Max(this.minRowHeight,
          c.imageHeight + this.rowVerticalSpacing);
      }
      if(c.headerValue != null)
        this._hasHeader = true;
      if(c.footerValue != null)
        this._hasFooter = true;
    }

    if(this.hasHeader) {
      this.minHeaderHeight = Mathf.Max(this.minHeaderHeight,
        this.headerIconHeight + this.headerTopMargin +
        this.headerBottomMargin);
    }

    this._hasHeaderIcons = hasHeaderIcons;

    if(this.columns.Count > 1)
      this._hasColumnOverlay = true;
    else
      this._hasColumnOverlay = false;

    if(this.columnOverlayLines == null)
      this.columnOverlayLines = new List<RectTransform>();

    if(this.factory == null)
      this.factory = new Factory(this);

    if(this.control == null) {
      this.control = new Control(this, this.factory);
      this.extraTextColumn = Column.TextColumn(this, -1, -1, -1,
        this.headerDatum, this.footerDatum, false);
    }

    if(this.data != null)
      this.data.Clear();
    //intentially make a new one here, dont reuse old
    this.data = new TableDatumList(this, this.control);

    this.factory.Build(this.headerDatum, this.footerDatum, this.control);

    //////////////////////////////////////////////
    // Do post factory cleanup and assigns here
    if(this.headerRow != null) {
      for(int i = 0; i < this.headerRow.cells.Count; i++) {
        HeaderCell hc = this.headerRow.cells[i] as HeaderCell;
        hc.icon.gameObject.SetActive(this.hasHeaderIcons);
        if(this.columns.Count - 1 >= i) {
          hc.Initialize(this.columns[i], headerClickCallback);
        }
      }
    }

    this.bodyRect.isMeasured = false;

    this.bodyScroller.horizontalNormalizedPosition = 0f;
    this.bodyScroller.verticalNormalizedPosition = 1f;

  }  // _initialize

  public void SetGameObjectActiveLater(GameObject go, bool state) {
    if((go.activeInHierarchy && !state) || (!go.activeInHierarchy && state))
      StartCoroutine(this.DoSetGameObjectActiveLater(go, state));
  }

  IEnumerator DoSetGameObjectActiveLater(GameObject go, bool state) {
    yield return new WaitForEndOfFrame();
    go.SetActive(state);
  }

  public void DirtyNow() {
    //this.lastMarkForRebuild = Time.realtimeSinceStartup;
    LayoutRebuilder.MarkLayoutForRebuild(this.root);
  }

  public void DirtyLater() {
    //this.lastMarkForRebuild = Time.realtimeSinceStartup;
    if(!this.doingDirtyLater && this.gameObject.activeInHierarchy)
      StartCoroutine(this.DoDirtyLater());
  }

  IEnumerator DoDirtyLater() {
    this.doingDirtyLater = true;
    yield return new WaitForEndOfFrame();
    this.doingDirtyLater = false;
    LayoutRebuilder.MarkLayoutForRebuild(this.root);
  }

  private bool overlayIsHiding;

  public void FadeOverlay(float overTime, float v0, float v1, float delay=0) {
    if(this.overlayIsHiding ||
      !this.loadingOverlay.gameObject.activeInHierarchy)
      return;
    this.overlayIsHiding = true;
    StartCoroutine(this.DofadeOverlay(overTime, v0, v1, delay));
  }

  IEnumerator DofadeOverlay(float overTime, float v0, float v1, float delay) {
    yield return new WaitForSeconds(delay);
    float startTime = Time.time;
    while(Time.time < startTime + overTime) {
      this.loadingOverlay.alpha =
        Mathf.Lerp(v0, v1, (Time.time - startTime) / overTime);
      yield return null;
    }
    this.loadingOverlay.gameObject.SetActive(false);
  }

  [Obsolete("'startRenderEngine' method is obsolete.  Please use 'StartRenderEngine' method instead.")]
  public void startRenderEngine() {
    this.StartRenderEngine();
  }

  public void StartRenderEngine() {
    if(this.hasError)
      return;
    this._isRunning = true;
    if(this.hasHeader) {
      this.headerRow.datum = this.headerDatum;
      this.headerDatum.isDirty = true;
    }
    if(this.hasFooter) {
      this.footerRow.datum = this.footerDatum;
      this.footerDatum.isDirty = true;
    }
    this.control.Draw();
    this.lastRootSize = this.root.rect.size;
  }

  public float SumColumnWidths() {
    float wsum = 0;
    for(int i = 0; i < this.columns.Count; i++) {
      //print("sum col: " + i + " width: " + this.columnList[i].rawWidth);
      wsum += this.columns[i].safeWidth;
    }
    return wsum;
  }

  public void SetBodyRectSizeLater(float s1, float s2) {
    StopCoroutine("_setBodyRectSizeLater");
    StartCoroutine(this.DoSetBodyRectSizeLater(s1, s2));
  }

  IEnumerator DoSetBodyRectSizeLater(float s1, float s2) {
    yield return new WaitForEndOfFrame();
    this.bodyRect.rt.offsetMin = new Vector2(0, s1);
    this.bodyRect.rt.offsetMax = new Vector2(0, s2 * -1f);
  }

  public void SetLayoutVertical() {
    if(this.bodyRect == null || !this.isRunning)
      return;
    this.control.SetLayoutVertical();
  }

  public void SetLayoutHorizontal() {
    if(this.bodyRect == null || !this.isRunning)
      return;
    this.control.SetLayoutHorizontal();
  }

  override protected void OnRectTransformDimensionsChange() {
    base.OnRectTransformDimensionsChange();
    if(this.root != null && this.lastRootSize != this.root.rect.size) {
      this.RedrawTable();
      this.lastRootSize = this.root.rect.size;
    }
  }

  private void RedrawTable() {
    if(this.isActiveAndEnabled && this.control != null) {
      this.loadingOverlay.gameObject.SetActive(true);
      this.loadingOverlay.alpha = 1f;
      this.overlayIsHiding = false;
      for(int i = 0; i < this.columns.Count; i++)
        this.columns[i].ClearMeasure();
      this.bodyScroller.horizontalNormalizedPosition = 0f;
      this.bodyScroller.verticalNormalizedPosition = 1f;
      this.control.SizeForRectTransform();
      this.data.ClearMeasured();
      this.data.ClearSafeHeightSum();
      for(int i = 0; i < this.data.Count; i++)
        this.data[i].isDirty = true;
      StopCoroutine("startRenderLater");
      StartCoroutine(this.DoStartRenderLater());
    }
  }

  IEnumerator DoStartRenderLater() {
    yield return new WaitForEndOfFrame();
    this.StartRenderEngine();
  }

  public Row GetRowForDatum(Datum item) {
    if(item == null)
      return null;
    if(item.isHeader)
      return this.headerRow;
    if(item.isFooter)
      return this.footerRow;
    for(int i = 0; i < this.rows.Count; i++) {
      if(this.rows[i].datum == item)
        return this.rows[i];
    }
    return null;
  }

  void OnDrawGizmosSelected() {
    if(!this.drawGizmos)
      return;
    UnityEngine.Canvas c = this.GetComponentInParent<UnityEngine.Canvas>();
    if(c != null && c.renderMode == RenderMode.ScreenSpaceOverlay) {
      RectTransform rt = this.GetComponent<RectTransform>();
      Vector2 center = rt.position;
      Gizmos.color = this.gizmoColor;
      Gizmos.DrawCube(center, rt.rect.size);
    }
  }

}

}