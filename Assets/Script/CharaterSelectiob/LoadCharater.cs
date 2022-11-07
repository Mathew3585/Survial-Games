using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharater : MonoBehaviour
{

    public GameObject[] charaterPrebabs;
    public Transform spawnPoint;

    void Start()
    {
        int seletedcharater = PlayerPrefs.GetInt("seletedCharacter");
        GameObject prefabs = charaterPrebabs[seletedcharater];
        GameObject clone = Instantiate(prefabs, spawnPoint.position, Quaternion.identity); 
    }


}
