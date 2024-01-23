using System.Collections.Generic;

namespace GroupProject_RichardsFootlong
{
    internal class OrderRender
    {
        public ActiveUser user;
        public int Quantity { get; set; }
        public string Cheese { get; set; }
        public string BreadName { get; set; }
        public string MeatName { get; set; }
        public string Price { get; set; }

        public List<string> Veggie { get; set; }
        public List<string> Sauce { get; set; }
    }
}
