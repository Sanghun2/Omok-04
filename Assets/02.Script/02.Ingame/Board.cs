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

    // ���� �ʱ�ȭ
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

    public void OnPointerUp(PointerEventData eventData)
    {
        positionSelector.SetActive(false);

        // �̹� ���� �������ִٸ� ���
        if (currentCell.Marker != Cell.CellMarker.None)
        {
            currentCell = null;
            return;
        }

        // �� ����
        GameObject blackMarkerObj = Instantiate(blackMarker,transform);

        // ���õ� ���� row�� col�� �������� ��ġ ����
        Vector3 markerPos = new Vector3((currentCell.CellRow - 7) * 0.45f, (currentCell.CellCol - 7) * 0.45f, 0);
        blackMarkerObj.transform.position = markerPos;

        // ���� ��Ŀ ����
        currentCell.SetMarker(Cell.CellMarker.Black);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        positionSelector.SetActive(true);

        // ��ũ�� ���� -> ���� ���� ��ȯ
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

        // �������� ��ġ ����
        Vector3 selectorPosition = new Vector3(Mathf.Round(worldPosition.x / 0.45f) * 0.45f, Mathf.Round(worldPosition.y / 0.45f) * 0.45f, 0);
        positionSelector.transform.position = selectorPosition;

        // ���� �������� ��ġ�� �������� �� ����
        currentCell = board[(int)Mathf.Round(worldPosition.x / 0.45f) +7, (int)Mathf.Round(worldPosition.y / 0.45f) + 7];
    }
}
