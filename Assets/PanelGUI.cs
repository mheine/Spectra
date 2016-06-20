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


        Button button = GameObject.Find("B1").GetComponent<Button>();
        GameObject grid = GameObject.Find("Grid");

        //Simple filter for .mp3 files
        foreach (string filename in listOfFiles)
        {
            if(filename.EndsWith(".mp3"))
                filesAsList.Add(filename);
        }


        foreach (string filename in filesAsList)
        {
     
            //Create button and clean up filename
            var btn = (Button)Instantiate(button, transform.position, Quaternion.identity);
            string strippedFilename = filename.Substring(dir.Length + 1, filename.Length - (dir.Length + 5));

            btn.name = "B_" + strippedFilename;

            //Set the grid as the parent
            btn.transform.SetParent(grid.transform, false);

            //Set the text of our button to the filename (excluding the dir and the '.mp3')
            btn.GetComponentInChildren<Text>().text = strippedFilename;

            //Add an actionlistener to each button
            btn.onClick.AddListener(delegate { test(btn.name); });

            counter++;
        }

        //Deactivate our dummy button
        GameObject.Find("B1").SetActive(false);
        //button.enabled = false;


    }

    void test(string s)
    {
        print("We got a click from button: " + s);
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
