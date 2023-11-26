using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GridValueMenu
{
    WallOnPillar = 0,
    Wall = 1,
    Caro = 2,
    Brick = 3,
    Unbrick = 4,
    StartPoint = 5,
    Chest = 6,
}

public enum Direct
{
    None,
    Foward,
    Back,
    Left,
    Right
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    public GameObject victoryPanel; // Panel chiến thắng
    public GameObject startPanel;

    private string rootFolder;
    private string readMap;
    private string[] arrRow;
    [SerializeField] private List<TextAsset> listLevel = new List<TextAsset>();

    [SerializeField] private int level = 0;
    private int colNumber, rowNumber;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject finish;
    [SerializeField] private List<GameObject> listPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> listBrick = new List<GameObject>();
    [SerializeField] private List<GameObject> listUnBrick = new List<GameObject>();

    [SerializeField] private Grid checkGridPlayer;
    private List<Grid> listGrid = new List<Grid>();

    [SerializeField] private Direct directPlayer;

    [SerializeField] private bool isMoving = false;
    public Direct DirectPlayer { get => directPlayer; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        finish = new GameObject();
        OnInit();
    }

    void Update()
    {
        checkGridPlayer = Vector3ToGridPos(player.transform.position);

        Brick.Instance.AddBrick(listBrick, checkGridPlayer);
        if (listBrick.Count > 0)
        {
            Brick.Instance.RemoveBrick(listUnBrick, checkGridPlayer);
        }
        if (Brick.Instance.isFinish(finish, checkGridPlayer))
        {
            victoryPanel.SetActive(true);
            return;
        }

        if (!isMoving)
        {
            GetDirect();
            CheckWallManager();
        }

        if (listGrid.Count == 0)
        {
            directPlayer = Direct.None;
            isMoving = false;
            return;
        }

        if (directPlayer != Direct.None)
        {
            MoveToTarget();
        }

       
    }

    private void OnInit()
    {
        directPlayer = Direct.None;
        //listLevel = new List<string> { "Level1.txt", "Level2.txt" };
        //rootFolder = @"\Unity2D\StackMaker_Nangtm\Assets\Map\";
        CustomMap();
    }

