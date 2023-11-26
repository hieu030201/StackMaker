//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using static UnityEditor.PlayerSettings;

//enum ElementMap
//{
//    WallOnPillar = 0,
//    Wall = 1,
//    Caro = 2,
//    Brick = 3,
//    Unbrick = 4,
//    StartPoint = 5,
//    Chest = 6,
//}
//public class MapManager : MonoBehaviour
//{
//    private static MapManager instance;
//    public static MapManager Instance { get => instance; }
//    [SerializeField] private GameObject player;

//    #region LIST
//    public List<GameObject> brickPrefabs = new List<GameObject>();
//    [SerializeField] private List<TextAsset> listLevel = new List<TextAsset>();
//    #endregion

//    public string[] arrRow;
//    public int colNumber, rowNumber;
//    private int countMap;

//    private void Awake()
//    {
//        MapManager.instance = this;
//    }
//    void Start()
//    {
//        CreateLevel();
//        countMap = 0;
//    }
//    public GameObject GetPrefab(int gridValue)
//    {
//        int indexInList = gridValue;
//        if (indexInList >= 0 && indexInList < brickPrefabs.Count)
//        {
//            return brickPrefabs[indexInList];
//        }
//        return null;
//    }
//    private string[] ReadLevelText()
//    {
//        string curentMap = listLevel[countMap].name.ToString();
//        TextAsset binData = Resources.Load(curentMap) as TextAsset;
//        string data = binData.text.Replace(Environment.NewLine, string.Empty);
//        return data.Split('-');
//    }
//    private void CreateLevel()
//    {
//        arrRow = ReadLevelText();
//        colNumber = arrRow[0].Length;
//        rowNumber = arrRow.Length;

//        for (int i = 0; i < arrRow.Length; i++)
//        {
//            char[] valuesRow = arrRow[i].ToCharArray();
//            for (int j = 0; j < valuesRow.Length; j++)
//            {
//                int value = int.Parse(valuesRow[j].ToString());
//                GameObject prefab;
//                if (GetPrefab(value) != null)
//                {
//                    prefab = Instantiate(GetPrefab(value));
//                    prefab.transform.SetParent(transform, false);
//                    prefab.transform.position = GridToVector3(j, i);

//                    if (value == 5)
//                    {
//                        player.transform.SetParent(prefab.transform, false);
//                    }
//                }
//            }
//        }
//    }
//    private Vector3 GridToVector3(int row, int col)
//    {
//        return new Vector3(row, 0, col);
//    }

//    private int GetRowOrColNumber(float x)
//    {
//        return Mathf.FloorToInt(x + 0.5f);
//    }

//    public Grid Vector3ToGridPos(Vector3 playerPos)
//    {
//        int row = GetRowOrColNumber(playerPos.x);
//        int col = GetRowOrColNumber(playerPos.z);
//        return new Grid(row, col);
//    }


//}
