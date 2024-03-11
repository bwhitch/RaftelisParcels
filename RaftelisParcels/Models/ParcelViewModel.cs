namespace RaftelisParcels.Models
{
    public class ParcelViewModel
    {
        //PIN|ADDRESS|OWNER|MARKET_VALUE|SALE_DATE|SALE_PRICE|LINK

        public string Pin { get; set; }
        public int HouseNumber { get; set; }

        public string Street { get; set;}

        public string Owner { get; set;}

        public Decimal Value { get; set;}

        public String SaleDate { get; set; }

        public Decimal SalePrice { get; set; }

        public string Link { get; set;}

        public string Unit { get; set;}

        public string FirstName { get; set; }

        public string LocationUrl { get; set; }
    }
}
