using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진행중인 게임 영역에 대한 정보를 담고 있는 클래스이다.
/// 게임 영역에 따라 카메라가 담는 영역을 변화시킨다.
/// </summary>
public class Frame
{
    private float top, right;
    private static readonly float INITIAL_TOP = 2.0f;
    private static readonly float INITIAL_RIGHT = 3.0f;

    private static readonly float EXPAND_SPEED = 1.0f;

    private Camera camera;

    public Frame()
    {
        top = INITIAL_TOP;
        right = INITIAL_RIGHT;
    }
    
    public Grid GetRandomGridInFrame()
    {
        int iTop = Mathf.FloorToInt(top);
        int iRight = Mathf.FloorToInt(right);

        //set x, y
        int randomX = Random.Range(-iRight, iRight + 1);
        int randomY = Random.Range(-iTop, iTop + 1);

        //set offset x, y
        float randomOffsetX = Random.Range(0f, 1f);
        float randomOffsetY = Random.Range(0f, 1f);

        //clamp check
        bool leftC, rightC, topC, bottomC;  //direction is clamped by Frame?
        if (randomY == iTop)    topC = true;    else topC = false;
        if (randomY == -iTop) bottomC = true; else bottomC = false;
        if (randomX == iRight)  rightC = true;  else rightC = false;
        if (randomX == -iRight) leftC = true; else leftC = false;

        float clampOffsetRight = Mathf.Clamp01(2 * (top - Mathf.Floor(right))); //offset ratio clamped by frame
        float clampOffsetTop = Mathf.Clamp01(2 * (top - Mathf.Floor(top)));

        if (rightC)
            randomOffsetX = Random.Range(0f, clampOffsetRight);
        else if (leftC)
            randomOffsetX = Random.Range(1f - clampOffsetRight, 1f);
        if (topC)
            randomOffsetY = Random.Range(0f, clampOffsetTop);
        else if (bottomC)
            randomOffsetY = Random.Range(1f - clampOffsetTop, 1f);

        bool horizontalOffset = Random.Range(0, 2) == 0 ? true : false; //horizontal or vertical
        if (horizontalOffset)
            randomOffsetY = Mathf.Round(randomOffsetY);
        else
            randomOffsetX = Mathf.Round(randomOffsetX);

        return new Grid(randomX, randomY, randomOffsetX, randomOffsetY);
    }

    public bool isGridInFrame(Grid grid)
    {
        int iTop = Mathf.FloorToInt(top);
        int iRight = Mathf.FloorToInt(right);

        float clampOffsetRight = Mathf.Clamp01(2 * (top - Mathf.Floor(right))); //offset ratio clamped by frame
        float clampOffsetTop = Mathf.Clamp01(2 * (top - Mathf.Floor(top)));

        //block check -> offset check, x -> y check order
        if (grid.block_x < -iRight) return false;
        else if (grid.block_x > iRight) return false;
        else if (grid.block_x == -iRight)
        {
            if (grid.offset_x < 1f - clampOffsetRight) return false;
        }
        else if (grid.block_x == iRight)
        {
            if (grid.offset_x > clampOffsetRight) return false;
        }

        if (grid.block_y < -iTop) return false;
        else if (grid.block_y > iTop) return false;
        else if (grid.block_y == -iTop)
        {
            if (grid.offset_y < 1f - clampOffsetTop) return false;
        }
        else if (grid.block_y == iTop)
        {
            if (grid.offset_y > clampOffsetTop) return false;
        }

        return true;
    }

}
