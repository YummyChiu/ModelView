using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace SLS.Widgets.Table {
public class Row : MonoBehaviour, IPointerEnterHandler,
  IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,
  IPointerClickHandler {

  private RectTransform _rt;

  public RectTransform rt { get { return this._rt; } }

  private RectTransform _cgrt;

  public RectTransform cgrt { get { return this._cgrt; } }

  private CanvasGroup _cg;
  //public CanvasGroup cg { get { return this._cg; } }
  public Row preceedingRow;

  //public ScrollRect extraTextScroller;
  public RectTransform extraTextRt;
  public Image extraTextBackground;
  public Text extraText;
  public List<Cell> cells;
  private Table table;
  private Image background;
  private MeasureMaster mm;
  private bool isHeader;
  private bool isFooter;
  public bool isOver;
  public bool isDown;

  // reset UI state if table is deactivated and reactivated
  private void OnEnable() {
    if(this.table == null)
      return;
    this.isOver = false;
    this.isDown = false;
    this.SetColor();
  }

  public bool Initialize(Table table, RectTransform rt, RectTransform cgrt,
                         CanvasGroup cg, Image background, MeasureMaster mm,
                         bool isHeader=false, bool isFooter=false) {
    this._cgrt = cgrt;
    this._cg = cg;
    this.table = table;
    this.background = background;
    if(!isHeader && !isFooter)
      this.table.rows.Add(this);
    this.isHeader = isHeader;
    this.isFooter = isFooter;
    this.cells = new List<Cell>();
    this._rt = rt;
    this.mm = mm;
    return true;
  }

  private Datum _datum;

  public Datum datum {
    set {
      bool noChange = (this._datum == value);
      this._datum = value;
      if(!this.table.isRunning)
        return;
      if(!this.isHeader && !this.isFooter)
        this.SetColor();
      if(!noChange || value.isDirty)
        this.Refresh();
    }
    get {
      return this._datum;
    }
  }

  public void Refresh() {
    //Debug.Log("in refresh: " + this.datum.uid + " Dirty: " + this.datum.isDirty);
    for(int i = 0; i < this._datum.elements.Count; i++) {
      //Debug.Log(i + " vs " + this.cells.Count + " vs " + this._datum.elements.Count + this.datum.uid + " elem: " + this._datum.elements[i].value);
      this.cells[i].element = this._datum.elements[i];
      if(this.datum.isHeader) {
        (this.cells[i] as HeaderCell).UpdateDatum();
      }
    }
    if(this.extraText != null) {
      if(this._datum.extraText != null && !string.IsNullOrEmpty(this._datum.extraText.value)) {
        this.extraText.text = this._datum.extraText.value;
        this.extraTextBackground.gameObject.SetActive(true);
        if(this.datum.extraTextBoxColor.HasValue)
          this.extraTextBackground.color =
            this.datum.extraTextBoxColor.Value;
        else
          this.extraTextBackground.color =
            this.table.extraTextBoxColor;
        if(this.datum.extraTextColor.HasValue)
          this.extraText.color = this.datum.extraTextColor.Value;
        else
          this.extraText.color = this.table.extraTextColor;
      }
      else
        this.extraTextBackground.gameObject.SetActive(false);
    } // if this.extraText != null

    if(this.datum.isDirty || this.datum.SafeHeight() < 0) {
      this.mm.DoMeasure(this.datum);
    }

    if(this.datum.animationStartTime > 0.0f) {
      float t = Time.realtimeSinceStartup - this.datum.animationStartTime;
      if(t < this.table.rowAnimationDuration) {
        StopCoroutine("dofadeCG");
        float currentAlpha = t / this.table.rowAnimationDuration;
        if(this.gameObject.activeInHierarchy)
          StartCoroutine(this.DofadeCG(this.table.rowAnimationDuration, currentAlpha, 1.0f));
        else
          this._cg.alpha = 1.0f;
      }
    }
    else {
      this._cg.alpha = 1f;
    }

    this.table.DirtyNow();
  }

  public void SetColor() {
    if(this.datum != null && this.datum.isFooter) {
      this.background.color = this.table.footerBackgroundColor;
      return;
    }
    if(this.table.bodyScrollWatcher.isDragging) {
      if(this.datum != null && this.datum == this.table.selectedDatum)
        this.background.color = this.table.rowSelectColor;
      else
        this.background.color = this.table.rowNormalColor;
    }
    else if(this.isOver) {
      if(this.isDown)
        this.background.color = this.table.rowDownColor;
      else
        this.background.color = this.table.rowHoverColor;
    }
    else if(this.datum != null && this.datum == this.table.selectedDatum)
      this.background.color = this.table.rowSelectColor;
    else
      this.background.color = this.table.rowNormalColor;
    this.ColorCells();
  }

  public void ColorCells() {
    for(int i = 0; i < this.cells.Count; i++) {
      this.cells[i].SetColor();
    }
  }

  IEnumerator DofadeCG(float overTime, float v0, float v1) {
    //Debug.Log("Animating: " + this.datum.uid + " Over " + overTime + " Starting at: " + v0);
    float startTime = Time.time;
    while(Time.time < startTime + overTime) {
      this._cg.alpha =
        Mathf.Lerp(v0, v1, (Time.time - startTime) / overTime);
      yield return null;
    }
  }

  public void OnPointerEnter(PointerEventData data) {
    this.isOver = true;
    if(this.isHeader || this.isFooter)
      return;
    this.SetColor();
    if(this.table.tooltipHandler != null && this._datum != null &&
      !string.IsNullOrEmpty(this._datum.tooltip))
      this.table.tooltipHandler(this.rt, this._datum.tooltip);
  }

  public void OnPointerExit(PointerEventData data) {
    this.isOver = false;
    if(this.isHeader || this.isFooter)
      return;
    this.SetColor();
  }

  public void OnPointerDown(PointerEventData data) {
    this.isDown = true;
    if(this.isHeader || this.isFooter)
      return;
    if(this.table.pointerDownHandler != null)
      this.table.pointerDownHandler(data, this.datum);
    this.SetColor();
  }

  public void OnPointerUp(PointerEventData data) {
    this.isDown = false;
    if(this.isHeader || this.isFooter)
      return;
    if(this.table.pointerUpHandler != null)
      this.table.pointerUpHandler(data, this.datum);
    this.SetColor();
  }

  public void OnPointerClick(PointerEventData data) {
    if(this.isHeader || this.isFooter || this.datum == null)
      return;
    this.table.selectedDatum = this.datum;
    this.SetColor();
  }


}
}
