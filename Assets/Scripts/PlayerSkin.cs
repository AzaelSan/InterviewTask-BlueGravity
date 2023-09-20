using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage the skin of the player
/// </summary>
public class PlayerSkin : MonoBehaviour
{
    [Header("Clothes Handles")]
    public SpriteRenderer[] m_Head;
    public SpriteRenderer[] m_Body;
    public SpriteRenderer[] m_Hands;
    public SpriteRenderer[] m_Pants;
    public SpriteRenderer[] m_Shoes;

    [HideInInspector]
    public bool m_SkinEquiped;
    private List<Item> m_CurrentSkinItems;
    private List<Item> m_TempSkinItems;
    public ItemSlot[] m_TempItemSlots = new ItemSlot[5]; //Slots of selected items to equip
    private List<Sprite> m_CurrentSkinSprites;

    private void Awake()
    {
        m_CurrentSkinSprites = new List<Sprite>();
        m_CurrentSkinItems = new List<Item>();
        m_TempSkinItems = new List<Item>();
    }

    private void Start()
    {
        int _count = GameManager.instance.m_Player.m_Inventory.m_Items.Count;
        for (int i = 0; i < _count; i++)
        {
            GameManager.instance.m_Player.m_Inventory.m_Items[i].m_Equiped = true;
            m_TempSkinItems.Add(GameManager.instance.m_Player.m_Inventory.m_Items[i]);
        }
        m_CurrentSkinItems.AddRange(m_TempSkinItems);
        SaveCurrentSkin();
    }

    /// <summary>
    /// Preview clothes on player
    /// </summary>
    /// <param name="a_Item">Item to equip</param>
    public void SetClothes(Item a_Item)
    {
        if (a_Item == null) return;
        int spriteCount = a_Item.m_Data.m_ClothesSprite.Length;
        switch (a_Item.m_Data.m_ItemType) 
        {
            case ItemType.HEAD:
                for (int i = 0; i < spriteCount; i++)
                {
                    m_Head[i].sprite = a_Item.m_Data.m_ClothesSprite[i];
                }
                m_TempItemSlots[0].EnableDefaultButtonSprite();
                m_TempItemSlots[0] = GameManager.instance.m_Player.m_Inventory.GetItemButtonByID(a_Item.m_Data.m_ItemID);
                m_TempSkinItems[0] = a_Item;
                break;
            case ItemType.BODY:
                for (int i = 0; i < spriteCount; i++)
                {
                    m_Body[i].sprite = a_Item.m_Data.m_ClothesSprite[i];
                }
                m_TempItemSlots[1].EnableDefaultButtonSprite();
                m_TempItemSlots[1] = GameManager.instance.m_Player.m_Inventory.GetItemButtonByID(a_Item.m_Data.m_ItemID);
                m_TempSkinItems[1] = a_Item;
                break;
            case ItemType.HANDS:
                for (int i = 0; i < spriteCount; i++)
                {
                    m_Hands[i].sprite = a_Item.m_Data.m_ClothesSprite[i];
                }
                m_TempItemSlots[2].EnableDefaultButtonSprite();
                m_TempItemSlots[2] = GameManager.instance.m_Player.m_Inventory.GetItemButtonByID(a_Item.m_Data.m_ItemID);
                m_TempSkinItems[2] = a_Item;
                break;
            case ItemType.PANTS:
                for (int i = 0; i < spriteCount; i++)
                {
                    m_Pants[i].sprite = a_Item.m_Data.m_ClothesSprite[i];
                }
                m_TempItemSlots[3].EnableDefaultButtonSprite();
                m_TempItemSlots[3] = GameManager.instance.m_Player.m_Inventory.GetItemButtonByID(a_Item.m_Data.m_ItemID);
                m_TempSkinItems[3] = a_Item;
                break;
            case ItemType.SHOES:
                for (int i = 0; i < spriteCount; i++)
                {
                    m_Shoes[i].sprite = a_Item.m_Data.m_ClothesSprite[i];
                }
                m_TempItemSlots[4].EnableDefaultButtonSprite();
                m_TempItemSlots[4] = GameManager.instance.m_Player.m_Inventory.GetItemButtonByID(a_Item.m_Data.m_ItemID);
                m_TempSkinItems[4] = a_Item;
                break;
        }
    }
    /// <summary>
    /// Equip current skin selection to the player
    /// </summary>
    public void EquipCurrentSkin()
    {
        SaveCurrentSkin();
        for (int i = 0; i < m_CurrentSkinItems.Count; i++)
        {
            m_CurrentSkinItems[i].m_Equiped = false;
            m_TempSkinItems[i].m_Equiped = true;
        }
        m_CurrentSkinItems.Clear();
        m_CurrentSkinItems.AddRange(m_TempSkinItems);
        m_SkinEquiped = true;
        GameManager.instance.m_Player.m_Inventory.UpdateItemsButtonsState();
    }
    /// <summary>
    /// Save current skin, this way its posible to be restored
    /// </summary>
    public void SaveCurrentSkin()
    {
        m_CurrentSkinSprites.Clear();

        for (int i = 0; i < m_Head.Length; i++)
        {
            m_CurrentSkinSprites.Add(m_Head[i].sprite);
        }
        for (int i = 0; i < m_Body.Length; i++)
        {
            m_CurrentSkinSprites.Add(m_Body[i].sprite);
        }
        for (int i = 0; i < m_Hands.Length; i++)
        {
            m_CurrentSkinSprites.Add(m_Hands[i].sprite);
        }
        for (int i = 0; i < m_Pants.Length; i++)
        {
            m_CurrentSkinSprites.Add(m_Pants[i].sprite);
        }
        for (int i = 0; i < m_Shoes.Length; i++)
        {
            m_CurrentSkinSprites.Add(m_Shoes[i].sprite);
        }
    }
    /// <summary>
    /// Resore skin to the equiped skin
    /// </summary>
    public void RestoreSkin()
    {
        if (m_SkinEquiped) return;

        int _currentSpriteIndex = 0;
        for (int i = 0; i < m_Head.Length; i++)
        {
            m_Head[i].sprite = m_CurrentSkinSprites[_currentSpriteIndex];
            _currentSpriteIndex++;
        }
        for (int i = 0; i < m_Body.Length; i++)
        {
            m_Body[i].sprite = m_CurrentSkinSprites[_currentSpriteIndex];
            _currentSpriteIndex++;
        }
        for (int i = 0; i < m_Hands.Length; i++)
        {
            m_Hands[i].sprite = m_CurrentSkinSprites[_currentSpriteIndex];
            _currentSpriteIndex++;
        }
        for (int i = 0; i < m_Pants.Length; i++)
        {
            m_Pants[i].sprite = m_CurrentSkinSprites[_currentSpriteIndex];
            _currentSpriteIndex++;
        }
        for (int i = 0; i < m_Shoes.Length; i++)
        {
            m_Shoes[i].sprite = m_CurrentSkinSprites[_currentSpriteIndex];
            _currentSpriteIndex++;
        }
    }
}
