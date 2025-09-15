using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Collider2D))]
public class Board : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Cell[,] board;
    private Cell currentCell;
    private Cell.CellMarker marker;

    [SerializeField] private GameObject positionSelector;
    [SerializeField] private GameObject blackMarker;
    [SerializeField] private GameObject whiteMarker;
    [SerializeField] private GameObject xMarker;
    [SerializeField] private GameObject lastPositionMarker;

    public const int BoardRow = 15;
    public const int BoardCol = 15;
    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked onCellClickedDelegate;

    private void Start()
    {
        InitBoard();
    }


    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void InitBoard()
    {
        board = new Cell[BoardRow, BoardCol];
        marker = Cell.CellMarker.Black;

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

        // ������ ���� �������� �����̵��� ����
        if (Mathf.Round(worldPosition.x / 0.45f) > 7 || Mathf.Round(worldPosition.x / 0.45f) < -7 ||
            Mathf.Round(worldPosition.y / 0.45f) > 7 || Mathf.Round(worldPosition.y / 0.45f) < -7)
            return;

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

        // ���õ� ���� row�� col�� �������� ��ġ ����
        Vector3 markerPos = new Vector3((currentCell.CellRow - 7) * 0.45f, (currentCell.CellCol - 7) * 0.45f, 0);

        // �̹� ���� �������ִٸ� ���
        if (currentCell.Marker != Cell.CellMarker.None)
        {
            xMarker.SetActive(true);
            xMarker.transform.position = markerPos; 
            currentCell = null;
            return;
        }

        if (marker == Cell.CellMarker.Black) {
            if (OmokAI.CheckRenju(Cell.CellMarker.Black, board, currentCell.CellRow, currentCell.CellCol))
            {
                xMarker.SetActive(true);
                xMarker.transform.position = markerPos;
                return;
            }            
        }



        // �� ����
        GameObject markerObj = marker == Cell.CellMarker.Black ? Instantiate(blackMarker, transform) : Instantiate(whiteMarker, transform);
        markerObj.transform.position = markerPos;

        currentCell.SetMarker(marker);

        lastPositionMarker.SetActive(true);
        lastPositionMarker.transform.position = markerPos;

        if (OmokAI.CheckGameWin(currentCell.Marker, board, currentCell.CellRow, currentCell.CellCol))
            Debug.Log($"{marker.ToString()} / ### GAME WIN ###");

        marker = marker == Cell.CellMarker.Black ? Cell.CellMarker.White : Cell.CellMarker.Black;

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
}
