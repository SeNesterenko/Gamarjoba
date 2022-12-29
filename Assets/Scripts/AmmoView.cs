using TMPro;
using UnityEngine;

public class AmmoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;

    public void RefreshAmmo(int ammoCount)
    {
        _ammoText.text = ammoCount.ToString();
    }
}