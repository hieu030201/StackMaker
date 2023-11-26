using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private List<GameObject> listBrickWasAdd = new List<GameObject>();
    private static Brick instance;
    public static Brick Instance { get => instance; }
    public List<GameObject> ListBrickWasAdd { get => listBrickWasAdd; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void RemoveBrick(List<GameObject> listUnBrick, Grid posGridPlayer)
    {
        Vector3 posPlayer = new Vector3(posGridPlayer.Row(), 0, posGridPlayer.Col());

        if (listBrickWasAdd.Count != 0)
        {
            for (int i = 0; i < listUnBrick.Count; i++)
            {
                if (posPlayer == listUnBrick[i].transform.position)
                {
                    transform.GetChild(transform.childCount - 1).SetParent(listUnBrick[i].transform);
                    transform.GetChild(1).position -= new Vector3(0, 0.5f, 0);
                    listBrickWasAdd.Remove(listBrickWasAdd[listBrickWasAdd.Count - 1]);
                    listUnBrick[i].transform.GetChild(0).position = listUnBrick[i].transform.position + new Vector3(0, 2.1f, 0);
                }
            }
        }
    }

    public void AddBrick(List<GameObject> listBrick, Grid posGridPlayer)
    {
        Vector3 posPlayer = new Vector3(posGridPlayer.Row(), 0, posGridPlayer.Col());
        for (int i = 0; i < listBrick.Count; i++)
        {
            if (posPlayer == listBrick[i].transform.parent.position && listBrick[i].transform.parent.name != "Player")
            {
                listBrick[i].transform.SetParent(this.transform);
                transform.GetChild(1).position += new Vector3(0, 0.5f, 0);
                listBrickWasAdd.Add(listBrick[i].gameObject);
                listBrick[i].transform.localPosition = new Vector3(0, 0.5f * listBrickWasAdd.Count, 0);
            }
        }
    }

    public bool isFinish(GameObject finish, Grid posGridPlayer)
    {
        if (finish != null && posGridPlayer != null)
        {
            Vector3 posPlayer = new Vector3(posGridPlayer.Row(), 0, posGridPlayer.Col());
            if (Vector3.Distance(finish.transform.position, posPlayer) <= 1.1f)
            {
                Debug.Log("You win!");
                Debug.Log("Score: " + (ListBrickWasAdd.Count - 1));
                foreach (GameObject brick in ListBrickWasAdd)
                {
                    brick.SetActive(false);
                }
                transform.GetChild(1).position = transform.position + new Vector3(0, 0.5f, 0);
                return true;
            }
            return false;
        }
        return false;
    }
}
