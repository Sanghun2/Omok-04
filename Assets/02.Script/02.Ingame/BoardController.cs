using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(Collider2D))]
public class BoardController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Cell[,] board;
    private Cell currentCell;

    [SerializeField] private GameObject positionSelector;
    [SerializeField] private GameObject blackStone;
    [SerializeField] private GameObject whiteStone;
    [SerializeField] private GameObject xMarker;
    [SerializeField] private GameObject lastPositionMarker;
    [SerializeField] private Define.Type.Game gameType;
    [SerializeField] private Button launchButton;
    [SerializeField] private Transform stones;

    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked onCellClickedDelegate;
    public delegate void OnStoneSetted(Define.Type.StoneColor stoneType);
    public OnStoneSetted onStoneSettedDelegate;
    public Cell[,] Board => board;

    private void OnDisable()
    {
        launchButton?.onClick.RemoveListener(OnClickLaunchButton);
    }

    /// <summary>
    /// 보드 초기화
    /// </summary>
    public void InitBoard()
    {
        this.gameObject.SetActive(true);
        launchButton.onClick.AddListener(OnClickLaunchButton);

        // 마커 초기화
        xMarker.SetActive(false);
        lastPositionMarker.SetActive(false);

        board = new Cell[Define.Value.BoardRow, Define.Value.BoardCol];

        onStoneSettedDelegate = (stoneType) =>
        {
            Debug.Log("### DEV_JSH MarkerEvent Start ###");
            Debug.Log($"### DEV_JSH 이번에 놓인 돌은 {stoneType.ToString()}");
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

        // 돌이 있을 경우 모두 삭제
        for (int i = stones.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = stones.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        positionSelector.SetActive(true);
        xMarker.SetActive(false);

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

        Define.Type.StoneColor sc = Managers.Turn.GetCurrentPlayer() == Define.Type.Player.Player1 ?
            Define.Type.StoneColor.Black : Define.Type.StoneColor.White;

        if(OmokAI.CheckRenju(sc, board, currentCell.CellRow, currentCell.CellCol))
        {
            positionSelector.SetActive(false);
            ActiveX_Marker(currentCell.CellRow, currentCell.CellCol);
        }
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

        Define.Type.StoneColor sc = Managers.Turn.GetCurrentPlayer() == Define.Type.Player.Player1 ?
            Define.Type.StoneColor.Black : Define.Type.StoneColor.White;

        if (OmokAI.CheckRenju(sc, board, currentCell.CellRow, currentCell.CellCol))
        {
            positionSelector.SetActive(false);
            ActiveX_Marker(currentCell.CellRow, currentCell.CellCol);
        }
    }

    public void OnClickLaunchButton()
    {
        if (currentCell == null)
            return;

        positionSelector.SetActive(false);

        currentCell.onCellClicked?.Invoke(currentCell.CellRow,currentCell.CellCol);
    }

    #region 자동 호출 메서드 / 금지 표시, 돌 생성, 셀 초기화
    public void ActiveX_Marker(int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        xMarker.SetActive(true);
        xMarker.transform.position = markerPos;
    }

    public void PlaceMarker(Define.Type.StoneColor stoneColor, int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        // 돌 생성
        GameObject stoneObj = stoneColor == Define.Type.StoneColor.Black ? Instantiate(blackStone, stones) : Instantiate(whiteStone, stones);
        stoneObj.transform.position = markerPos;

        lastPositionMarker.SetActive(true);
        lastPositionMarker.transform.position = markerPos;
    }

    public void ResetCurretCell()
    {
        currentCell= null;
    }
    #endregion
}
