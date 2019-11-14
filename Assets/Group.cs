using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    // Time since last gravity tick
    float lastFall = 0;
    public GameObject GameOverObject;
    public Canvas GameOverCanvas;
    //Helper Functions
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);

            //Not inside Border?
            if (!Playfield.insideBorder(v))
                return false;

            //Block in grid cell (and not part of the same group)
            if (Playfield.grid[(int)v.x, (int)v.y] != null && Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }

        return true;
    }

    void updateGrid()
    {
        //Remove old children from grid
        for (int y = 0; y < Playfield.height; ++y)
            for (int x = 0; x < Playfield.width; ++x)
                if (Playfield.grid[x, y] != null)
                    if (Playfield.grid[x, y].parent == transform)
                        Playfield.grid[x, y] = null;

        //Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }


    }

    void gameOver()
    {

        GameOverObject = GameObject.Find("GameOverCanvas");
        GameOverCanvas = GameOverObject.GetComponent<Canvas>();
        GameOverCanvas.enabled = true;
        Destroy(GameObject.Find("Spawner"));
        
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



    // Start is called before the first frame update
    void Start()
    {

        //Default position is not valid? GAME OVER!
        if (!isValidGridPos())
        {
            gameOver();
        }
        else
        {
            GameOverObject = GameObject.Find("GameOverCanvas");
            GameOverCanvas = GameOverObject.GetComponent<Canvas>();
            GameOverCanvas.enabled = false;
        }





    }

    // Update is called once per frame and gets key presses
    void Update()
    {
        //Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Modify Position
            transform.position += new Vector3(-1, 0, 0);

            //See if its valid
            if (isValidGridPos())
                //If valid update grid
                updateGrid();
            else
                //if not valid revert to previous position
                transform.position += new Vector3(1, 0, 0);
        }
        //Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Modify Position
            transform.position += new Vector3(1, 0, 0);

            //See if its valid
            if (isValidGridPos())
                //If valid update grid
                updateGrid();
            else
                //if not valid revert to previous position
                transform.position += new Vector3(-1, 0, 0);
        }

        // Move Downwards and Fall
        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if its valid
            if (isValidGridPos())
            {
                // If valid update grid.
                updateGrid();
            }
            else
            {
                // If not valid revert to previous position
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Playfield.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }

        //Rotate Blocks
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            //Rotate block group
            transform.Rotate(0, 0, -90);

            //See if its valid
            if (isValidGridPos())
                //If valid update grid
                updateGrid();
            else
                //If not valid revert to previous position
                transform.Rotate(0, 0, 90);

        }





    }
}
