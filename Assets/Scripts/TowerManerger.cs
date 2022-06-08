using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerManerger : Singleton<TowerManerger>
{
    public TowerButton pressed { get; set; }
    private SpriteRenderer S;
    // Start is called before the first frame update
    void Start()
    {
        S = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))//���U�k�ƹ��������
        {
            UnablePicture();
            pressed = null;
        }
        if (Input.GetMouseButtonDown(0))//���U�ƹ������m
        {
            Vector2 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePoint, new Vector2(0, 0));
            if (MousePoint != null && hit.collider != null && hit.collider.tag == "BuildGround")
            {
                PlaceTower(hit);
            }
        }
        if (S.enabled)
        {
            followMouse();
        }
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        UnablePicture();
        if (pressed != null && GameManerger.Instance.SubMoney(pressed.TowerPrice))
        {
                hit.collider.tag = "AlreadyBuild";
            GameManerger.Instance.AudioSource.PlayOneShot(SoundManerger.Instance.BuildTower);
            GameObject newTower = Instantiate(pressed.Tower);
                newTower.transform.position = hit.transform.position;
                GameManerger.Instance.SetMessage("You spend " + pressed.TowerPrice + " dollars for a " + pressed.Tower.name + "!");
        }
        else
        {
            if(pressed!)
            GameManerger.Instance.SetMessage("You have no enough money!");
        }
    }

    public void selectedTower(TowerButton select)
    {
            pressed = select;
            EnablePicture(pressed.DragSprite);
    }
    private void followMouse()
    {
        Vector2 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���ઽ����S��position����, �_�h�|�X�@�I�pbug�ɭP�Ϥ���SortingLayer�۰ʦA����C����m
        S.transform.position = MousePoint;
    }
    public void EnablePicture(Sprite sprite)
    {
        S.enabled = true;
        S.sprite = sprite;
    }
    public void UnablePicture()
    {
        S.enabled = false;
    }
}
