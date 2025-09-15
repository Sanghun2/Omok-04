using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Collider2D))]
public class BoardController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Cell[,] board;
    private Cell currentCell;
    private Cell.CellMarkerType marker;

    [SerializeField] private GameObject positionSelector;
    [SerializeField] private GameObject blackMarker;
    [SerializeField] private GameObject whiteMarker;
    [SerializeField] private GameObject xMarker;
    [SerializeField] private GameObject lastPositionMarker;

    public const int BoardRow = 15;
    public const int BoardCol = 15;
    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked onCellClickedDelegate;
    public delegate void OnMarkerSetted(Cell.CellMarkerType markerType);
    public OnMarkerSetted onMarkerSettedDelegate;

    private void Start()
    {
        InitBoard();
        GameLogic gameLogic = new GameLogic(this, board, Define.Type.Game.Local);
    }

    /// <summary>
    /// 보드 초기화
    /// </summary>
    public void InitBoard()
    {
        board = new Cell[BoardRow, BoardCol];
        marker = Cell.CellMarkerType.Black;

        onMarkerSettedDelegate = (marker) =>
        {
            Debug.Log("### DEV_JSH MarkerEvent Start ###");
            Debug.Log($"### DEV_JSH 이번에 놓인 돌은 {marker.ToString()}");
            Debug.Log("### DEV_JSH MarkerEvent End ###");
        };

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

        // 보드판 범위를 벗어나면 선택 취소
        if (Mathf.Round(worldPosition.x / 0.45f) > 7 || Mathf.Round(worldPosition.x / 0.45f) < -7 ||
            Mathf.Round(worldPosition.y / 0.45f) > 7 || Mathf.Round(worldPosition.y / 0.45f) < -7)
        {
            currentCell = null;
            return;
        }

        // 셀렉터의 위치 변경
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // 현재 셀렉터의 위치를 기준으로 셀 선택
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        positionSelector.SetActive(true);
        xMarker.SetActive(false) ;

        // 스크린 지점 -> 월드 지점 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // 셀렉터의 위치 변경
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // 현재 셀렉터의 위치를 기준으로 셀 선택
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }

    public void ActiveX_Marker(int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        xMarker.SetActive(true);
        xMarker.transform.position = markerPos;
        currentCell = null;
    }

    public void OnClickLaunchButton()
    {
        if (currentCell == null)
            return;

        positionSelector.SetActive(false);

        currentCell.onCellClicked?.Invoke(currentCell.CellRow,currentCell.CellCol);
    }

    public void PlaceMarker(Cell.CellMarkerType marker, int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        // 돌 생성
        GameObject markerObj = marker == Cell.CellMarkerType.Black ? Instantiate(blackMarker, transform) : Instantiate(whiteMarker, transform);
        markerObj.transform.position = markerPos;

        lastPositionMarker.SetActive(true);
        lastPositionMarker.transform.position = markerPos;
    }
}
