namespace Checkout.Service.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IList<DiscountOnQty> _discountPrices;

        public DiscountService(IList<DiscountOnQty> discountPrices)
        {
            _discountPrices = discountPrices;
        }

        public double GetDiscountedPrice(IList<Product> scannedProducts)
        {
            var scannedProductsGroup = scannedProducts.GroupBy(p => p.SKU);
            double discount = 0;

            foreach (var scannedProduct in scannedProductsGroup)
            {
                var SKU = scannedProduct.Key;
                var productQty = scannedProduct.Count();

                var discountItem = _discountPrices.SingleOrDefault(d => d.SKU == SKU);
                if (discountItem == null)
                    continue;

                var discountQty = discountItem.Quantity;
                var discountPrice = discountItem.Price;

                if (productQty < discountQty)
                    continue;

                discount += (productQty / discountQty) * discountPrice;
                discount += (productQty % discountQty) * getProductPrice(scannedProducts, SKU);
            }

            return discount;
        }

        public IList<DiscountOnQty> GetDiscountPrices()
        {
            return _discountPrices;
        }


        private double getProductPrice(IList<Product> scannedProducts, char SKU)
        {
            return scannedProducts?.Distinct()?.SingleOrDefault(p => p.SKU == SKU)?.Price ?? 0;
        }
    }
}