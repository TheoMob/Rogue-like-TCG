using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MoveCardToHand>().enabled = false;
        SceneManager.LoadScene("BonfireScene");
    }
}
