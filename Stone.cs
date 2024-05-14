using Neo;
using Neo.SmartContract.Framework;

namespace StoneMaster
{
    public class Stone
    {
        public string Name { get; set; }
        public UInt160 Owner { get; set; }
        public List<Position> Positions { get; set; }
    }
}