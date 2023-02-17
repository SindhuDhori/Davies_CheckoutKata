namespace Checkout.Service.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IList<Product> _products;
        private readonly IPricingService _pricingService;
        private readonly IList<Product> _scannedProducts;

        public CheckoutService(IList<Product> products, IPricingService pricingService)
        {
            _products = products;
            _pricingService = pricingService;
            _scannedProducts = new List<Product>();
        }

        public void ScanProducts(char SKU)
        {
            var product = _products.SingleOrDefault(p => p.SKU == SKU);

            if (product != null)
            {
                _scannedProducts.Add(product);
            }
        }

        public IList<Product> GetScannedProducts()
        {
            return _scannedProducts;
        }

        public double GetTotalPrice()
        {
            var discountedPrice = _pricingService.GetDiscountedPrice(_scannedProducts);
            var nonDiscountedPrice = _pricingService.GetNonDiscountedPrice(_scannedProducts);

            return discountedPrice + nonDiscountedPrice;
        }
    }
}