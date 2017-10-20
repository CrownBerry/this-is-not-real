namespace Interfaces
{
    public interface IPuzzle
    {
        void AddToList(ISwitchable switcher);
        void Open();
        void Close();
    }
}