using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Fukcing Level")]
    [SerializeField] private bool fukcingLevel = false;
    [SerializeField] private TextMeshProUGUI flevel_playerName;
    [SerializeField] private TextMeshProUGUI flevel_bossName;
    [SerializeField] private Camera flevel_camera;
    [SerializeField] private float flevel_zoomOutSpeed = 1;


    [Header("Main Menu")]
    [SerializeField] private bool mainMenu = false;
    [SerializeField] private Canvas[] menuCanvas;
    [SerializeField] private Canvas[] profileCanvas;
    [SerializeField] private Canvas[] electionsCanvas;
    [SerializeField] private Canvas[] daoCanvas;
    [SerializeField] private GameObject[] profileAnims;
    [SerializeField] private GameObject[] electionAnims;
    [SerializeField] private GameObject[] daoAnims;
    [SerializeField] private TextMeshProUGUI menu_playerName;
    [SerializeField] private GameObject menu_playerNameInputUI;
    [SerializeField] private GameObject menu_welcomePlayerUI;
    [SerializeField] private TMP_InputField menu_playerNameInput;
    [SerializeField] private GameObject[] levels;

    private int activeMenuCanvas;
    private int activeProfileCanvas;
    private int activeElectionsCanvas;
    private int activeDaoCanvas;
    private AudioManager audioManager;
    private bool levelStarted = false;

    void Start()
    {
        if (fukcingLevel)
        {
            // Get the name of the boss
            string levelBoss = "bossName_" + PlayerPrefs.GetFloat("completedLevel").ToString();

            // Display the player name
            if (PlayerPrefs.HasKey("playerName")) flevel_playerName.text = PlayerPrefs.GetString("playerName");
            else flevel_playerName.text = "Nameless Bastard";

            // Displaye the boss name
            if (PlayerPrefs.HasKey(levelBoss)) flevel_bossName.text = PlayerPrefs.GetString(levelBoss);
            else Debug.LogError("There is no boss called: " + levelBoss);
        }
        else if (mainMenu)
        {
            // Set names of the bosses
            PlayerPrefs.SetString("bossName_1", "Me");
            PlayerPrefs.SetString("bossName_2", "War");
            PlayerPrefs.SetString("bossName_3", "Inflation");
            PlayerPrefs.SetInt("unlockedLevel_1", 1);   // unlock the level 1 by default
            PlayerPrefs.Save();

            // PlayerPrefs.DeleteKey("playerName"); // Debug only

            CheckLevelAchievements();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fukcingLevel && levelStarted)
        {
            flevel_camera.orthographicSize += flevel_zoomOutSpeed * Time.deltaTime;

            // If we zoom out enough, finish the scene and return the main menu
            if (flevel_camera.orthographicSize >= 16)
            {
                audioManager.StopSFX("Heavenly");
                audioManager.SetBackgroundMusic(true);
                FindObjectOfType<LevelLoader>().LoadLevel("MenuScene");
            }
        }
    }

    //  ***** Fukcing Scene Methods *****   //
    public void LevelStartActions() {
        // Fix stuff that will run in every scene
        audioManager = FindObjectOfType<AudioManager>();
        levelStarted = true;

        if (fukcingLevel)
        {
            audioManager.PlaySFX("Heavenly");
        }
    }



    //  ***** Navigational Functions *****   //
    public void OpenMenuCanvas(int openCanvasNumber)
    {
        menuCanvas[activeMenuCanvas].gameObject.SetActive(false);   // Close the curren canvas
        menuCanvas[openCanvasNumber].gameObject.SetActive(true);    // Open the new one
        activeMenuCanvas = openCanvasNumber;                        // Save the new one as current

        // Start with the first canvas when opening the any menu canvas
        if (openCanvasNumber == 2) OpenProfileCanvas(0);            
        if (openCanvasNumber == 3) OpenElectionsCanvas(0);            
        if (openCanvasNumber == 4) OpenDaoCanvas(0);     
    }

    public void OpenProfileCanvas(int openCanvasNumber)
    {
        profileCanvas[activeProfileCanvas].gameObject.SetActive(false);     // Close the curren canvas
        profileCanvas[openCanvasNumber].gameObject.SetActive(true);         // Open the new one

        profileAnims[activeProfileCanvas].gameObject.SetActive(false);      // Close the current anim
        profileAnims[openCanvasNumber].gameObject.SetActive(true);          // Open the new one

        activeProfileCanvas = openCanvasNumber;                              // Save the new one as current
    }

    public void OpenElectionsCanvas(int openCanvasNumber)
    {
        electionsCanvas[activeElectionsCanvas].gameObject.SetActive(false);   // Close the curren canvas
        electionsCanvas[openCanvasNumber].gameObject.SetActive(true);    // Open the new one

        electionAnims[activeElectionsCanvas].gameObject.SetActive(false);      // Close the current anim
        electionAnims[openCanvasNumber].gameObject.SetActive(true);          // Open the new one

        activeElectionsCanvas = openCanvasNumber;                        // Save the new one as current
    }
    public void OpenDaoCanvas(int openCanvasNumber)
    {
        daoCanvas[activeDaoCanvas].gameObject.SetActive(false);   // Close the curren canvas
        daoCanvas[openCanvasNumber].gameObject.SetActive(true);    // Open the new one

        daoAnims[activeDaoCanvas].gameObject.SetActive(false);      // Close the current anim
        daoAnims[openCanvasNumber].gameObject.SetActive(true);          // Open the new one

        activeDaoCanvas = openCanvasNumber;                        // Save the new one as current
    }

    public void BackToMenuCanvas()
    {
        // Close all active canvases
        menuCanvas[activeMenuCanvas].gameObject.SetActive(false);
        profileCanvas[activeProfileCanvas].gameObject.SetActive(false);
        electionsCanvas[activeElectionsCanvas].gameObject.SetActive(false);
        daoCanvas[activeDaoCanvas].gameObject.SetActive(false);

        // Open the menu canvas
        menuCanvas[0].gameObject.SetActive(true);
        activeMenuCanvas = 0;
    }

    // Reset name and level achivements
    public void ResetOptions()
    {
        if (PlayerPrefs.HasKey("playerName")) Debug.Log("Player Name: " + PlayerPrefs.GetString("playerName"));
        else Debug.Log("No name");
        PlayerPrefs.DeleteKey("playerName");    // Delete player name

        // Delete all level achivements
        for (int i = 1; i <= 3; i++)
        {
            string levelName = "unlockedLevel_" + i.ToString();
            PlayerPrefs.DeleteKey(levelName);
        }

        PlayerPrefs.Save();
    }

    // Write player name from data into player prefs
    public void GetPlayerName()
    {
        if (PlayerPrefs.HasKey("playerName") && PlayerPrefs.GetString("playerName") != "") return;  // if the name already written, then don't execute writing code
        
        string playerNameEntry = menu_playerNameInput.text;

        PlayerPrefs.SetString("playerName", playerNameEntry); // save it
        PlayerPrefs.Save();
        menu_playerName.text = playerNameEntry;  // write it to text screen

        // open welcome text and close input text
        menu_welcomePlayerUI.SetActive(true);
        menu_playerNameInputUI.SetActive(false);
    }
    
    // Display updated player name
    private void DisplayPlayerName()
    {
        // If player name is already saved
        if (PlayerPrefs.HasKey("playerName") && PlayerPrefs.GetString("playerName") != "")
        {
            menu_playerName.text = PlayerPrefs.GetString("playerName");
            menu_welcomePlayerUI.SetActive(true);
        }
        else // If not, get it
        {
            menu_playerName.text = ""; // delete if there is any
            menu_playerNameInput.text = "";
            menu_playerNameInputUI.SetActive(true);
        }
    }

    // Check level achivements and update their unlock/lock functionality
    private void CheckLevelAchievements()
    {
        for (int i = 1; i <= 3; i++)
        {
            string levelName = "unlockedLevel_" + i.ToString();

            // if level has been unlocked
            if (PlayerPrefs.HasKey(levelName) && PlayerPrefs.GetInt(levelName) == 1)
            {
                // make its appearance "unlocked"
                levels[i - 1].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
                // make it interactable
                levels[i - 1].GetComponentInChildren<Button>().interactable = true;
            }
            else
            {
                // make its appearance "locked"
                levels[i - 1].GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0.3f);
                // make it NOT interactable
                levels[i - 1].GetComponentInChildren<Button>().interactable = false;

            }
        }
    }
}
