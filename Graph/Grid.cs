using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Struct 공부해서 그걸로 만들기
public struct Grid 
{

    public float offset_x { get; private set; }
    public float offset_y { get; private set; }
    public int block_x { get; private set; }
    public int block_y { get; private set; }

    public Grid(int block_x, int block_y, float offset_x = 0.0f, float offset_y = 0.0f )
    {
        this.offset_x = offset_x;
        this.offset_y = offset_y;
        this.block_x = block_x;
        this.block_y = block_y;
    }
    /// <summary>
    /// return new Grid(1, 0);
    /// </summary>
    public static Grid up = new Grid(1, 0);
    /// <summary>
    /// return new Grid(-1, 0);
    /// </summary>
    public static Grid down = new Grid(-1, 0);
    /// <summary>
    /// return new Grid(1, 0);
    /// </summary>
    public static Grid right = new Grid(1, 0);
    /// <summary>
    /// return new Grid(-1, 0);
    /// </summary>
    public static Grid left = new Grid(-1, 0);

    /// <summary>
    /// {up, down, left, right}가 들어있음.
    ///  Iterator를 위해서 사용한다.
    /// </summary>
    public static Grid[] UDLR = { up, down, left, right };
    public static Grid GetRandomUDLR()
    {
        int i = Random.Range(0, 4);

        switch(i)
        {
            case 0: return up; break;
            case 1: return down; break;
            case 2: return left; break;
            case 3: return right; break;
            default: throw new System.Exception(); break;
        }
    }
    
    public bool Equals(Grid other)
    {
        return (Mathf.Abs(this.offset_x - other.offset_x) < GameMgr.FLOAT_MARGIN_OF_ERROR) 
            && ((Mathf.Abs(this.offset_y - other.offset_y) < GameMgr.FLOAT_MARGIN_OF_ERROR))
            && (this.block_x == other.block_x)
            && (this.block_y == other.block_y);
    }

    public override bool Equals(object obj)
    {
        if(obj is Grid)
        {
            return this.Equals(obj);
        }
        else return false;
    }
    public override int GetHashCode()
    {
        int hashcodex = offset_x.GetHashCode();
        int hashcodey = offset_y.GetHashCode();

        return hashcodex ^ hashcodey ^ block_x ^ block_y;
    }


    public static bool operator ==(Grid a, Grid b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Grid a, Grid b)
    {
        return !a.Equals(b);
    }

    public static Grid operator +(Grid a, Grid b)
    {
        return new Grid(a.block_x + b.block_x,
                              a.block_y + b.block_y,
                              a.offset_x + b.offset_x,
                              a.offset_y + b.offset_y);

    }
}
