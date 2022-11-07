using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharaterSelection : MonoBehaviour
{

    public GameObject[] charater;
    public int seletedcharater = 0;


    public void NextCharater()
    {
        charater[seletedcharater].SetActive(false);
        seletedcharater = (seletedcharater + 1) % charater.Length;
        charater[seletedcharater].SetActive(true);
    }
    
    public void PreviousCharater()
    {
        charater[seletedcharater].SetActive(false);
        seletedcharater--;
        if(seletedcharater < 0)
        {
            seletedcharater += charater.Length;
        }
        charater[seletedcharater].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("seletedCharacter", seletedcharater);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
