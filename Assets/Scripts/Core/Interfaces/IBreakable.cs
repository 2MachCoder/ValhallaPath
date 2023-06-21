namespace Interfaces
{
    public interface IBreakable
    {
        public bool IsBroken { get; }
        void Break();

        void Fix();
    }
}