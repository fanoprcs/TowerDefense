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
        if (Input.GetMouseButtonDown(1))//按下右滑鼠取消選取
        {
            UnablePicture();
            pressed = null;
        }
        if (Input.GetMouseButtonDown(0))//按下滑鼠左鍵放置
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
        Vector2 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);//不能直接用S的position等於, 否則會出一點小bug導致圖片的SortingLayer自動再比較低的位置
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
