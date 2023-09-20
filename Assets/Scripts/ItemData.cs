using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public ItemType m_ItemType;
    public int m_ItemID;
    public string m_Name;
    public int m_SellingCost, m_PurchaseCost;
    public Sprite m_Icon;
    public Sprite[] m_ClothesSprite;
}

public enum ItemType
{
    HEAD,
    BODY,
    HANDS,
    PANTS,
    SHOES
}
