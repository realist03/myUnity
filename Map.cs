using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Pos
{
    public int x = 0;
    public int y = 0;
    public Pos()
    {
        
    }

    public Pos(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class SearchData
{
    public int step;

    public SearchData(int step)
    {
        this.step = step;
    }
}

public class Map : MonoBehaviour
{
    public Transform wall;
    public Transform sCube;
    public Transform character;
    public GameObject env;
    public GameObject sEnv;
    Ray ray;

    int mapWide = 40;
    int mapHeight = 30;

    static int wide = 50;
    static int height = 50;

    int[,] map;
    int[,] searchMap = new int[wide, height];
    
    SearchData[,] sData = new SearchData[wide,height];

    List<Pos> wait = new List<Pos>();
    Pos to = new Pos(20, 20);
    Pos thisPos = new Pos(5, 5);
	void Start ()
    {
        ReadMapFile();
        AddWall();
        InitSearch(thisPos,to);
    }

    void Update ()
    {
        AddPoint();
        BFS(to);
        Debug.Log("x:" +wait[0].x );
        Debug.Log("y:"+ wait[0].y);

    }

    public void ReadMapFile()
    {
        string path = Application.dataPath + "//" + "Map.txt";
        if (!File.Exists(path))
        {
            return;
        }

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);


        map = new int[mapWide, mapHeight];

        for (int j = 0; j < mapHeight; j++)
        {
            string file = read.ReadLine();

            for (int i = 0; i < mapWide && i < file.Length; i++)
            {
                int a = 0;

                if (file[i] == '#')
                {
                    a = 1;
                    map[i, j] = 1;
                }
            }
        }
    }

    public void  AddWall()
    {
        Transform cube;

        for (int j = 0; j < mapHeight; j++)
        {
            for (int i = 0; i < mapWide; i++)
            {
                Vector3 pos = new Vector3(j, 0, i);

                if (map[i, j] == 1)
                {
                    cube = Instantiate(wall, pos, Quaternion.identity,env.transform);
                }
            }
        }
    }

    public void  DrawSearch()
    {
        Transform cube;

        Vector3 sPos = new Vector3(wait[0].x, 0, wait[0].y);

        cube = Instantiate(sCube, sPos, Quaternion.identity);

    }

    public void AddPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(mousePos);
        Transform cha;

        if(Input.GetMouseButton(0))
        {
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.gameObject.tag == "Floor")
                {
                    Vector3 pos = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                    cha = Instantiate(character, pos, Quaternion.identity);
                }
            }
        }
    }

    public void InitSearch(Pos thisPos,Pos target)
    {
        wait.Clear();
        wait.Add(thisPos);
        sData[thisPos.x, thisPos.y] = new SearchData(0);
    }

    public void Search(Pos thisPos,Pos target)
    {
        DrawSearch();

        //上 y+1
        int step = sData[thisPos.x, thisPos.y].step;

        if(thisPos.x <= wide && thisPos.y <= height && map[thisPos.x, thisPos.y+1] != 1)
        {
            Pos nextY1 = new Pos(thisPos.x, thisPos.y + 1);
            if(nextY1 == target)
            {
                return;
            }
            else
            {
                wait.Add(nextY1);
                sData[nextY1.x, nextY1.y] = new SearchData(step + 1);
            }
        }
        //y-1
        if (thisPos.x <= wide && thisPos.y <= height && map[thisPos.x, thisPos.y-1] != 1)
        {
            Pos nextY_1 = new Pos(thisPos.x, thisPos.y - 1);
            if(nextY_1 == target)
            {
                return;
            }
            else
            {
                wait.Add(nextY_1);
                sData[nextY_1.x, nextY_1.y] = new SearchData(step + 1);
            }
        }
        //x-1
        if (thisPos.x <= wide && thisPos.y <= height && map[thisPos.x-1, thisPos.y] != 1)
        {
            Pos nextX_1 = new Pos(thisPos.x, thisPos.y - 1);
            if(nextX_1 == target)
            {
                return;
            }
            else
            {
                wait.Add(nextX_1);
                sData[nextX_1.x, nextX_1.y] = new SearchData(step + 1);
            }
        }
        //x+1
        if (thisPos.x <= wide && thisPos.y <= height && map[thisPos.x + 1, thisPos.y] != 1)
        {
            Pos nextX1 = new Pos(thisPos.x, thisPos.y - 1);
            if(nextX1 == target)
            {
                return;
            }
            else
            {
                wait.Add(nextX1);
                sData[nextX1.x, nextX1.y] = new SearchData(step + 1);
            }
        }
        wait.Remove(thisPos);
    }

    public void BFS(Pos target)
    {
        Search(wait[0], target);
    }
}
