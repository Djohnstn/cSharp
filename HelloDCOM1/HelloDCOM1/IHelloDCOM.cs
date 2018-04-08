using System;

namespace HelloDCOM1
{
    public interface IHelloDCOM
    {
        string Computer { get; }
        string Info { get; }
        string Time { get; }
        string UserID { get; }

        void Save(string info);
    }
}