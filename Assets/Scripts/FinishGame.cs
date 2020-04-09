using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayersImpact>().enabled = false;
            collision.gameObject.GetComponent<LogsClientAPI>().enabled = false;
            GameManager.Instanse.GetFinishedGame();
            collision.gameObject.SetActive(false);
        }
    }
}
