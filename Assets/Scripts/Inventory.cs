using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot m_ItemSlotPrefab;
    public Transform m_InventoryTransform;
    public List<Item> m_Items;
    [HideInInspector] public Action onInventoryChangedEvent;
    /// <summary>
    /// Reset item scriptable objects to default values on game start
    /// </summary>
    public void ResetItemsToDefaultValues()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i].m_Amount = 1;
            //Player default items
            if (m_Items[i].m_Data.m_Name.Contains("3"))
            {
                m_Items[i].m_AvailableToBuy = false;
                m_Items[i].m_Equiped = true;
            }
            //Rest of the items
            else
            {
                m_Items[i].m_AvailableToBuy = true;
                m_Items[i].m_Equiped = false;
            }
        }
    }
    /// <summary>
    /// Sort inventory by item type
    /// </summary>
    public void SortInventory()
    {
        m_Items = m_Items.OrderBy(x => x.m_Data.m_ItemType).ToList();
    }
    /// <summary>
    /// Return item by ID
    /// </summary>
    /// <param name="a_ItemID"></param>
    /// <returns></returns>
    public Item GetItem(int a_ItemID)
    {
        foreach (Item item in m_Items) 
        {
            if(item.m_Data.m_ItemID == a_ItemID)
                return item;
        }
        return null;
    }
    private void AddItem(Item a_Item, int a_Amount)
    {
        if (!m_Items.Contains(a_Item))
        {
            m_Items.Add(a_Item);
        }
        else 
            m_Items[m_Items.IndexOf(a_Item)].Add(a_Amount);
    }
    private void RemoveItem(Item a_Item, int a_Amount)
    {
        m_Items[m_Items.IndexOf(a_Item)].Remove(a_Amount);
        if(m_Items[m_Items.IndexOf(a_Item)].m_Amount <= 0)
            m_Items.Remove(a_Item);
    }
    public void AddItems(Item[] a_Items, int[] a_Amounts)
    {
        for (int i = 0; i < a_Items.Length; i++)
        {
            AddItem(a_Items[i], a_Amounts[i]);
        }
    }
    public void RemoveItems(Item[] a_Items, int[] a_Amounts)
    {
        for (int i = 0; i < a_Items.Length; i++)
        {
            RemoveItem(a_Items[i], a_Amounts[i]);
        }
    }
    public void OpenInventory()
    {
        GameManager.instance.PlaySound(1);
        GameManager.instance.m_Player.m_Inventory.m_OpenButton.interactable = false;
        GameManager.instance.m_Player.m_Interacting = true;
        GameManager.instance.m_Player.StopPlayer();
        m_InventoryTransform.gameObject.SetActive(true);
        GameManager.instance.CameraZoomInToPlayer();
        m_InventoryTransform.DOScale(Vector3.one, 0.5f);
    }
    public void CloseInventory()
    {
        GameManager.instance.m_Player.m_Inventory.m_OpenButton.interactable = true;
        m_InventoryTransform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => { 
                m_InventoryTransform.gameObject.SetActive(false);
            });
        GameManager.instance.CameraResetZoom();
        GameManager.instance.m_Player.m_Interacting = false;
    }
}
