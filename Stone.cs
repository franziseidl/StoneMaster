using Neo;
using Neo.SmartContract.Framework;

namespace StoneMaster
{
    public class Stone
    {
        public string Name { get; set; }
        public List<Position> Positions { get; set; }
    }
}