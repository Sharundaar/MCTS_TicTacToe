using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HumanPlayer : IPlayer
{
    TicTacToe.Player m_Type;
    private int m_move = -1;

    public int Score { get; set; }
    public bool Enabled { get; set; }

    class BoxxedInt { public int data; }

    public HumanPlayer(TicTacToe.Player _type)
    {
        m_Type = _type;

        Button[] uiCells = TicTacToe.Instance.UICells;
        for (int i = 0; i < uiCells.Length; ++i)
        {
            BoxxedInt idx = new BoxxedInt() { data = i };
            uiCells[i].onClick.AddListener(new UnityEngine.Events.UnityAction(
                    () => ClickEventReceived(idx.data)
                )
            );
        }
    }

    public void Reset()
    {
        m_move = -1;
    }

    public bool GetMove(out int _cellIdx)
    {
        if (m_move == -1)
        {
            _cellIdx = -1;
            return false;
        }

        _cellIdx = m_move;
        m_move = -1;
        return true;
    }

    private void ClickEventReceived(int _cellIdx)
    {
        if(Enabled)
            m_move = _cellIdx;
    }

}
