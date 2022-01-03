using UnityEngine;
public class SelectableUnit : MonoBehaviour
{
    public bool selected = false;
    [SerializeField]
    private GameObject selectedIndicator;

    void Update()
    {
        if (selected)
        {
            selectedIndicator.SetActive(true);
        }
        else
        {
            if (selectedIndicator.activeInHierarchy)
            {
                selectedIndicator.SetActive(false);
            } 
        }
    }
}
