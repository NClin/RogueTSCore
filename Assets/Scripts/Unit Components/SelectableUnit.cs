using UnityEngine;
public class SelectableUnit : MonoBehaviour
{
    public bool selected = false;

    // Implement selection sprite and highlighting here.


    void Update()
    {
        if (selected)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

}
