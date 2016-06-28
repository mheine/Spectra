using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PanelGUI : MonoBehaviour {

    private bool isShowing;
    private bool isPlaying;

    private string dir;

    private int counter;

    public AudioSource audio_source;
    public GameObject canvasObject;
    public GameObject grid;
    public Button button;

    public WWW request;

    public Object[] songlist;


    // Use this for initialization
    void Start () {

        isShowing = true;
        isPlaying = true;
        counter = 0;

        List<string> filesAsList = new List<string>();

        button          = GameObject.Find("B1").GetComponent<Button>();
        grid            = GameObject.Find("Grid");
        audio_source           = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        canvasObject    = GameObject.Find("SongGUI");

        //Load all resources (hopefully there are only .mp3 there)
        songlist = Resources.LoadAll("music");

        //Add all songs to the list
        foreach (object song in songlist)
        {
            filesAsList.Add(song.ToString());
        }


        //Create the 
        foreach (string filename in filesAsList)
        {
     
            //Create button and clean up filename
            var btn = (Button)Instantiate(button, transform.position, Quaternion.identity);
            string strippedFilename = filename.Substring(0, filename.Length - (24));
            btn.name = "B_" + strippedFilename;

            //Set the grid as the parent
            btn.transform.SetParent(grid.transform, false);

            //Set the text of our button to the filename (excluding the dir and the '.mp3')
            btn.GetComponentInChildren<Text>().text = strippedFilename;

            //Add an actionlistener to each button - counter is the index in the list
            var index = counter;
            btn.onClick.AddListener(delegate { PlayTrack(index); });

            counter++;
        }

        //Deactivate our dummy button
        GameObject.Find("B1").SetActive(false);
        GameObject.Find("Scrollbar").SetActive(false); 


    }


    /* UNUSED - IEnumerator to load tracks on runtime from StreamingAssets*/
    IEnumerator loadTrack(string file)
    {
        string path = "file://" + Application.dataPath + "/StreamingAssets/" + file + ".mp3";
        print("Loading from streamingAsset" + path);

        request = new WWW(path);

        // Wait for download to complete
        yield return request;
    }


    void PlayTrack(int index)
    {
        //Stop the current song, set the clip as the song with the correct index, and play song
        audio_source.Stop();
        audio_source.clip = (AudioClip)songlist[index];
        audio_source.Play();
    }

    // Update is called once per frame
    void Update () {

        //Show or hide GUI
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isShowing = !isShowing;
            canvasObject.GetComponent<Canvas>().enabled = isShowing;
        }

        //Play or pause the current song
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPlaying)
                audio_source.Pause();
     
            else
                audio_source.Play();

            isPlaying = !isPlaying;

        }

        //Quit application
        if (Input.GetKey("escape"))
            Application.Quit();

    }
}
