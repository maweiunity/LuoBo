using System;



public class Round
{
    public int MasterId; // 本局的唯一ID
    public int Count; // 数量 

    public Round(int masterId, int count)
    {
        MasterId = masterId;
        Count = count;
    }
}