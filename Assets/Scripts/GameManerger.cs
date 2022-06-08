using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStats
{
    next, play, gameover, win
}
public class GameManerger : Singleton<GameManerger>
{
    [SerializeField]
    private int totalWaves;//�`�@���X�i

    [SerializeField]
    private Text totalMoneyLbl;//���ҤW������
    [SerializeField]
    private Text currentWaves;//��e�ĴX�i
    [SerializeField]
    private Text PlayBtnLbl;//���NextWave���������s
    [SerializeField]
    private Button playBtn;//�s����Button�����s
    [SerializeField]
    private Text CastleLifeLbl;//�ѤU�X���R
    [SerializeField]
    private Text remainEnemy;
    [SerializeField]
    private int MaxCaslteLife = 5;

    [SerializeField]
    private Text MessageLbl;//�H�����
    [SerializeField]
    private Message message;//�H�����

    private int castleLife = 5;
    private int WaveNum = 1;
    private int money = 10;//��ƪ�����
    private int totalInjured = 0;//�`�@�D���֤F�X���R
    private int roundInjured = 0;//��e�D���֤F�X���R
    private int totalKilled = 0;
    //private int HowMuchEnemyToSpawn = 0;//�ӽ��ͦ��h�ּĤH������
    private gameStats curStats;

    private AudioSource audioSource;//����
    public int waveNum
    {
        get { return WaveNum; }
        set
        {
            WaveNum = value;
            currentWaves.text = "Wave: " + WaveNum;
        }
    }
    public int CastleLife
    {
        get { return castleLife; }
        set
        {
            castleLife = value;
            CastleLifeLbl.text = "CASTLE HP: " + CastleLife;
        }
    }
    public int TotalInjured
    {
        get { return totalInjured; }
        set { totalInjured = value; }
    }
    public int RoundInjured
    {
        get { return roundInjured; }
        set { 
            roundInjured = value;
            remainEnemy.text = "Remian Enemy: " + (TotalEnemy - TotalKilled - RoundInjured);
        }
    }
    public int TotalKilled
    {
        get { return totalKilled; }
        set { totalKilled = value;
            remainEnemy.text = "Remian Enemy: " + (TotalEnemy - TotalKilled - RoundInjured);
        }
    }
    [SerializeField]// unity���ت�, ���H���ܼƨ��«O��private,���O��bunity�����ç�復
    private GameObject SpawnPoint;
    [SerializeField]
    private GameObject[] CheckPoint;
    [SerializeField]
    private GameObject[] BuildGround;
    [SerializeField]
    private GameObject[] Enemys;
    [SerializeField]

    //private int EnemyPerSpawn = 2;
    private int MaxEnemyOnScreen;
    private int totalEnemy;//��e�^�X�̦h���h�ּĤH
    private int count = 0;

