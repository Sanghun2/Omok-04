using UnityEngine;

public class Cell
{
    private int cellRow;
    private int cellCol;
    private StoneType stoneType;

    public int CellRow => cellRow;
    public int CellCol => cellCol;

    public enum StoneType { None, Black, White }
    public StoneType Stone => stoneType;
    public delegate void OnCellClicked(int row, int col);  
    public OnCellClicked onCellClicked;

    /// <summary>
    /// 셀 초기화
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="onCellClicked"></param>
    public void InitCell(int row, int col,OnCellClicked onCellClicked)
    {
        cellRow = row; 
        cellCol = col;
        SetMarker(StoneType.None);
        this.onCellClicked = onCellClicked;
    }

    /// <summary>
    /// 마커 변경
    /// </summary>
    /// <param name="marker"></param>
    public void SetMarker(StoneType marker)
    {
        this.stoneType = marker;
    }
}
