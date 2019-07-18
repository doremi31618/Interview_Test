using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : MonoBehaviour
{
    public int max_capacity = 4;
    public int randomNumber = 5;

    public Material visualizer_material;
    public QuadTreeData quadTree;
    public GameObject pointPrefab;

    #region custom class
    [System.Serializable]
public class Point
{
    public float x, y;
    public Point(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
}
[System.Serializable]
public class Rectangle
{
    public float x, y, w, h;

    public Rectangle(float _x, float _y, float _w, float _h)
    {
        x = _x;//the x cordinate of center
        y = _y;//the y cordinate of center
        w = _w;//the hieght of boundary
        h = _h;//the width of boundary
        GameObject visualizer = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube),new Vector3(x,500/w,y),Quaternion.identity);
        visualizer.transform.localScale = new Vector3(w,300/w,h);
        visualizer.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/visualizer");;
        // Color c = visualizer.GetComponent<MeshRenderer>().material.color;
        // c = new Vector4(w,w,w,w);
        // visualizer.GetComponent<MeshRenderer>().material.SetColor("_Color",c);
       
    }

    public bool isContains(float _x, float _y)
    {
        return (
            (_x < x + w) &&
            (_x > x) &&
            (_y < y + h) &&
            (_y > y)
        );
    }

}

[System.Serializable]
public class QuadTreeData
{
    public int capacity;
    public Rectangle boundary = new Rectangle(0, 0, 200, 200);
    public List<Point> points;
    private bool divided = false;
    public List<QuadTreeData> dividedSection = new List<QuadTreeData>();
    public QuadTreeData(Rectangle _boundary, int _capacity)
    {
        this.boundary = _boundary;
        this.capacity = _capacity;
        this.points = new List<Point>();
        this.dividedSection = new List<QuadTreeData>();
        this.divided = false;
        Debug.Log("divided" + this.divided);

    }
    public void Insert(Point _point)
    {

        if (!this.boundary.isContains(_point.x, _point.y)) return;

        if (this.points.Count < this.capacity)
        {
            this.points.Add(_point);
        }
        else
        {
            //Debug.Log(this.divided);
            if (!divided)
            {
                this.subdivide();

            }

            this.dividedSection[0].Insert(_point);
            this.dividedSection[1].Insert(_point);
            this.dividedSection[2].Insert(_point);
            this.dividedSection[3].Insert(_point);


        }

    }
    private void subdivide()
    {
        //divide to four section 
        //north west : nw
        //north east : ne
        //south west : sw
        //south east : ses

        Rectangle nw = new Rectangle(this.boundary.x - this.boundary.w / 4, this.boundary.y + this.boundary.h / 4, this.boundary.w / 2, this.boundary.h / 2);
        dividedSection.Add(new QuadTreeData(nw, this.capacity));

        Rectangle ne = new Rectangle(this.boundary.x + this.boundary.w/4, this.boundary.y + this.boundary.h / 4, this.boundary.w / 2, this.boundary.h / 2);
        dividedSection.Add(new QuadTreeData(ne, this.capacity));

        Rectangle sw = new Rectangle(this.boundary.x - this.boundary.w / 4, this.boundary.y - this.boundary.h / 4, this.boundary.w / 2, this.boundary.h / 2);
        dividedSection.Add( new QuadTreeData(sw, this.capacity));

        Rectangle se = new Rectangle(this.boundary.x + this.boundary.w / 4, this.boundary.y - this.boundary.h / 4, this.boundary.w / 2, this.boundary.h / 2);
        dividedSection.Add(new QuadTreeData(se, this.capacity));

        this.divided = true;

    }
}
    
    #endregion

    #region unity function
    private void Start() {
        Rectangle _boundary = new Rectangle(0,0,200,200);
        quadTree = new QuadTreeData(_boundary,max_capacity);
        for(int i = 0;i<randomNumber;i++)
        {
            Point p = new Point((Random.value-0.5f) * 200,(Random.value-0.5f)*200);
            //GameObject pointVisualizer = Instantiate(pointPrefab,new Vector3(p.x,10,p.y),Quaternion.identity);
            quadTree.Insert(p);
            //Debug.Log(p);
        }
    }
    #endregion
}
