using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform exit;//Tansform 代表位置
    [SerializeField]
    private Transform[] pointOnWay;
    [SerializeField]
    private int health;
    [SerializeField]
    private int reward;
    private float speed;
    private int index = 0;
    private Transform enemy;
    private float navigationTime = 0;
    public bool IsDie = false;
    private float navigationUpdate = 0.01f;
    private Animator anim;
    public int Health
    {
        get { return health; }
        set
        {
            if (value <= 0)
            {
                health = 0;
                //make a animation of die
                
                //enemy die

                if (IsDie == false)
                {
                    anim.SetTrigger("didDie");
                    GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.Death);
                    GameManerger.Instance.TotalKilled++;
                    GameManerger.Instance.EnemyCount--; GameManerger.Instance.AddMoney(reward);
                    GameManerger.Instance.IsWaveOver();
                }
                    IsDie = true;
                
                //GameManerger.Instance.UnRegisterEnemy(this);
            }
            else
            {
                GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.Hit);
                health = value; 
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();//這行讓我們能得到並使用enemy的scripts和component等
        GameManerger.Instance.RegisterEnemy(this);
        anim = GetComponent<Animator>();
        float x = (GameManerger.Instance.TotalEnemy / 1000f) + 0.010f;
        if (x >= 0.02f)
            x /= 2f;
        speed = Random.Range(0.009f, x);
    }

    // Update is called once per frame
    void Update()
    {
        if (pointOnWay != null && !IsDie)//表示有遇到轉彎
        {
            navigationTime += Time.deltaTime;//Time.dealtatime上一幀到這一幀的時間
            if (navigationTime > navigationUpdate)
            {
                if (index < pointOnWay.Length)//還沒超出陣列範圍
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, pointOnWay[index].position, speed);//往每個中繼點移動
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, speed);//往終點移動
                }
                navigationTime = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)//判斷碰撞到Trigger時會呼叫, 固定用法, 在這邊other是當作中繼點和終點使用
    {
        if (other.tag == "CheckPoint")//Unity中設定的Tag
        {
            index++;
        }

        else if (other.tag == "Finish")
        {
            if (GameManerger.Instance.Gameover == false)
            {
                GameManerger.Instance.EnemyCount--;
                GameManerger.Instance.RoundInjured++;
                GameManerger.Instance.TotalInjured++;
                GameManerger.Instance.CastleLife--;
            }
            GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.HitCastle);
            GameManerger.Instance.SetMessage("Castle has suffered 1 point damage.");
            GameManerger.Instance.UnRegisterEnemy(this);
            GameManerger.Instance.IsWaveOver();
            //走到終點摧毀該物件
        }
        
    }
    
    public void Injured(int damage)
    {
        anim.Play("Hurt1");
        //Debug.Log("I Got Hurt");
        Health -= damage;
    }
}
