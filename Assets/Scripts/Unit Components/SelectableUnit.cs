using UnityEngine;
public class SelectableUnit : MonoBehaviour
{
    public bool selected = false;
    [SerializeField]
    private GameObject selectedIndicator;

    private void Start()
    {
        if (selectedIndicator == null)
        {
            selectedIndicator = Instantiate((GameObject)Resources.Load("SelectionIndicator"), gameObject.transform);
            ToggleIndicator();
        }
    }

    void Update()
    {
        ToggleIndicator();
    }

    void ToggleIndicator()
    {
        if (selected)
            selectedIndicator.SetActive(true);

        else if (selectedIndicator.activeInHierarchy)
            selectedIndicator.SetActive(false);
    }
}
