using UnityEngine;

public class SpotScript : MonoBehaviour
{
    public SpriteRenderer ox;

    [SerializeField]
    private int cell;


    public void ButtonClick()
    {
        if (GameManager.Instance.isEnd == true)
            return;

        if(ox.sprite == null)
        {
            int mark = GameManager.Instance.MarkOX(cell);
            SetOX(mark);
        }
    }

    private void SetOX(int mark)
    {
        if(mark == (int)Sign.O)
            ox.sprite = GameManager.Instance.spriteO;
        else if(mark == (int)Sign.X)
            ox.sprite = GameManager.Instance.spriteX;
    }
}
