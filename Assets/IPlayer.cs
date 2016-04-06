using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPlayer
{
    bool GetMove(out int _cellIdx);
    int Score { get; set; }
    bool Enabled { get; set; }
    void Reset();
}
