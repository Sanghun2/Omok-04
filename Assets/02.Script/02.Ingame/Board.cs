using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Collider2D))]
public class Board : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Cell[,] board;
    private Cell currentCell;

    [SerializeField] private GameObject positionSelector;
    [SerializeField] private GameObject blackMarker;
    [SerializeField] private GameObject whiteMarker;

    public const int BoardRow = 15;
    public const int BoardCol = 15;
    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked onCellClickedDelegate;

    private void Start()
    {
        InitBoard();
    }

    // 보드 초기화
    public void InitBoard()
    {
        board = new Cell[BoardRow, BoardCol];

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Cell();

                board[i,j].InitCell(i, j, (i,j) =>
                {
                    onCellClickedDelegate(i, j);
                });
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 스크린 지점 -> 월드 지점 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 보드판 범위 내에서만 움직이도록 조건
        if (Mathf.Round(worldPosition.x / 0.45f) > 7 || Mathf.Round(worldPosition.x / 0.45f) < -7 ||
            Mathf.Round(worldPosition.y / 0.45f) > 7 || Mathf.Round(worldPosition.y / 0.45f) < -7)
            return;

        // 셀렉터의 위치 변경
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // 현재 셀렉터의 위치를 기준으로 셀 선택
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        positionSelector.SetActive(false);

        // 이미 돌이 놓여져있다면 취소
        if (currentCell.Marker != Cell.CellMarker.None)
        {
            currentCell = null;
            return;
        }

        // 돌 생성
        GameObject blackMarkerObj = Instantiate(blackMarker,transform);

        // 선택된 셀의 row와 col을 기준으로 위치 지정
        Vector3 markerPos = new Vector3((currentCell.CellRow - 7) * 0.45f, (currentCell.CellCol - 7) * 0.45f, 0);
        blackMarkerObj.transform.position = markerPos;

        // 셀의 마커 변경
        currentCell.SetMarker(Cell.CellMarker.Black);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        positionSelector.SetActive(true);

        // 스크린 지점 -> 월드 지점 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 셀렉터의 위치 변경
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // 현재 셀렉터의 위치를 기준으로 셀 선택
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }
}
