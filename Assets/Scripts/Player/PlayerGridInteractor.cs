using UnityEngine;

public class PlayerGridInteractor : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(PlaceablesManager.Instance._grid.WorldToCell(transform.position));
    }
}
