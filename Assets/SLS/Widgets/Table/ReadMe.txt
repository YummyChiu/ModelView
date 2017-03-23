Thanks for using Table Pro (v1.13)!

Release Notes 1.14 (2016.10.19):
  BUG: Fixed issue with table redrawing in draggable parent rect
  BUG: Fixed issue applying table formatting after redraw
  ENHANCEMENT: Code Formatting Updates
  
Release Notes 1.13 (2016.10.13):
  ENHANCEMENT: Multiple bug fixes and enhancements per awesome user feedback!

Release Notes 1.12 (2016.09.19):
  BUG: Corrected cell coloring issue when deactivating and activating a table
  ENHANCEMENT: Will now auto-scroll when using the MoveSelectionUp/MoveSelectionDown methods
  ENHANCEMENT: Added new "General Settings" called "Table Selection UI Mode"
               this can be used to disable cell-level UI highlighting on interaction

Release Notes 1.11 (2016.08.18):
  Refactored all methods to make them UpperCase.  Tried to wrap the core methods with an 
  "obsolete" warning message but could potentially throw errors if more obscure methods 
  were previously being used in external implementations.  Simply change the method call
  to UpperCase and it'll work.
  

Simple instructions to get started:

  1. Create an 'empty' UI element and anchor it however you wish.
  2. Add our Table script to your empty UI element.
  3. In one of your scripts, just access and work with the Table
     component something like the simple example below.

Please see our other samples in the included Samples directory or visit
  http://semi-legitimate.com/tablepro/ for additional documentation!

Sample Usage:

using UnityEngine;
using SLS.Widgets.Table;

public class Simple : MonoBehaviour {

  private Table table;

  void Start () {

    this.table = this.GetComponent<Table>();

    this.table.ResetTable();

    this.table.AddTextColumn();
    this.table.AddTextColumn();
    this.table.AddTextColumn();

    // Initialize Your Table
    this.table.Initialize(this.OnTableSelected);

    // Populate Your Rows (obviously this would be real data here)
    for (int i = 0; i < 100; i++) {
      Datum d = Datum.Body(i.ToString());
      d.elements.Add("Col1:Row" + i.ToString());
      d.elements.Add("Col2:Row" + i.ToString());
      d.elements.Add("Col3:Row" + i.ToString());
      this.table.data.Add(d);
    }

    // Draw Your Table
    this.table.StartRenderEngine();

  }

  // Handle the row selection however you wish (be careful as column can be null here
  //  if your table doesn't fill the full horizontal rect space and the user clicks an edge)
  private void OnTableSelected(Datum datum, Column column) {
    string cidx = "N/A";
    if (column != null) cidx = column.idx.ToString();
    print("You Clicked: " + datum.uid + " Column: " + cidx);
  }

}