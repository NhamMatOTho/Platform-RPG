using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public List<InventoryItem> stash;
    public List<InventoryItem> equipment;

    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquimentSlot[] equimentSlots;
    private UI_StatSlot[] statSlots;

    [Header("Item cooldown")]
    private float lastTimeUseFlask;
    private float lastTimeUseArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipments;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equimentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquimentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItem();
    }

    private void AddStartingItem()
    {
        foreach (ItemData_Equipment item in loadedEquipments)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquiment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquiment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquiment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquiment, newItem);
        newEquiment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equimentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equimentSlots[i].slotType)
                    equimentSlots[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);


        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlots.Length)
        {
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requireMaterials[i].stackSize)
                {
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStastList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment EquipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                EquipedItem = item.Key;
        }

        return EquipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUseFlask = Time.time;
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUseArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUseArmor = Time.time;
            return true;
        }

        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemID in _data.equipmentID)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && loadedItemID == item.itemID)
                {
                    loadedEquipments.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentID.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemID);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item database")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}
