using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour {

    public Text[] m_scoreLabels;

    private static TicTacToe s_Instance;
    public static TicTacToe Instance
    {
        get { return s_Instance; }
    }

    public enum Player
    {
        X,
        O,
        NONE,
        DRAW, // Little hack
    }

    private String[] m_playerNames = new string[] { "X", "O", "", "" };

    Button[] m_uiCells = new Button[9];
    public Button[] UICells
    {
        get { return m_uiCells; }
    }


    Player[] m_cells = new Player[9];

    Dictionary<Player, IPlayer> m_players = new Dictionary<Player, IPlayer>();
    Player m_currentPlayer = Player.NONE;

    // Use this for initialization
    void Start () {
        s_Instance = this;

        // Fetching buttons
        var cells = FindObjectsOfType<Button>();
        foreach(var cell in cells)
        {
            int cellIdx = cell.GetComponent<GameCell>().cellId;
            if(cellIdx >= 0 && cellIdx < 9)
            {
                m_uiCells[cellIdx] = cell;
            }
        }

        InitPlayers();

        Restart();
	}

    void InitPlayers()
    {
        m_players[Player.O] = new HumanPlayer(Player.O);
        m_players[Player.X] = new HumanPlayer(Player.X);
        for(int i=0; i< m_scoreLabels.Length; i++)
            m_scoreLabels[i].text = m_playerNames[i] + " : " + m_players[(Player)i].Score;
    }

    void UpdateScores()
    {
        for (int i = 0; i < m_scoreLabels.Length; i++)
            m_scoreLabels[i].text = m_playerNames[i] + " : " + m_players[(Player)i].Score;
    }

    void Awake()
    {
        s_Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        int move;
        if (m_players[m_currentPlayer].GetMove(out move))
        {
            if(move >= 0 && move < 9 && m_cells[move] == Player.NONE)
            {
                m_cells[move] = m_currentPlayer;
                SwitchPlayer();
                UpdateButton(m_uiCells[move], move);
            }

            Player winner = GetWinner();
            if(winner == Player.O || winner == Player.X)
            {
                m_players[winner].Score++;
                UpdateScores();
                Restart();
            }
            else if(winner == Player.DRAW)
            {
                Restart();
            }
        }
	}

    void Restart()
    {
        // initializing the grid
        for (int i = 0; i < m_cells.Length; ++i)
        {
            m_cells[i] = Player.NONE;
            UpdateButton(m_uiCells[i], i);
        }

        // setup initial player
        // disable everyone and call Reset
        foreach (var pair in m_players)
        {
            pair.Value.Enabled = false;
            pair.Value.Reset();
        }

        m_currentPlayer = Player.O;
        m_players[m_currentPlayer].Enabled = true;
    }

    Player GetWinner()
    {
        // 0 1 2
        // 3 4 5
        // 6 7 8
        for(int i=0; i<3; i++)
            print(m_cells[3*i] + " " + m_cells[3*i+1] + " " + m_cells[3*i+2]);
        print("______");

        // first line
        if (m_cells[0] == m_cells[1] && m_cells[0] == m_cells[2])
            return m_cells[0];

        // second line
        if (m_cells[3] == m_cells[4] && m_cells[3] == m_cells[5])
            return m_cells[3];

        // third line
        if (m_cells[6] == m_cells[7] && m_cells[6] == m_cells[8])
            return m_cells[6];

        // first collumn
        if (m_cells[0] == m_cells[3] && m_cells[0] == m_cells[6])
            return m_cells[0];

        // second collumn
        if (m_cells[1] == m_cells[4] && m_cells[1] == m_cells[7])
            return m_cells[1];

        // third collumn
        if (m_cells[2] == m_cells[5] && m_cells[2] == m_cells[8])
            return m_cells[2];

        // first diag
        if (m_cells[0] == m_cells[4] && m_cells[0] == m_cells[8])
            return m_cells[0];

        // second diag
        if (m_cells[2] == m_cells[4] && m_cells[2] == m_cells[6])
            return m_cells[2];

        // check for draw
        bool full = true;
        int k = 0;
        while (full && k < m_cells.Length)
            full = m_cells[k++] != Player.NONE;

        if (full)
            return Player.DRAW;
        else
            return Player.NONE;
    }

    void UpdateButton(Button _button, int _idx)
    {
        string txt = "";
        switch(m_cells[_idx])
        {
            case Player.O:
                txt = "O";
                break;
            case Player.X:
                txt = "X";
                break;
            default:
                break;

        }
        _button.GetComponentInChildren<Text>().text = txt;
    }

    void SwitchPlayer()
    {
        m_players[m_currentPlayer].Enabled = false;
        switch (m_currentPlayer)
        {
            case Player.O:
                m_currentPlayer = Player.X;
                break;
            case Player.X:
                m_currentPlayer = Player.O;
                break;
            default:
                break;
        }
        m_players[m_currentPlayer].Enabled = true;
    }

    void OnGUI()
    {

    }
}
