using UnityEngine;

public class Cell
{
    private int cellRow;
    private int cellCol;
    private Define.Type.StoneColor stoneColor;
    private bool isRenju = false;
    private bool onMarker = false;

    public int CellRow => cellRow;
    public int CellCol => cellCol;

    public Define.Type.StoneColor Stone => stoneColor;
    public bool IsRenju
    {
        get { return isRenju; }
        set { isRenju = value; }
    }
    public bool OnX_Marker
    {
        get { return onMarker; }
        set { onMarker = value; }
    }
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
        SetMarker(Define.Type.StoneColor.None);
        this.onCellClicked = onCellClicked;
    }

    /// <summary>
    /// 마커 변경
    /// </summary>
    /// <param name="stoneColor"></param>
    public void SetMarker(Define.Type.StoneColor stoneColor)
    {
        this.stoneColor = stoneColor;
    }
}