    private bool gameover = false;
    private int enemyCount = 0;
    public bool Gameover
    {
        get { return gameover; }
    }
    public int TotalEnemy
    {
        get { return totalEnemy; }
        set { totalEnemy = value;
            remainEnemy.text = "Remian Enemy: " + TotalEnemy;
        }
    }
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            totalMoneyLbl.text = money.ToString();
        }
    }
    public AudioSource AudioSource
    {
        get { return audioSource; }
    }

    public int EnemyCount
    { 
        get { return enemyCount; }
        set { enemyCount = value; }
    }
    private List<Enemy> enemyList = new List<Enemy>();
    public List<Enemy> EnemyList
    {
        get { return enemyList; }
        set { EnemyList = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        setCurrentState();
        showMenu();

        for (int i = 0; i < CheckPoint.Length; i++)
            Instantiate(CheckPoint[i]);

    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator spawn()
    {
        if (count < TotalEnemy && gameover == false)
        {
            int x = Random.Range(1, TotalEnemy / 4 + 3);
            for (int i = 0; i < x; i++)
                if (EnemyCount < MaxEnemyOnScreen)
                {
                    SpawnEnemy();
                    count++;
                    if (count == TotalEnemy)
                        break;
                }
            yield return new WaitForSeconds(Random.Range(2, 8));//�ʤְh���t��k
            StartCoroutine(spawn());
        }
    }
    void SpawnEnemy()
    {
        GameObject NewEnemy;
        if (waveNum <= 2)
        {
            NewEnemy = Instantiate(Enemys[0]); //as Enemys GameObject;
            NewEnemy.transform.position = SpawnPoint.transform.position;
        }
        else if (waveNum <= 5)
        {
            int x = Random.Range(1, 201);
            if (x <= 450/waveNum)
            NewEnemy = Instantiate(Enemys[0]);
            else
                NewEnemy = Instantiate(Enemys[1]);
            NewEnemy.transform.position = SpawnPoint.transform.position;
        }
        else if (waveNum <= 10)
        {
            int ran = Random.Range(1, 301);
            if (ran < 200 / waveNum)
            {
                NewEnemy = Instantiate(Enemys[0]); 
            }
            else
            {
                int k = 50 * (waveNum-6);
                int cur = Random.Range(1, 301 + k);
                if(cur <= 800/waveNum)
                    NewEnemy = Instantiate(Enemys[1]);
                else
                    NewEnemy = Instantiate(Enemys[2]);
            }
            NewEnemy.transform.position = SpawnPoint.transform.position;
        }
        else
        {
            int ran = Random.Range(1, 11);
            if (ran == 1)
            {
                NewEnemy = Instantiate(Enemys[0]);
            }
            else
            {
                int cur = Random.Range(1, 3);
                NewEnemy = Instantiate(Enemys[cur]);
            }
            NewEnemy.transform.position = SpawnPoint.transform.position;
        }
        enemyCount++;
    }
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnRegisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);//���F����Enemy��list���~, �٭n��unity�W�w�ͦ�������P��
    }

    public void DestroyAllEnemies()
    {
        foreach (Enemy enemy in EnemyList)
            Destroy(enemy.gameObject);
        EnemyList.Clear();
    }
    public void DestroyAllTower()
    {
        foreach (GameObject ground in BuildGround)//�Q��RaycastHit, �q�i�y���a�O�P�_���Ǧa�O��hit���, �úR����M���]�a�O���A
        {
            if (ground.tag == "AlreadyBuild")
            {
                RaycastHit2D hit = Physics2D.Raycast(ground.transform.position, new Vector2(0, 0), 0, 1 << LayerMask.NameToLayer("Collection"));
                //Debug.Log("Tag: " + hit.collider.tag);
                if (hit.collider.tag == "Tower")
                {
                    Destroy(hit.collider.gameObject);
                }
                ground.tag = "BuildGround";
            }
        }
    }
    public void AddMoney(int value)
    {
        Money += value;
    }
    public bool SubMoney(int value)
    {
        if (Money >= value)
        {
            Money -= value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void showMenu()
    {
        switch (curStats)
        {

            case gameStats.next:
                PlayBtnLbl.text = "Next Wave";
                break;
            case gameStats.play:
                PlayBtnLbl.text = "Start to Play";
                break;
            case gameStats.win:
                PlayBtnLbl.text = "Congraduration!!";
                break;
            case gameStats.gameover:
                PlayBtnLbl.text = "Play Again!!";
                break;
        }

    }
    public void ButtonPressed()//���U�}�l���s��, �ͦ��ĤH
    {
        switch (curStats)
        {
            case gameStats.next:
                waveNum += 1;
                TotalEnemy += waveNum;
                Money += 3;
                SetMessage("Wave clear, You get 3 coins as reward!!");
                break;
            default://play,playagain,�C���ӧQ, ���ݩ�o��, �ҥH��l��
                TotalEnemy = 3;
                TotalInjured = 0;
                Money = 10;
                CastleLife = MaxCaslteLife;
                waveNum = 1;
                DestroyAllTower();
                gameover = false;
                break;
        }
        //���ެƻ򪬪p���n���檺
        DestroyAllEnemies();
        enemyCount = 0;
        audioSource.PlayOneShot(SoundManerger.Instance.NewGame);
        TotalKilled = 0;//�o�^�X���ļ�
        RoundInjured = 0;
        count = 0;
        MaxEnemyOnScreen = TotalEnemy / 3 + 5;
        playBtn.gameObject.SetActive(false);//true�Mfalse�M�w�n���n�ثe�b�ù��W���;
        StartCoroutine(spawn());
    }
    public void IsWaveOver()
    {
        if (gameover == false)
        {  //�ˬd��e��stats
            if (CastleLife <= 0)
            {
                setCurrentState();
                showMenu();
                AudioSource.PlayOneShot(SoundManerger.Instance.Gameover);
                gameover = true;
                playBtn.gameObject.SetActive(true);//true�Mfalse�M�w�n���n�ثe�b�ù��W���;
            }
            else if (RoundInjured + TotalKilled == TotalEnemy)//��e�^�X
            {
                setCurrentState();
                showMenu();
                playBtn.gameObject.SetActive(true);//true�Mfalse�M�w�n���n�ثe�b�ù��W���;
            }
        }

    }
    public void SetMessage(string str)//�]�w�H��
    {
        MessageLbl.text = str + "\n" + MessageLbl.text;
    }

    public void setCurrentState()
    {
        //Debug.Log("Wave: " + waveNum + ", total wave: " + totalWaves);
        if (waveNum == 1 && TotalKilled + RoundInjured == 0 && TotalInjured == 0)
        { curStats = gameStats.play; }
        else if (TotalInjured >= MaxCaslteLife)
        { curStats = gameStats.gameover; }
        else if (waveNum >= totalWaves)
        { curStats = gameStats.win; }
        else
        {
            curStats = gameStats.next;
        }
    }
}

