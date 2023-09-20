using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public ItemData m_Data;
    public int m_Amount, m_MaxAmount;
    public bool m_AvailableToBuy;
    public bool m_Equiped;

    public void Add(int a_Amount)
    {
        if (m_Amount >= m_MaxAmount) return;

        if (m_Amount + a_Amount > m_MaxAmount)
            m_Amount = m_MaxAmount;
        else
            m_Amount += a_Amount;

        if(m_Amount > 0) m_AvailableToBuy = true;
    }

    public void Remove(int amount)
    {
        if (m_Amount <= 0) return;

        if (m_Amount - amount <= 0)
        {
            m_Amount = 0;
            m_AvailableToBuy = false;
        }
        else
            m_Amount -= amount;
    }
}
