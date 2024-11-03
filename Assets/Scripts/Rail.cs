using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public Transform[] RailEnds;
    public GameObject Controller;
    private GameObject player;
    private bool startMove;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < RailEnds.Length; i++)
        {
            if (Vector2.Distance(player.transform.position, RailEnds[i].position) <= 0.5f )
            {
                if(Input.GetKey(KeyCode.E))
                {
                    this.GetComponent<Blinking>().StopBlinking();
                    targetPos = i == 0 ? RailEnds[1].position : RailEnds[0].position;
                    targetPos.z = player.transform.position.z;
                    startMove = true;
                    player.transform.parent = Controller.transform;
                    player.transform.localPosition = new Vector3(0, 0f, 0);
                    UserControl.Instance.CouldControl = false;
                    break;
                }
            }
        }

        if(startMove)
        {
            Controller.transform.position = Vector2.MoveTowards(Controller.transform.position, targetPos, 3 * Time.deltaTime);
            if(Vector2.Distance(Controller.transform.position,targetPos)<=0.1f)
            {
                startMove = false;
                player.transform.parent = null;
                UserControl.Instance.CouldControl = true;
            }
        }
    }
}
