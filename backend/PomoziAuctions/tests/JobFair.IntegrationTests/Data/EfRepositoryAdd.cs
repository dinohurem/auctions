//using PomoziAuctions.Core.PurchaseOrderAggregate.Models;
//using Xunit;

//namespace PomoziAuctions.IntegrationTests.Data;
//public class EfRepositoryAdd : BaseEfRepoTestFixture
//{
//  [Fact]
//  public async Task AddsPurchaseOrderAndSetsId()
//  {
//    var repository = GetRepository();
//    var purchaseOrder = new PurchaseOrder { VendorName = "Test", Status = Core.PurchaseOrderAggregate.Enums.PurchaseOrderStatusEnum.Draft };

//    await repository.AddAsync(purchaseOrder);

//    var newPurchaseOrder = (await repository.ListAsync())
//                    .FirstOrDefault();

//    Assert.True(newPurchaseOrder?.Id > 0);
//  }
//}
