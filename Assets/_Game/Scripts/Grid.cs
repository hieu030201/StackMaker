using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid
{
    private int row;
    private int col;

    public Grid(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public int Row() { return row; }
    public int Col() { return col; }
}
