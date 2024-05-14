using Neo;

namespace StoneMaster
{
    public class Position
    {
        public double Timestamp { get; set; }
        public string Image { get; set; }
        public UInt160 Sender { get; set; }
    }
}