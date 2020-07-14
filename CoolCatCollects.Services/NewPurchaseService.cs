using CoolCatCollects.Data.Entities.Purchases;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public class NewPurchaseService : IDisposable
	{
		public BaseRepository<NewPurchase> _repo = new BaseRepository<NewPurchase>();

		public async Task<IEnumerable<NewPurchaseModel>> GetAll()
		{
			var newPurchases = await _repo.FindAllAsync();

			return newPurchases.Select(ToModel).OrderByDescending(x => x.Date);
		}

		public async Task<NewPurchaseModel> FindAsync(int id)
		{
			var newPurchase = await _repo.FindOneAsync(id);

			return ToModel(newPurchase);
		}

		public async Task Add(NewPurchaseModel model)
		{
			var newPurchase = new NewPurchase
			{
				Date = model.Date,
				SetNumber = model.SetNumber,
				SetName = model.SetName,
				Theme = model.Theme,
				Promotions = model.Promotions,
				Price = model.Price,
				UnitPrice = model.UnitPrice,
				Quantity = model.Quantity,
				Parts = model.Parts,
				TotalParts = model.TotalParts,
				PriceToPartOutRatio = model.PriceToPartOutRatio,
				Source = model.Source,
				PaymentMethod = model.PaymentMethod,
				AveragePartOutValue = model.AveragePartOutValue,
				MyPartOutValue = model.MyPartOutValue,
				ExpectedGrossProfit = model.ExpectedGrossProfit,
				ExpectedNetProfit = model.ExpectedNetProfit,
				Status = model.Status,
				SellingNotes = model.SellingNotes,
				Notes = model.Notes
			};

			await _repo.AddAsync(newPurchase);
		}

		public async Task Edit(NewPurchaseModel model)
		{
			var newPurchase = await _repo.FindOneAsync(model.Id);

			newPurchase.Date = model.Date;
			newPurchase.SetNumber = model.SetNumber;
			newPurchase.SetName = model.SetName;
			newPurchase.Theme = model.Theme;
			newPurchase.Promotions = model.Promotions;
			newPurchase.Price = model.Price;
			newPurchase.UnitPrice = model.UnitPrice;
			newPurchase.Quantity = model.Quantity;
			newPurchase.Parts = model.Parts;
			newPurchase.TotalParts = model.TotalParts;
			newPurchase.PriceToPartOutRatio = model.PriceToPartOutRatio;
			newPurchase.Source = model.Source;
			newPurchase.PaymentMethod = model.PaymentMethod;
			newPurchase.AveragePartOutValue = model.AveragePartOutValue;
			newPurchase.MyPartOutValue = model.MyPartOutValue;
			newPurchase.ExpectedGrossProfit = model.ExpectedGrossProfit;
			newPurchase.ExpectedNetProfit = model.ExpectedNetProfit;
			newPurchase.Status = model.Status;
			newPurchase.SellingNotes = model.SellingNotes;
			newPurchase.Notes = model.Notes;

			await _repo.UpdateAsync(newPurchase);
		}

		public async Task Delete(int id)
		{
			var newPurchase = await _repo.FindOneAsync(id);

			await _repo.RemoveAsync(newPurchase);
		}

		public NewPurchaseModel ToModel(NewPurchase newPurchase)
		{
			return new NewPurchaseModel
			{
				Id = newPurchase.Id,
				Date = newPurchase.Date,
				SetNumber = newPurchase.SetNumber,
				SetName = newPurchase.SetName,
				Theme = newPurchase.Theme,
				Promotions = newPurchase.Promotions,
				Price = newPurchase.Price,
				UnitPrice = newPurchase.UnitPrice,
				Quantity = newPurchase.Quantity,
				Parts = newPurchase.Parts,
				TotalParts = newPurchase.TotalParts,
				PriceToPartOutRatio = newPurchase.PriceToPartOutRatio,
				Source = newPurchase.Source,
				PaymentMethod = newPurchase.PaymentMethod,
				AveragePartOutValue = newPurchase.AveragePartOutValue,
				MyPartOutValue = newPurchase.MyPartOutValue,
				ExpectedGrossProfit = newPurchase.ExpectedGrossProfit,
				ExpectedNetProfit = newPurchase.ExpectedNetProfit,
				Status = newPurchase.Status,
				SellingNotes = newPurchase.SellingNotes,
				Notes = newPurchase.Notes
			};
		}

		public void Dispose()
		{
			_repo.Dispose();
		}
	}
}
