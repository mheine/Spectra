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
    public AudioSource sound;
    public AudioClip current;
    public WWW request;
    public Object[] songlist;


    // Use this for initialization
    void Start () {

        isShowing = true;
        isPlaying = true;
        counter = 0;

        sound = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        //Hard-coded directory, for now
        dir = Application.dataPath + "//Resources";

        songlist = Resources.LoadAll("music");

        print("type: " + songlist.GetType());

        string[] listOfFiles = Directory.GetFiles(dir);
        List<string> filesAsList = new List<string>();


        Button button = GameObject.Find("B1").GetComponent<Button>();
        GameObject grid = GameObject.Find("Grid");

        //Simple filter for .mp3 files
        foreach (object song in songlist)
        {
            filesAsList.Add(song.ToString());
        }

        //Simple filter for .mp3 files
        foreach (string filename in listOfFiles)
        {
            if (filename.EndsWith(".mp3") || filename.EndsWith(".wav"))
                filesAsList.Add(filename);
        }


        foreach (string filename in filesAsList)
        {
     
            //Create button and clean up filename
            var btn = (Button)Instantiate(button, transform.position, Quaternion.identity);
            string strippedFilename = filename.Substring(0, filename.Length - (24));
            //string strippedFilename = filename;

            btn.name = "B_" + strippedFilename;

            //Set the grid as the parent
            btn.transform.SetParent(grid.transform, false);

            //Set the text of our button to the filename (excluding the dir and the '.mp3')
            btn.GetComponentInChildren<Text>().text = strippedFilename;

            //Add an actionlistener to each button - coutner is the index in the list
            var n = counter;
            btn.onClick.AddListener(delegate { PlayOrPause(n); });

            counter++;
        }

        //Deactivate our dummy button
        GameObject.Find("B1").SetActive(false);
        GameObject.Find("Scrollbar").SetActive(false); 
        //button.enabled = false;


    }

    IEnumerator loadTrack(string file)
    {

        //string file_path = Application.dataPath + "/StreamingAssets/" + file;
        //using (Mp3FileReader reader = new Mp3FileReader(file_path))
        //{
        //    WaveFileWriter.CreateWaveFile(file_path, reader);
        //}

        string path = "file://" + Application.dataPath + "/StreamingAssets/" + file + ".mp3";
        print("Loading from streamingAsset" + path);

        request = new WWW(path);

        // Wait for download to complete
        yield return request;

        

        current = request.GetAudioClip(false, false);
    }


    void PlayOrPause(int index)
    {
        
        print(index + " <- index");
        //StartCoroutine(loadTrack(filename));

        var clip = (AudioClip) songlist[index];  

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
            if (isPlaying)
            {
                sound.Pause();
                isPlaying = !isPlaying;
            }

            else
            {
                sound.Play();
                isPlaying = !isPlaying;
            }

        }

        if (Input.GetKey("escape"))
            Application.Quit();

    }
}
