using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PanelGUI : MonoBehaviour {

    private bool isShowing;
    private string dir;
    private int counter;


	// Use this for initialization
	void Start () {

        isShowing = true;
        counter = 0;
        //Hard-coded directory, for now
        dir = "C:\\Users\\Marcus\\Music\\Pendulum";

        string[] listOfFiles = Directory.GetFiles(dir);
        List<string> filesAsList = new List<string>();


        var button = GameObject.Find("B1");
        GameObject grid = GameObject.Find("Grid");

        foreach (string filename in listOfFiles)
        {
            if(filename.EndsWith(".mp3"))
            {
                filesAsList.Add(filename);
            }
        }


            foreach (string filename in filesAsList)
        {
     
            var btn = (GameObject)Instantiate(button, transform.position, Quaternion.identity);
            btn.name = "B_" + counter;

            //Set the grid as the parent
            btn.transform.SetParent(grid.transform, false);

            //Set the text of our button to the filename (excluding the dir and the '.mp3')
            btn.GetComponentInChildren<Text>().text = filename.Substring(dir.Length+1, filename.Length-(dir.Length + 5));
            counter++;
        }

        //Deactivate our dummy button
        button.SetActive(false);


    }

    // Update is called once per frame
    void Update () {

        GameObject canvasObject = GameObject.Find("SongGUI");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isShowing = !isShowing;
            canvasObject.GetComponent<Canvas>().enabled = isShowing;
        }

    }
}
