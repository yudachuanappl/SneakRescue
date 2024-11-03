using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndChecker : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isGameEnd && Vector2.Distance(player.transform.position,this.transform.position)<=1)
        {
            GameManager.Instance.HandleGameEnd();
        }
    }
}
