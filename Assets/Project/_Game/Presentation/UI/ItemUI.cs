using System.Collections.Generic;
using JetBrains.Annotations;
using MoveStopMove.Core.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveStopMove.Presentation.UI
{
    public class ItemUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform itemContext;

        [Header("Info")]
        [SerializeField] private Image iconItem;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemBuff;
        [CanBeNull] [SerializeField] private TextMeshProUGUI itemSkill;
        [CanBeNull] [SerializeField] private TextMeshProUGUI itemMaxRange;
        [SerializeField] private TextMeshProUGUI itemPrice;

        private List<WeaponData> m_unlockedWeapons = new();
        private List<WeaponData> m_lockedWeapons = new();

        private List<PantData> m_unlockedPants = new();
        private List<PantData> m_lockedPants = new();

        private List<HairData> m_unlockedHairs = new();
        private List<HairData> m_lockedHairs = new();

        private List<CustomData> m_unlockedCustoms = new();
        private List<CustomData> m_lockedCustoms = new();

        private void DisplayItem<T>(List<T> unlockedItems, List<T> lockedItems) where T : ScriptableObject
        {
            foreach (var item in unlockedItems)
            {
                var itemUI = Instantiate(itemPrefab, itemContext);
                //itemUI.GetComponent<ItemUI>().Setup(item, true);
            }

            foreach (var item in lockedItems)
            {
                var itemUI = Instantiate(itemPrefab, itemContext);
                //itemUI.GetComponent<ItemUI>().Setup(item, false);
            }
        }

        /*private void SetupInfo<T>(T item, bool isUnlocked) where T : ScriptableObject
        {
            iconItem = item.
        }*/
    }
}