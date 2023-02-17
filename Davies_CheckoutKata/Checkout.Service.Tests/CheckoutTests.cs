namespace Checkout.Service.Tests
{
    public class CheckoutTests
    {
        ICheckoutService _checkoutService;
        public CheckoutTests()
        {
            var _fakeProducts = new List<Product>
            {
                new Product { SKU = 'A', Price = 10 },
                new Product { SKU = 'B', Price = 15},
                new Product { SKU = 'C', Price = 40 },
                new Product { SKU = 'D', Price = 55 }
            };

            var _fakeDiscountPrices = new List<DiscountOnQty>
            {
                new DiscountOnQty { SKU = 'D', Quantity = 2, Price = 82.5 },
                new DiscountOnQty { SKU = 'B', Quantity = 3, Price = 40 }
            };

            var discountService = new DiscountService(_fakeDiscountPrices);
            var pricingService = new PricingService(discountService);

            _checkoutService = new CheckoutService(_fakeProducts, pricingService);
        }
        private void fakeScanProduct(string products)
        {
            foreach (var product in products.ToCharArray())
            {
                _checkoutService.ScanProducts(product);
            }
        }

        [Fact]
        public void ReturnScannedProductWhenSKUScanned()
        {
            _checkoutService.ScanProducts('A');

            var scannedProducts = _checkoutService.GetScannedProducts();

            Assert.Equal('A', scannedProducts[0].SKU);
        }

        [Theory]
        [InlineData("A", 10)]
        [InlineData("B", 15)]
        [InlineData("C", 40)]
        [InlineData("D", 55)]
        public void ReturnTotalWhenSingleProductIsScanned(string products, double expectedTotal)
        {
            fakeScanProduct(products);

            var total = _checkoutService.GetTotalPrice();

            Assert.Equal(expectedTotal, total);
        }

        [Theory]
        [InlineData("AB", 25)]
        [InlineData("BC", 55)]
        [InlineData("CD", 95)]
        [InlineData("DA", 65)]
        public void ReturnTotalWhenTwoDifferentProductsAreScanned(string products, double expectedTotal)
        {
            fakeScanProduct(products);

            var total = _checkoutService.GetTotalPrice();

            Assert.Equal(expectedTotal, total);
        }

        [Theory]
        [InlineData("BBB", 40)]
        [InlineData("DD", 82.5)]
        public void ReturnDiscountedTotalWhenProductsAligibleForDiscountAreScanned(string products, double expectedTotal)
        {
            fakeScanProduct(products);

            var total = _checkoutService.GetTotalPrice();

            Assert.Equal(expectedTotal, total);
        }
    }
}
