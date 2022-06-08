using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform exit;//Tansform �N���m
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
        enemy = GetComponent<Transform>();//�o�����ڭ̯�o��èϥ�enemy��scripts�Mcomponent��
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
        if (pointOnWay != null && !IsDie)//��ܦ��J�����s
        {
            navigationTime += Time.deltaTime;//Time.dealtatime�W�@�V��o�@�V���ɶ�
            if (navigationTime > navigationUpdate)
            {
                if (index < pointOnWay.Length)//�٨S�W�X�}�C�d��
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, pointOnWay[index].position, speed);//���C�Ӥ��~�I����
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, speed);//�����I����
                }
                navigationTime = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)//�P�_�I����Trigger�ɷ|�I�s, �T�w�Ϊk, �b�o��other�O��@���~�I�M���I�ϥ�
    {
        if (other.tag == "CheckPoint")//Unity���]�w��Tag
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
            //������I�R���Ӫ���
        }
        
    }
    
    public void Injured(int damage)
    {
        anim.Play("Hurt1");
        //Debug.Log("I Got Hurt");
        Health -= damage;
    }
}
