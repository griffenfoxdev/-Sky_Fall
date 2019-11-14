using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    //The playing field grid
    public static int width = 10;
    public static int height = 32;
    public static Transform[,] grid = new Transform[width, height];

    //Helper Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Helper function to round the vector
    //Used because rotations cause the coordinates to not be round numbers anymore
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    //Helper function to check if coordinate is between the borders (Side_Border_Left, Side_Border_Right, Vertical)
    //First tests the x position is between 0 and the grid width, then finds out if the y position is still positive
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    //Helper function to delete all blocks in a certain row
    //Used when a player fills an entire row with blocks
    public static void deleteRow(int y)
    {
        //Takes the y parameter (row to be deleted), loops through each block in that row to destroy it and clear the reference
        for (int x = 0; x < width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //Helper function to move the remaining rows after deletion down a level
    //Loops through every block in the row given in the parameter and moves it one unit to the bottom while updating the blocks world position
    //  World Position is updated by adding Vector(0, -1, 0) to it
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                //Move one towards the bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //Update block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //Helper function to move every row above a certain index (row deleted) down one level
    public static void decreaseRowAbove(int y)
    {
        for (int i = y; i < height; ++i)
            decreaseRow(i);
    }

    //Helper function to check if a row is full
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < width; ++x)
            if (grid[x, y] == null)
                return false;

        return true;
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    //Function to delete full rows
    //This function uses all the above helper functions to delete all full ros and always decrease the above rows y coordinates by 1
    public static void deleteFullRows()
    {
        for (int y = 0; y < height; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowAbove(y + 1);
                ScoreManager.score += (height - y) * 10;
                ScoreManager.lines += 1;
                --y;

            }
        }
    }
}
