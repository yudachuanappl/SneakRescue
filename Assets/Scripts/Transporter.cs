using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    public Transform[] Gates;
    private GameObject player;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        for (int i = 0;i<Gates.Length;i++)
        {
            if(Vector2.Distance(player.transform.position, Gates[i].position) <= 0.2f && timer<=0)
            {
                Vector3 targetPos = i == 0 ? Gates[1].position : Gates[0].position;
                timer = 1.5f;
                targetPos.z = player.transform.position.z;
                player.transform.position = targetPos;
                break;
            }
        }
    }
}
