using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[RequireComponent (typeof(Collider2D))]
public class BoardController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Cell[,] board;
    private Cell currentCell;

    [SerializeField] private GameObject positionSelector;
    [SerializeField] private GameObject blackStone;
    [SerializeField] private GameObject whiteStone;
    [SerializeField] private Transform stones;
    [SerializeField] private GameObject xMarker;
    [SerializeField] private GameObject x_MarkerPrefab;
    [SerializeField] private Transform markers;
    [SerializeField] private GameObject lastPositionMarker;
    [SerializeField] private Button blackStoneLaunchButton;
    [SerializeField] private Button whiteStoneLaunchButton;

    public delegate void OnCellClicked(int row, int col);
    public OnCellClicked OnStonePlace;
    public delegate void OnStoneSetted(Define.Type.Player playerType, Define.Type.StoneColor stoneType, int row, int col);
    public OnStoneSetted OnStonePlaceSuccess;
    public Cell[,] Board => board;

    private void OnDisable()
    {
        DeactiveLaunchButton();
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void InitBoard()
    {
        this.gameObject.SetActive(true);

        //blackStoneLaunchButton.onClick.AddListener(OnClickBlackStoneLaunchButton);
        //whiteStoneLaunchButton.onClick.AddListener(OnClickWhiteStoneLaunchButton);

        // ��Ŀ �ʱ�ȭ
        xMarker.SetActive(false);
        lastPositionMarker.SetActive(false);

        board = new Cell[Define.Value.BoardRow, Define.Value.BoardCol];

        OnStonePlaceSuccess += (playerType, stoneType, row, col) =>
        {
            SoundManager.Instance.OnAttackSound();
            Debug.Log("### DEV_JSH MarkerEvent Start ###");
            Debug.Log($"### DEV_JSH ��� ���� �÷��̾�� {playerType.ToString()}");
            Debug.Log($"### DEV_JSH ��� ���� ���� {stoneType.ToString()}");
            Debug.Log($"### DEV_JSH ��� ���� ���� ��ġ�� Row : {row} / Col : {col}");
            Debug.Log("### DEV_JSH MarkerEvent End ###");
        };

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Cell();

                board[i,j].InitCell(i, j, (i,j) => {
                    OnStonePlace(i, j);
                });
            }
        }

        // ���� ���� ��� ��� ����
        for (int i = stones.transform.childCount - 1; i >= 0; i--)
        {
            Transform childStone = stones.transform.GetChild(i);
            Destroy(childStone.gameObject);
        }

        // X ��Ŀ�� ���� ��� ��� ����
        for (int i = markers.transform.childCount - 1; i >= 0; i--)
        {
            Transform childMarker = markers.transform.GetChild(i);
            Destroy(childMarker.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        positionSelector.SetActive(true);
        xMarker.SetActive(false);

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

    public void OnClickBlackStoneLaunchButton()
    {
        if (currentCell == null || Managers.Turn.GetCurrentPlayer() != Define.Type.Player.Player1)
            return;

        positionSelector.SetActive(false);
        Debug.LogAssertion($"Black ����");

        currentCell.onCellClicked?.Invoke(currentCell.CellRow,currentCell.CellCol);
    }

    public void OnClickWhiteStoneLaunchButton()
    {
        if (currentCell == null || Managers.Turn.GetCurrentPlayer() != Define.Type.Player.Player2)
            return;

        positionSelector.SetActive(false);

        Debug.LogAssertion($"White ����");
        currentCell.onCellClicked?.Invoke(currentCell.CellRow, currentCell.CellCol);
    }
    
    #region �ڵ� ȣ�� �޼��� / ���� ǥ��, �� ����, �� �ʱ�ȭ
    public void ShowAllRenju(Cell[,] board)
    {
        foreach (var cell in board)
        {
            if (cell.IsRenju && !cell.OnX_Marker)
            {
                cell.OnX_Marker = true;
                GameObject x_MarkerObj = Instantiate(x_MarkerPrefab, markers);
                x_MarkerObj.transform.position = new Vector3((cell.CellRow - 7) * 0.45f, (cell.CellCol - 7) * 0.45f, 0);
            }
            else if (!cell.IsRenju && cell.OnX_Marker)
            {
                cell.OnX_Marker = false;
                DestroyX_Marker(cell.CellRow, cell.CellCol);
            }
        }
    }

    public void DeactiveLaunchButton()
    {
        blackStoneLaunchButton?.onClick.RemoveListener(OnClickBlackStoneLaunchButton);
        whiteStoneLaunchButton?.onClick.RemoveListener(OnClickWhiteStoneLaunchButton);
    }

    public void DestroyX_Marker(int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        foreach (Transform x_marker in markers)
        {
            if(x_marker.position == markerPos)
            {
                Destroy(x_marker.gameObject);
            }
        }
    }

    public void ActiveX_Marker(int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        xMarker.SetActive(true);
        xMarker.transform.position = markerPos;
    }

    public void PlaceMarker(Define.Type.StoneColor stoneColor, int row, int col)
    {
        Vector3 markerPos = new Vector3((row - 7) * 0.45f, (col - 7) * 0.45f, 0);

        // �� ����
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
