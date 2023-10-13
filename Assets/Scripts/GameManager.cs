using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{    public enum MODE
    {
        TITLE, //�^�C�g��
        PLAY, //�v���C��
        OVER //�Q�[���I�[�o�[
    }
    static public MODE GameMode; //�Q�[���̏��

    //int[,] map;
    //[SerializeField] GameObject GroundPrefab;
    //[SerializeField] GameObject WallPrefab;
    //[SerializeField] GameObject WallColliderPrefab;
    [SerializeField] GameObject PlayerPrefab;

    [SerializeField] int MapSize;

    NavMeshSurface myNavMesh; //���g�̃i�r���b�V���T�[�t�F�[�X

    // Start is called before the first frame update
    void Start()
    {
        //���g�̃i�r���b�V���T�[�t�F�[�X���擾
        myNavMesh = GetComponent<NavMeshSurface>();

        //CreateMap();

        GameObject M = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);

        GameMode = MODE.PLAY;
    }

    //void CreateMap()
    //{
    //    WallPrefab.transform.localScale = new Vector3(2.0f, 0.7f, 1.1f);

    //    map = new int[MapSize, MapSize];
    //    float height = -2.5f;

    //    for (int i = 0; i < MapSize; i++)
    //    {
    //        for (int j = 0; j < MapSize; j++)
    //        {
    //            if (i == 0 || j == 0 || i == MapSize - 1 || j == MapSize - 1)
    //            {
    //                map[i, j] = 0;
    //            }
    //            else
    //            {
    //                map[i, j] = 1;
    //            }
    //            SpawnGround(i - MapSize * 0.5f, height, j - MapSize * 0.5f, map[i, j]);
    //        }
    //    }
    //    float colliderLength = MapSize + 20;
    //    WallColliderPrefab.transform.localScale = new Vector3(colliderLength, 20, 2);
    //    Instantiate(WallColliderPrefab, new Vector3(0, height, (MapSize * 0.5f) - 1), Quaternion.Euler(0, 0, 0));
    //    Instantiate(WallColliderPrefab, new Vector3((MapSize * 0.5f) - 1, height, 0), Quaternion.Euler(0, 90, 0));
    //    Instantiate(WallColliderPrefab, new Vector3(0, height, (-MapSize * 0.5f)), Quaternion.Euler(0, 0, 0));
    //    Instantiate(WallColliderPrefab, new Vector3((-MapSize * 0.5f), height, 0), Quaternion.Euler(0, 90, 0));

    //    //�����p�ӂł����̂Ńi�r���b�V���ʐς��v�Z
    //    myNavMesh.BuildNavMesh();
    //}

    //void SpawnGround(float x, float y, float z, int ID)
    //{
    //    Vector3 Pos = new Vector3(x, y, z); 
 
    //    if ( ID == 1 ) 
    //    {
    //        GameObject ground = Instantiate(GroundPrefab, Pos, Quaternion.identity); 
    //        ground.transform.parent = transform;//���̐e���}�l�[�W���[��
    //    }
    //    else if(ID == 0)
    //    {
    //        GameObject wallObj = Instantiate(WallPrefab, Pos, Quaternion.Euler(0, 180, 0)); 
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
