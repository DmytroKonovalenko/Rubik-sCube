using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject pieceCube;
    Transform cubeTransform;
    List<GameObject> AllPieceCube = new List<GameObject>();
    GameObject cubeCenterPiece;
    bool canRotate=true;
    bool canShaffle = true;
    
    List<GameObject>UpPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 0);
        }
    }
    List<GameObject> DownPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -2);
        }
    }
    List<GameObject> FrontPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 0);
        }
    }
    List<GameObject> BackPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -2);
        }
    }
    List<GameObject> LeftPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 0);
        }
    }
    List<GameObject> RightPieces
    {
        get
        {
            return AllPieceCube.FindAll(x => Mathf.Round(x.transform.localPosition.z) == -2);
        }
    }

    Vector3[] RotationVectors =
    {
        new Vector3(0,1,0), new Vector3 (0,-1,0),
        new Vector3(0,0,-1),new Vector3 (0,0,1),
        new Vector3 (1,0,0),new Vector3(-1,0,0)
    };
    void Start()
    {
        cubeTransform = transform;
        CreateCube();
    }

    
    void Update()
    {
        if(canRotate)
        ChekInput();
    }
    private void CreateCube()
    {
        foreach (GameObject go in AllPieceCube)
            DestroyImmediate(go);
        AllPieceCube.Clear();
            for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    GameObject go = Instantiate(pieceCube, cubeTransform, false);
                    go.transform.localPosition =new Vector3 (-x, -y, -z);
                    go.GetComponent<PieceCube>().SetColor(x, -y, z);
                    AllPieceCube.Add(go);
                }
        cubeCenterPiece = AllPieceCube[13 ];
    }
    void ChekInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0)));
        else if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, -1, 0)));
        else if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, -1)));
        else if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1)));
        else if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(Rotate(FrontPieces, new Vector3(1, 0, 0)));
        else if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Rotate(BackPieces, new Vector3(-1, 0, 0)));
        else if (Input.GetKeyDown(KeyCode.Z) && canShaffle)
            StartCoroutine(Shuffle());
        else if (Input.GetKeyDown(KeyCode.Q) && canShaffle)
            CreateCube();
    }

    IEnumerator Shuffle()
    {
        canShaffle = false;

        for (int moveCount=Random.Range(15,30);moveCount>=0;moveCount--)
        {
            int edge = Random.Range(0, 6);
            List<GameObject> edgePieces = new List<GameObject>();

            switch(edge)
            {
                case 0:edgePieces = UpPieces;break;
                case 1: edgePieces = DownPieces;break;
                case 2: edgePieces = LeftPieces;break;
                case 3: edgePieces = RightPieces; break;
                case 4: edgePieces = FrontPieces; break;
                case 5: edgePieces = BackPieces; break;
            }
            StartCoroutine(Rotate(edgePieces,RotationVectors[edge],15));
            yield return new WaitForSeconds(.3f);
        }

        canShaffle = true;
    }
    IEnumerator Rotate(List<GameObject>pieces,Vector3 rotationVec,int speed=5)
    {
        canRotate = false;
        int angle = 0;
        while(angle<90)
        {
            foreach (GameObject go in pieces)
                go.transform.RotateAround(cubeCenterPiece.transform.position, rotationVec, speed);
            angle += speed;
            yield return null;
        }
        CheckComplete();

        canRotate = true;
        
    }
    void CheckComplete()
    {
        if (IsSideComplete(UpPieces) &&
            IsSideComplete(DownPieces) &&
            IsSideComplete(LeftPieces) &&
            IsSideComplete(RightPieces) &&
            IsSideComplete(FrontPieces) &&
            IsSideComplete(BackPieces)) 
        Debug.Log("Complete!");

    }
    bool IsSideComplete(List<GameObject> pieces)
    {
        int mainPlaneIndex = pieces[4].GetComponent<PieceCube>().Planes.FindIndex(x => x.activeInHierarchy);

        for (int i=0; i<pieces.Count; i++)
        {
            if (!pieces[i].GetComponent<PieceCube>().Planes[mainPlaneIndex].activeInHierarchy ||
                pieces[i].GetComponent<PieceCube>().Planes[mainPlaneIndex].GetComponent<Renderer>().material.color !=
                pieces[4].GetComponent<PieceCube>().Planes[mainPlaneIndex].GetComponent<Renderer>().material.color)
                return false;
            

            
        }
        return true;
    }
}
