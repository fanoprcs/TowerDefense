using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private AttackTool attackTool;
    private Enemy target = null;
    private float attackCounter;
    private bool isAttack;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if (target == null || target.IsDie)
        {
            Enemy nearEnemy = NearestEnemy();
            if (nearEnemy != null/* && Vector2.Distance(this.transform.position, nearEnemy.transform.position) <= attackRadius*/)
            {
                target = nearEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttack = true;
                attackCounter = attackSpeed;
            }
            else
                isAttack = false;

            if (Vector2.Distance(this.transform.localPosition, target.transform.localPosition) > attackRadius)
                target = null;
            if (isAttack == true)
            {
                Attack();

            }
        }
    }
    public void Attack()
    {
        AttackTool newtool = Instantiate(attackTool) as AttackTool;
        newtool.transform.localPosition = this.transform.localPosition;
        if (newtool.Type == AttackType.arrow)
            GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.Arrow);
        if (newtool.Type == AttackType.rock)
            GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.Rock);
        if (newtool.Type == AttackType.fireball)
            GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.Fireball);
        if (target == null)
        {
            Destroy(newtool);
        }
        else
        {
            StartCoroutine(Shoot(newtool));
        }
    }
    IEnumerator Shoot(AttackTool newtool)
    {

        while (target != null && newtool != null)//�ݭntarget������null����]�O�]��yield return �O�v�V�i�檺, �ҥH�C�@�V��update���|����, target�|�ܦ�null
        {
            var dir = target.transform.localPosition - this.transform.localPosition;//var �|���᭱���O���ܼƾ�����, �b����@Vector3
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;//�b��var��@float, �᭱Atan2(y,x)�|�o����I(�b�����@���I)��(x,y)����, �A���W�Ѽ�Rad2Deg�N�|�ܦ�����
            newtool.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);

            if (newtool.Type == AttackType.arrow)
                newtool.transform.localPosition = Vector3.MoveTowards(newtool.transform.localPosition, target.transform.localPosition, 5f * Time.deltaTime);
            if (newtool.Type == AttackType.rock)
                newtool.transform.localPosition = Vector3.MoveTowards(newtool.transform.localPosition, target.transform.localPosition, 3f * Time.deltaTime);
            if (newtool.Type == AttackType.fireball)
                newtool.transform.localPosition = Vector3.MoveTowards(newtool.transform.localPosition, target.transform.localPosition, 3f * Time.deltaTime);
            yield return 0;
            if (target != null && newtool != null && Vector2.Distance(newtool.transform.localPosition, target.transform.localPosition) <= 0.5)
            {
                target.Injured(attackTool.AttackDamage);
                Destroy(newtool.gameObject);
                break;
            }
        }
        if (target == null && newtool != null)
        {
            Destroy(newtool.gameObject);
        }

    }
    private float getTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
            thisEnemy = NearestEnemy();
        if (thisEnemy == null)
            return 0f;
        return Mathf.Abs(Vector2.Distance(this.transform.localPosition, thisEnemy.transform.localPosition));
    }
    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> emeInRange = new List<Enemy>();
        int alter = 1;
        foreach (Enemy enemy in GameManerger.Instance.EnemyList)
        {
            if (enemy.IsDie == false && Vector2.Distance(this.transform.localPosition, enemy.transform.localPosition) < attackRadius)
            {
                alter = 0;
                emeInRange.Add(enemy);
            }
        }
        if (alter == 0)
            return emeInRange;
        else
            return null;
    }
    private Enemy NearestEnemy()
    {
        Enemy nearest = null;
        double smallestDistance = double.PositiveInfinity;
        List<Enemy> tmp = GetEnemiesInRange();
        if (tmp != null)
            foreach (Enemy enemy in tmp)
            {
                double dis = Vector2.Distance(this.transform.localPosition, enemy.transform.localPosition);
                if (dis < smallestDistance)
                {
                    smallestDistance = dis;
                    nearest = enemy;
                }
            }
        return nearest;
    }
}
