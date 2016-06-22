using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
[RequireComponent (typeof (AudioSource))]

public class PanelGUI : MonoBehaviour {

    private bool isShowing;
    private bool isPlaying;
    private string dir;
    private int counter;
    public AudioSource sound;


    // Use this for initialization
    void Start () {

        isShowing = true;
        isPlaying = true;
        counter = 0;

        //Hard-coded directory, for now
        dir = Application.dataPath + "//Resources";

        string[] listOfFiles = Directory.GetFiles(dir);
        List<string> filesAsList = new List<string>();

        GameObject grid = GameObject.Find("Grid");
        Button button = GameObject.Find("B1").GetComponent<Button>();
        

        //Simple filter for .mp3 files
        foreach (string filename in listOfFiles)
        {
            if(filename.EndsWith(".mp3") || filename.EndsWith(".wav"))
                filesAsList.Add(filename);
        }


        foreach (string filename in filesAsList)
        {
     
            //Create button and clean up filename
            var btn = (Button)Instantiate(button, transform.position, Quaternion.identity);
            string strippedFilename = filename.Substring(dir.Length + 1, filename.Length - (dir.Length + 5));


            

            btn.name = "B_" + strippedFilename;
            //sound = btn.GetComponent<AudioSource>();
            //sound.clip = clip;
            //btn.GetComponent<AudioSource>().clip = clip;

            //Set the grid as the parent
            btn.transform.SetParent(grid.transform, false);

            //Set the text of our button to the filename (excluding the dir and the '.mp3')
            btn.GetComponentInChildren<Text>().text = strippedFilename;

            //Add an actionlistener to each button
            btn.onClick.AddListener(delegate { PlayOrPause(strippedFilename); });

            counter++;
        }

        //Deactivate our dummy button
        GameObject.Find("B1").SetActive(false);
        //button.enabled = false;


    }

    void PlayOrPause(string dir)
    {  
        var clip = Resources.Load(dir) as AudioClip;
        sound = GameObject.Find("Audio_Source").GetComponent<AudioSource>();

        sound.Stop();
        sound.clip = clip;
        sound.Play();
    }

    // Update is called once per frame
    void Update () {

        GameObject canvasObject = GameObject.Find("SongGUI");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isShowing = !isShowing;
            canvasObject.GetComponent<Canvas>().enabled = isShowing;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var sound = GameObject.Find("Audio_Source").GetComponent<AudioSource>();
            if (isPlaying) {
                sound.Pause();
                isPlaying = !isPlaying;
            }

            else {
                sound.Play();
                isPlaying = !isPlaying;
            }
                
        }

    }
}