    private string[] ReadFile()
    {
        //string map = Path.Combine(rootFolder, listLevel[level - 1]);
        //readMap = File.ReadAllText(map);
        string curentMap = listLevel[level].name.ToString();
        TextAsset binData = Resources.Load(curentMap) as TextAsset;
        string data = binData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    private void CustomMap()
    {
        //ReadFile();
        //arrRow = readMap.Split('\n');

        arrRow = ReadFile();
        Debug.Log(arrRow.Length);
       
        colNumber = arrRow[0].Length;
        rowNumber = arrRow.Length;

        for (int i = 0; i < rowNumber; i++)
        {
            char[] valuesRow = arrRow[i].ToCharArray();
            for (int j = 0; j < colNumber; j++)
            {
                int value = int.Parse(valuesRow[j].ToString());
                GameObject prefab;
                if (GetPrefab(value) != null)
                {
                    prefab = Instantiate(GetPrefab(value));
                    prefab.transform.SetParent(transform, false);
                    prefab.transform.position = new Vector3(j, 0, i);

                    if (value == 3 || value == 5)
                    {
                        listBrick.Add(prefab.transform.GetChild(0).gameObject);
                    }

                    if (value == 4)
                    {
                        listUnBrick.Add(prefab);
                    }

                    if (value == 5)
                    {
                        //player.transform.SetParent(prefab.transform, false);
                        player.transform.position = prefab.transform.position + new Vector3(0, 2f, 0);
                    }

                    if (value == 6)
                    {
                        finish = prefab;
                    }
                }
            }
        }
    }

    private GameObject GetPrefab(int gridValue)
    {
        int indexListPre = gridValue;
        if (indexListPre >= 0 && indexListPre < listPrefabs.Count)
        {
            return listPrefabs[indexListPre];
        }
        else
        {
            return null;
        }
    }

    private Vector3 GridToVector3(int row, int col)
    {
        return new Vector3(row, 0, col);
    }

    private int GetRowOrColNumber(float x)
    {
        return Mathf.FloorToInt(x + 0.5f);
    }

    private Grid Vector3ToGridPos(Vector3 playerPos)
    {
        int row = GetRowOrColNumber(playerPos.x);
        int col = GetRowOrColNumber(playerPos.z);
        return new Grid(row, col);
    }

    private void GetDirect()
    {
        Vector2 swipeDirection = InputManager.Instance.EndPos - InputManager.Instance.StartPos;

        if (InputManager.Instance.IsTouching == false && (swipeDirection != Vector2.zero))
        {
            if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x)) // uu tien huong nao
            {
                if (InputManager.Instance.EndPos.y > InputManager.Instance.StartPos.y)
                {
                    directPlayer = Direct.Foward;
                }
                else
                {
                    directPlayer = Direct.Back;
                }
            }

            else
            {
                if (InputManager.Instance.EndPos.x > InputManager.Instance.StartPos.x)
                {
                    directPlayer = Direct.Right;
                }
                else
                {
                    directPlayer = Direct.Left;
                }
            }
        }
    }

    private void ResetDirect()
    {
        InputManager.Instance.StartPos = Vector3.zero;
        InputManager.Instance.EndPos = Vector3.zero;
        directPlayer = Direct.None;
        isMoving = false;
    }

    private void MoveToTarget()
    {
        Vector3 target = new Vector3(listGrid[listGrid.Count - 1].Row(), player.transform.position.y, listGrid[listGrid.Count - 1].Col());
        PlayerMove.Instance.Move(directPlayer);

        if (Vector3.Distance(player.transform.position, target) < 0.1f)
        {
            ResetDirect();
            player.transform.position = target;
        }
    }

    private void CheckWallManager()
    {
        switch (directPlayer)
        {
            case Direct.Left:
                CheckWallLeft(checkGridPlayer);
                isMoving = true;
                break;
            case Direct.Right:
                CheckWallRight(checkGridPlayer);
                isMoving = true;
                break;
            case Direct.Foward:
                CheckWallFoward(checkGridPlayer);
                isMoving = true;
                break;
            case Direct.Back:
                CheckWallBack(checkGridPlayer);
                isMoving = true;
                break;
            default:
                listGrid.Clear();
                break;
        }
    }

    private void CheckWallFoward(Grid posPlayer)
    {
        for (int i = posPlayer.Col() + 1; i < rowNumber; i++)
        {
            char[] value = arrRow[i].ToCharArray();

            if (value[posPlayer.Row()] == '0' || value[posPlayer.Row()] == '1' || value[posPlayer.Row()] == '6')
            {
                break;
            }
            else
            {
                listGrid.Add(new Grid(posPlayer.Row(), i));
            }
        }
    }

    private void CheckWallBack(Grid posPlayer)
    {
        for (int i = posPlayer.Col() - 1; i >= 0; i--)
        {
            char[] value = arrRow[i].ToCharArray();

            if (value[posPlayer.Row()] == '0' || value[posPlayer.Row()] == '1' || value[posPlayer.Row()] == '6')
            {
                break;
            }
            else
            {
                listGrid.Add(new Grid(posPlayer.Row(), i));
            }
        }
    }

    private void CheckWallRight(Grid posPlayer)
    {
        for (int i = posPlayer.Row() + 1; i < colNumber; i++)
        {
            char[] value = arrRow[posPlayer.Col()].ToCharArray();

            if (value[i] == '0' || value[i] == '1' || value[i] == '6')
            {
                break;
            }
            else
            {
                listGrid.Add(new Grid(i, posPlayer.Col()));
            }
        }
    }

    private void CheckWallLeft(Grid posPlayer)
    {
        for (int i = posPlayer.Row() - 1; i >= 0; i--)
        {
            char[] value = arrRow[posPlayer.Col()].ToCharArray();

            if (value[i] == '0' || value[i] == '1' || value[i] == '6')
            {
                break;
            }
            else
            {
                listGrid.Add(new Grid(i, posPlayer.Col()));
            }
        }
    }
}
