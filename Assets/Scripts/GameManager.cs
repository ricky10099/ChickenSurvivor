using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{    public enum MODE
    {
        TITLE, //タイトル
        PLAY, //プレイ中
        OVER, //ゲームオーバー
        TITLE_TO_PLAY,
        SELECT_MODE,
    }
    static public MODE GameMode; //ゲームの状態

    public enum DIFFICULTY
    {
        EASY,
        HARD,
    }
    static public DIFFICULTY difficulty;
    static public int WormLevel;

    [SerializeField] static public int maxWorm = 5;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtWorm;
    [SerializeField] Text txtTimer;
    [SerializeField] Text txtMsg;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnMode;
    [SerializeField] Button btnEat;
    [SerializeField] Button btnRun;
    [SerializeField] Image imgRank;
    [SerializeField] Text[] txtRank = new Text[5];
    [SerializeField] GameObject dirLightObj;
    [SerializeField] AudioClip easyTitleBGM;
    [SerializeField] AudioClip easyPlayBGM;
    [SerializeField] AudioClip hardTitleBGM;
    [SerializeField] AudioClip hardPlayBGM;
    [SerializeField] AudioClip finishBGM;
    [SerializeField] GameObject rain;
    [SerializeField] GameObject[] fire = new GameObject[5];
    [SerializeField] GameObject[] thunder = new GameObject[5];

    AudioSource audioSrc;

    Light dirLight;
    Vector3 easyLight = new Vector3(50, -30, 0);
    Vector3 hardLight = new Vector3(-4, -30, 0);
    Color32 easyDirColor = new Color32(255, 244, 214, 0);
    Color32 hardDirColor = new Color32(129, 0, 188, 0);
    Color32 easyAmbientColor = new Color32(146, 255, 254, 0);
    Color32 hardAmbientColor = new Color32(46, 61, 152, 0);

    float[] rank = new float[5];
    float elapsed = 0.0f;
    float clearTime;
    int wormCount = 0;
    int treeIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("EASYRank0"))
        {
            for(int i = 0; i < rank.Length; i++)
            {
                PlayerPrefs.SetFloat("EASYRank" + i, float.MaxValue);
            }
        }
        if (!PlayerPrefs.HasKey("HARDRank0"))
        {
            for (int i = 0; i < rank.Length; i++)
            {
                PlayerPrefs.SetFloat("HARDRank" + i, float.MaxValue);
            }
        }

        difficulty = DIFFICULTY.EASY;
        dirLight = dirLightObj.GetComponent<Light>();
        SetTitle();
    }

    void SetTitle()
    {
        if(difficulty == DIFFICULTY.EASY) { 
            audioSrc.clip = easyTitleBGM;
            rain.SetActive(false);
        }
        else
        {
            audioSrc.clip = hardTitleBGM;
            rain.SetActive(true);
        }
        audioSrc.loop = true;
        audioSrc.Play();
        GameMode = MODE.TITLE;
        txtTitle.enabled = true;
        btnStart.GetComponentInChildren<Text>().text = "START";
        btnMode.GetComponentInChildren<Text>().text = "MODE";
        btnStart.gameObject.SetActive(true);
        btnMode.gameObject.SetActive(true);

        txtWorm.enabled = false;
        txtTimer.enabled = false;
        txtMsg.enabled = false;
        btnEat.gameObject.SetActive(false);
        btnRun.gameObject.SetActive(false);
        imgRank.enabled = false;
        for(int i = 0; i < 5; i++)
        {
            txtRank[i].enabled = false;
        }
    }

    public void GameStart()
    {        
        GameMode = MODE.PLAY;
        WormLevel = 1;
        wormCount = 0;
        elapsed = 0;
        txtTitle.enabled = false;
        btnStart.gameObject.SetActive(false);
        btnMode.gameObject.SetActive(false);

        txtWorm.enabled = true;
        txtWorm.text = "Worm : 0 / " + maxWorm;
        txtTimer.enabled = true;
        txtTimer.text = "Time : 0.00s";
        btnEat.gameObject.SetActive(true);
        btnRun.gameObject.SetActive(true);
        txtMsg.text = "START";
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMode == MODE.PLAY) {
            elapsed += Time.deltaTime;
            txtTimer.text = "Time : " + elapsed.ToString("f2") + "s";

            if(elapsed > 1.5f)
            {
                txtMsg.enabled = false;
            }

            if(difficulty == DIFFICULTY.HARD)
            {
                int time = (int)elapsed;
                if (time > 5)
                {
                    //treeIdx = UnityEngine.Random.Range(0, 4);
                    LightningStrike();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(gameObject.scene.name);
        }
    }

    public void WormEaten()
    {
        txtWorm.text = "Worm : " + ++wormCount + " / " + maxWorm;
        WormLevel++;
        if(wormCount >= maxWorm)
        {
            GameFinish();
        }
    }

    void GameFinish()
    {
        audioSrc.clip = finishBGM;
        audioSrc.loop = false;
        audioSrc.Play();

        GameMode = MODE.OVER;
        txtMsg.text = "FINISH";
        txtMsg.enabled = true;
        ShowRank();
        btnStart.GetComponentInChildren<Text>().text = "TITLE";
        btnStart.gameObject.SetActive(true);

        btnRun.gameObject.SetActive(false);
        btnEat.gameObject.SetActive(false);
    }

    void ShowRank()
    {
        imgRank.enabled = true;
        clearTime = elapsed;

        for (int i = 0; i < rank.Length; i++)
        {
            rank[i] = PlayerPrefs.GetFloat(difficulty + "Rank" + (i));
            txtRank[i].enabled = true;
        }

        int newRank = -1;
        for (int i = rank.Length - 1; i >= 0; i--)
        {
            if (clearTime < rank[i] && clearTime != 0.0f)
            {
                newRank = i;
            }
        }

        if (newRank != -1)
        {
            for (int i = rank.Length - 1; i > newRank; i--)
            {
                rank[i] = rank[i - 1];
            }
            rank[newRank] = clearTime;
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetFloat(difficulty + "Rank" + (i), rank[i]);
            }
        }

        for (int i = 0; i < rank.Length; i++)
        {
            if (rank[i] == float.MaxValue)
            {
                txtRank[i].text = "0.00s";
            }
            else
            {
                txtRank[i].text = rank[i].ToString("f2") + "s";
            }
        }
    }

    void PlayTransition()
    {
        if (difficulty == DIFFICULTY.EASY)
        {
            audioSrc.clip = easyPlayBGM;
        }
        else
        {
            audioSrc.clip = hardPlayBGM;
        }

        audioSrc.Play();
        GameMode = MODE.TITLE_TO_PLAY;
        txtTitle.enabled = false;
        btnStart.gameObject.SetActive(false);
        btnMode.gameObject.SetActive(false);

        txtMsg.text = "READY";
        txtMsg.enabled = true;

        Invoke("GameStart", 1);
    }

    public void StartButton()
    {
        switch (GameMode)
        {
            case MODE.TITLE:
                PlayTransition();
                break;
            case MODE.OVER:
                SetTitle();
                break;
            case MODE.SELECT_MODE:
                ChangeMode(1);
                break;
        }
    }

    public void ModeButton()
    {
        switch (GameMode)
        {
            case MODE.TITLE:
                btnMode.GetComponentInChildren<Text>().text = "HARD";
                btnStart.GetComponentInChildren<Text>().text = "EASY";
                GameMode = MODE.SELECT_MODE;
                break;
            case MODE.SELECT_MODE:
                ChangeMode(2);
                break;
        }

    }

    void ChangeMode(int mode)
    {
        //GameMode = MODE.TITLE;
        //btnMode.GetComponentInChildren<Text>().text = "MODE";
        //btnStart.GetComponentInChildren<Text>().text = "START";

        switch (mode)
        {
            case 1:
                difficulty = DIFFICULTY.EASY;
                RenderSettings.ambientLight = easyAmbientColor;
                dirLight.color = easyDirColor;
                dirLightObj.transform.rotation = Quaternion.Euler(easyLight);
                break;
            case 2:
                difficulty = DIFFICULTY.HARD;
                RenderSettings.ambientLight = hardAmbientColor;
                dirLight.color = hardDirColor;
                dirLightObj.transform.rotation = Quaternion.Euler(hardLight);
                break;
        }

        SetTitle();
    }

    void LightningStrike()
    {
        while (thunder[treeIdx].activeSelf)
        {
            treeIdx = UnityEngine.Random.Range(0, 4);
        }

        thunder[treeIdx].SetActive(true);
        Invoke("BurnTree", 0.3f);
    }

    void BurnTree()
    {
        fire[treeIdx].SetActive(true);
    }
}
