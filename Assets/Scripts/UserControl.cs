using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{
    public float Speed;
    public bool IsCarryingTarget;
    public static UserControl Instance;
    public bool CouldControl;
    // Start is called before the first frame update
    void Start()
    {
        CouldControl = true;
        IsCarryingTarget = false;
        this.GetComponentInChildren<Animator>().SetBool("WithTarget", false);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void HandleCarryTarget()
    {
        this.GetComponentInChildren<Animator>().SetBool("WithTarget", true);
        IsCarryingTarget = true;
        Speed /= 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameEnd || !CouldControl)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 vel = (h!=0 && v!=0)? new Vector2(h * Speed/Mathf.Sqrt(2), -v * Speed / Mathf.Sqrt(2)) : new Vector2(h * Speed, -v * Speed);
        this.GetComponent<Rigidbody2D>().velocity = vel;
        if (h!=0||v!=0)
        {
            transform.up = new Vector3(h, -v, 0);
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", true);
        }
        else
        {
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", false);
        }
    }
}
