using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;
    [SerializeField]
    private Sprite dragSprite;
    [SerializeField]
    private int towerPrice;
    public GameObject Tower
    {
        get { return tower; }
    }
    public Sprite DragSprite
    {
        get { return dragSprite; }
    }
    public int TowerPrice
    {
        get { return towerPrice; }
    }
}
