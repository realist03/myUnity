using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Pos
{
    bool ifFind = false;
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

    public bool Equals(Pos other)
    {
        if (x == other.x && y == other.y)
        {
            return true;
        }
        else
            return false;
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
    public Transform env;
    public Transform sEnv;
    public Transform way;
    Ray ray;

    int mapWide = 40;
    int mapHeight = 30;

    static int wide = 40;
    static int height = 30;

    int[,] map;
    int[,] searchMap = new int[wide, height];
    
    SearchData[,] sData = new SearchData[wide,height];

    List<Pos> wait = new List<Pos>();
    Pos to;
    Pos thisP;
	void Start ()
    {
        ReadMapFile();
        AddWall();
    }

    void Update ()
    {
        AddPoint();
        if (wait.Count > 0 && to != null)
        {
            //Debug.Log("x:" + wait[0].x + "y:" + wait[0].y);
            StartCoroutine(DrawSearch());
            BFS(to);
        }
        StartCoroutine(DrawWay());
        //Debug.Log(sData[to.x, to.y].step);
        //Debug.Log(wait.Count);
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
                Vector3 pos = new Vector3(i, 0, -j);

                if (map[i, j] == 1)
                {
                    cube = Instantiate(wall, pos, Quaternion.identity,sEnv);
                }
            }
        }
    }

    IEnumerator  DrawSearch()
    {
        Transform cube;

        Vector3 sPos = new Vector3(wait[0].x, 2, -wait[0].y);
        cube = Instantiate(sCube, sPos, Quaternion.identity,sEnv);


        yield return 0;
    }

    IEnumerator DrawWay()
    {
        Transform wayCube;
        int wayStep = sData[to.x, to.y].step;
        int nowStep = wayStep;

        //foreach (var item in sData)
        //{
        //    if (item.step == nowStep)
        //    {
        //        wayCube = Instantiate(way, new Vector3(item., -0.5f, -j), Quaternion.identity);
        //        nowStep--;
        //    }
        //}

        yield return 0;
    }

    public void AddPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(mousePos);
        Transform cha;

        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.gameObject.tag == "Floor")
                {
                    Vector3 pos = new Vector3((int)hit.point.x, hit.point.y + 0.5f, (int)hit.point.z);
                    thisP = new Pos((int)pos.x,-(int)pos.z);
                    cha = Instantiate(character, pos, Quaternion.identity);

                    InitSearch(thisP);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    Vector3 pos = new Vector3((int)hit.point.x, hit.point.y + 0.5f, (int)hit.point.z);
                    to = new Pos((int)pos.x, (int)-pos.z);
                    cha = Instantiate(character, pos, Quaternion.identity);
                }
            }
        }
    }

    public void InitSearch(Pos thisPos)
    {
        wait.Clear();
        wait.Add(thisPos);
        sData[thisPos.x, thisPos.y] = new SearchData(0);
    }

    public bool Search(Pos thisPos,Pos target)
    {
        if(thisPos.x == wide || thisPos.y == height)
        {
            return false;
        }
        //上 y+1
        int step = sData[thisPos.x, thisPos.y].step;

        Pos nextY1 = new Pos(thisPos.x, thisPos.y + 1);

        if (nextY1.y < height)
        {
            if(map[nextY1.x, nextY1.y] != 1)
            {
                if (nextY1.Equals(target))
                {
                    sData[nextY1.x, nextY1.y] = new SearchData(step + 1);

                    return true;
                }
                else
                {
                    if (sData[nextY1.x, nextY1.y] == null)
                    {
                        wait.Add(nextY1);
                        sData[nextY1.x, nextY1.y] = new SearchData(step + 1);
                    }
                }
            }
        }
        //y-1
        Pos nextY_1 = new Pos(thisPos.x, thisPos.y - 1);

        if (nextY_1.y > 0)
        {
            if(map[nextY_1.x, nextY_1.y] != 1)
            {
                if (nextY_1.Equals(target))
                {
                    sData[nextY_1.x, nextY_1.y] = new SearchData(step + 1);

                    return true;
                }
                else
                {
                    if (sData[nextY_1.x, nextY_1.y] == null)
                    {
                        wait.Add(nextY_1);
                        sData[nextY_1.x, nextY_1.y] = new SearchData(step + 1);
                    }
                }
            }
        }
        //x-1
        Pos nextX_1 = new Pos(thisPos.x - 1, thisPos.y);

        if (nextX_1.x > 0)
        {
            if (map[nextX_1.x, nextX_1.y] != 1)
            {
                if (nextX_1.Equals(target))
                {
                    sData[nextX_1.x, nextX_1.y] = new SearchData(step + 1);

                    return true;
                }
                else
                {
                    if (sData[nextX_1.x, nextX_1.y] == null)
                    {
                        wait.Add(nextX_1);
                        sData[nextX_1.x, nextX_1.y] = new SearchData(step + 1);
                    }
                }
            }
        }
        //x+1
        Pos nextX1 = new Pos(thisPos.x + 1, thisPos.y);

        if (nextX1.x < wide)
        {
            if(map[nextX1.x, nextX1.y] != 1)
            {
                if (nextX1.Equals(target))
                {
                    sData[nextX1.x, nextX1.y] = new SearchData(step + 1);

                    return true;
                }
                else
                {
                    if (sData[nextX1.x, nextX1.y] == null)
                    {
                        wait.Add(nextX1);
                        sData[nextX1.x, nextX1.y] = new SearchData(step + 1);
                    }
                }
            }
        }
        return false;
    }

    public void BFS(Pos target)
    {
        //DrawSearch(wait[0]);

        if (Search(wait[0], target) == true)
        {
            //Debug.Log(wait[0]);
            Debug.Log("找到了！");
            wait.Clear();
            return;
        }
        wait.Remove(wait[0]);

    }
}
