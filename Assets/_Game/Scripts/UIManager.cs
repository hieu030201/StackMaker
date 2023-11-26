using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
   public int level;
   public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
