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

    public const int BoardRow = 15;
    public const int BoardCol = 15;
    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked onCellClickedDelegate;
    public delegate void OnMarkerSetted(Cell.StoneType stoneType);
    public OnMarkerSetted onMarkerSettedDelegate;
    public Cell[,] Board => board;

    private void Start()
    {
        InitBoard();
        GameLogic gameLogic = new GameLogic(this, board, gameType);
        AssignLaunchRole();
    }

    public void AssignLaunchRole()
    {
        launchButton.onClick.AddListener(OnClickLaunchButton);
    }

    public void DepiveLaunchRole()
    {
        launchButton?.onClick.RemoveListener(OnClickLaunchButton);
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void InitBoard()
    {
        board = new Cell[BoardRow, BoardCol];

        onMarkerSettedDelegate = (stoneType) =>
        {
            Debug.Log("### DEV_JSH MarkerEvent Start ###");
            Debug.Log($"### DEV_JSH �̹��� ���� ���� {stoneType.ToString()}");
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
        // ��ũ�� ���� -> ���� ���� ��ȯ
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // ������ ������ ����� ���� ���
        if (Mathf.Round(worldPosition.x / 0.45f) > 7 || Mathf.Round(worldPosition.x / 0.45f) < -7 ||
            Mathf.Round(worldPosition.y / 0.45f) > 7 || Mathf.Round(worldPosition.y / 0.45f) < -7)
        {
            currentCell = null;
            return;
        }

        // �������� ��ġ ����
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // ���� �������� ��ġ�� �������� �� ����
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        positionSelector.SetActive(true);
        xMarker.SetActive(false) ;

        // ��ũ�� ���� -> ���� ���� ��ȯ
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // �������� ��ġ ����
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // ���� �������� ��ġ�� �������� �� ����
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }

    public void OnClickLaunchButton()
    {
        if (currentCell == null)
            return;

        positionSelector.SetActive(false);

        currentCell.onCellClicked?.Invoke(currentCell.CellRow,currentCell.CellCol);
    }

    #region �ڵ� ȣ�� �޼��� / ���� ǥ��, �� ����
    public void ActiveX_Marker(int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        xMarker.SetActive(true);
        xMarker.transform.position = markerPos;
        currentCell = null;
    }

    public void PlaceMarker(Cell.StoneType stoneType, int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        // �� ����
        GameObject stoneObj = stoneType == Cell.StoneType.Black ? Instantiate(blackStone, transform) : Instantiate(whiteStone, transform);
        stoneObj.transform.position = markerPos;

        lastPositionMarker.SetActive(true);
        lastPositionMarker.transform.position = markerPos;
    }
    #endregion
}
