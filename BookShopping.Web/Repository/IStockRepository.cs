﻿namespace BookShopping.Web.Repository
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByBookId(int bookId);
        Task ManageStock(StockDTO stockToManage);
    }
}
 
