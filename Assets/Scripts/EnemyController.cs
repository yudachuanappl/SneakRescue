using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float ViewRangeAngle;
    public float ViewRangeDistance;
    public bool RotationMode;
    public float RotationSpeed;
    public bool MoveMode;
    public GameObject[] MovePath;
    public float MoveSpeed;
    public GameObject Avatar;
    public Material ViewMat;
    private int currentMovePathIndex = 0;
    private Animator animator;
    private GameObject go;
    private MeshFilter mf;
    private MeshRenderer mr;
    private Shader shader;
    private GameObject Player;
    private bool isEnemyDisabled;


    public float MinDistanceGetAttracted;
    private Vector3 currentTargetPos;

    public void GetHit()
    {
        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        go.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = Avatar.GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        isEnemyDisabled = false;
    }


    IEnumerator ShootAnimation()
    {
        this.transform.LookAt(Player.transform.position);
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(2);
       // Player.GetComponent<UserControl>().Death();
        yield return new WaitForSeconds(1);
        animator.SetTrigger("ShootDone");
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameEnd) return;
        //if there is sounds
        if(IsBehaviourValid())
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentTargetPos, MoveSpeed * Time.deltaTime);
            transform.up = currentTargetPos - transform.position;
            if(Vector3.Distance(currentTargetPos,this.transform.position)<0.3f)
            {
                isEnemyDisabled = true;
            }
        }
        else
        {
            if (RotationMode)
            {
                this.transform.Rotate(new Vector3(0, 0, RotationSpeed) * Time.deltaTime);
            }
            if (MoveMode)
            {
                // animator.SetBool("Walk", true);
                this.transform.position = Vector3.MoveTowards(this.transform.position, MovePath[currentMovePathIndex].transform.position, MoveSpeed * Time.deltaTime);
                //   this.transform.LookAt(MovePath[currentMovePathIndex].transform.position);
                transform.up = MovePath[currentMovePathIndex].transform.position - transform.position;
                if(this.GetComponentInChildren<Animator>()!=null) this.GetComponentInChildren<Animator>().SetBool("IsWalk", true);
                if (Vector3.Distance(this.transform.position, MovePath[currentMovePathIndex].transform.position) <= 0.1f)
                {
                    currentMovePathIndex++;
                    if (currentMovePathIndex >= MovePath.Length)
                    {
                        currentMovePathIndex = 0;
                    }

                }
            }
        }
        //draw range and detect player

       ToDrawSectorSolid(transform, transform.localPosition, ViewRangeAngle, ViewRangeDistance);
       if (DetectPlayer())
       {
           GameManager.Instance.HandleGameFailed();
       }
        // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 5,Color.red);
        //if (DetectPlayer() && !Player.GetComponent<UserControl>().IsDead)
        //{
        //    StartCoroutine(ShootAnimation());
        //}
    }
    public float CalAngle(Vector3 from, Vector3 to)
    {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector3.Angle(from, to);
        return cross.z > 0 ? -angle : angle;
    }

    private bool CouldSeeTarget(Vector3 position,string tag)
    {
        Vector3 enemyToPlayer = position - this.transform.position;
        bool couldSee = false;
        //Debug.DrawRay(transform.position, enemyToPlayer, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPlayer);
        if (hit.collider!=null && hit.transform.tag == tag)
        {
            couldSee = true;
        }
       return couldSee;
    }    

    private bool DetectPlayer()
    {
        if (Player == null) return false;
        if (!Player.GetComponent<UserControl>().CouldControl) return false;

        if (Vector3.Distance(this.transform.position, Player.transform.position) > ViewRangeDistance) return false;

        Vector3 enemyToPlayer = Player.transform.position - this.transform.position;
        float angle = CalAngle(transform.up.normalized, enemyToPlayer.normalized);
        angle = Mathf.Abs(angle);

        if (angle <= ViewRangeAngle / 2 && (CouldSeeTarget(Player.transform.position,"Player")|| isEnemyDisabled))
        {
            print("see player");
            return true;
        }

        return false;
    }

    private GameObject CreateMesh(List<Vector3> vertices)

    {

        int[] triangles;

        Mesh mesh = new Mesh();

        int triangleAmount = vertices.Count - 2;

        triangles = new int[3 * triangleAmount];

        //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）   

        //顺序必须为顺时针或者逆时针      

        for (int i = 0; i < triangleAmount; i++)

        {

            triangles[3 * i] = 0;//固定第一个点      

            triangles[3 * i + 1] = i + 1;

            triangles[3 * i + 2] = i + 2;

        }

        if (go == null)

        {

            go = new GameObject("mesh");

            go.transform.localPosition = new Vector3(0, 0, 0.2f);//让绘制的图形上升一点，防止被地面遮挡  
            go.transform.eulerAngles = new Vector3(0, 0, 0);
            mf = go.AddComponent<MeshFilter>();

            mr = go.AddComponent<MeshRenderer>();

        }

        mesh.vertices = vertices.ToArray();

        mesh.triangles = triangles;

        mf.mesh = mesh;

        mr.material = ViewMat;

        return go;

    }  

    public void ToDrawSectorSolid(Transform t, Vector3 center, float angle, float radius)  

    {  

        int pointAmount = 100;//点的数目，值越大曲线越平滑   

        float eachAngle = angle / pointAmount;

        Vector3 forward = t.up;

        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(center);  

        for (int i = 1; i<pointAmount - 1; i++)  
        {  
            Vector3 pos = Quaternion.Euler(0f,  0f, -angle / 2 + eachAngle * (i - 1)) * forward * radius + center;
            vertices.Add(pos);  
        }

        CreateMesh(vertices);  

    }

    public bool IsBehaviourValid()
    {
        if ((GameManager.Instance.ActiveAudioList.Count != 0 && GetCloestAudioSource() && CouldSeeTarget(currentTargetPos,"PHONE")))
        {
            return true;
        }
        return false;
    }


    private bool GetCloestAudioSource()
    {
        if (GameManager.Instance.ActiveAudioList.Count == 0) return false;

        float shortestDis = float.MaxValue;
        for (int i = 0; i < GameManager.Instance.ActiveAudioList.Count; i++)
        {
            float dis = Vector3.Distance(this.transform.position, ((GameObject)GameManager.Instance.ActiveAudioList[i]).transform.position);
            if (dis < shortestDis && dis <= MinDistanceGetAttracted)
            {
                shortestDis = dis;
                currentTargetPos = ((GameObject)GameManager.Instance.ActiveAudioList[i]).transform.position;
            }
        }
        return shortestDis != float.MaxValue && shortestDis <= MinDistanceGetAttracted;
    }



}
