using CoolCatCollects.Data.Entities.Purchases;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public class UsedPurchaseService : IDisposable
	{
		public BaseRepository<UsedPurchase> _repo = new BaseRepository<UsedPurchase>();
		public BaseRepository<UsedPurchaseWeight> _weightRepo;

		public UsedPurchaseService()
		{
			_weightRepo = new BaseRepository<UsedPurchaseWeight>(_repo.Context);
		}

		public async Task<IEnumerable<UsedPurchaseModel>> GetAll()
		{
			var UsedPurchases = await _repo.FindAllAsync();

			return UsedPurchases.Select(ToModel).OrderByDescending(x => x.Date);
		}

		public async Task<UsedPurchaseModel> FindAsync(int id)
		{
			var usedPurchase = await _repo.FindOneAsync(id);

			return ToModel(usedPurchase);
		}

		public async Task Add(UsedPurchaseModel model)
		{
			var usedPurchase = new UsedPurchase
			{
				Id = model.Id,
				Date = model.Date,
				Source = model.Source,
				SourceUsername = model.SourceUsername,
				OrderNumber = model.OrderNumber,
				Price = model.Price,
				PaymentMethod = model.PaymentMethod,
				Receipt = model.Receipt,
				DistanceTravelled = model.DistanceTravelled,
				Location = model.Location,
				Postage = model.Postage,
				Weight = model.Weight,
				PricePerKilo = model.PricePerKilo,
				CompleteSets = model.CompleteSets,
				Notes = model.Notes
			};

			await _repo.AddAsync(usedPurchase);
		}

		public async Task Edit(UsedPurchaseModel model)
		{
			var usedPurchase = await _repo.FindOneAsync(model.Id);

			usedPurchase.Id = model.Id;
			usedPurchase.Date = model.Date;
			usedPurchase.Source = model.Source;
			usedPurchase.SourceUsername = model.SourceUsername;
			usedPurchase.OrderNumber = model.OrderNumber;
			usedPurchase.Price = model.Price;
			usedPurchase.PaymentMethod = model.PaymentMethod;
			usedPurchase.Receipt = model.Receipt;
			usedPurchase.DistanceTravelled = model.DistanceTravelled;
			usedPurchase.Location = model.Location;
			usedPurchase.Postage = model.Postage;
			usedPurchase.Weight = model.Weight;
			usedPurchase.PricePerKilo = model.PricePerKilo;
			usedPurchase.CompleteSets = model.CompleteSets;
			usedPurchase.Notes = model.Notes;

			await _repo.UpdateAsync(usedPurchase);
		}

		public async Task Delete(int id)
		{
			var usedPurchase = await _repo.FindOneAsync(id);

			await _repo.RemoveAsync(usedPurchase);
		}

		public UsedPurchaseModel ToModel(UsedPurchase usedPurchase)
		{
			return new UsedPurchaseModel
			{
				Id = usedPurchase.Id,
				Date = usedPurchase.Date,
				Source = usedPurchase.Source,
				SourceUsername = usedPurchase.SourceUsername,
				OrderNumber = usedPurchase.OrderNumber,
				Price = usedPurchase.Price,
				PaymentMethod = usedPurchase.PaymentMethod,
				Receipt = usedPurchase.Receipt,
				DistanceTravelled = usedPurchase.DistanceTravelled,
				Location = usedPurchase.Location,
				Postage = usedPurchase.Postage,
				Weight = usedPurchase.Weight,
				PricePerKilo = usedPurchase.PricePerKilo,
				CompleteSets = usedPurchase.CompleteSets,
				Notes = usedPurchase.Notes
			};
		}

		public UsedPurchaseWeightModel ToModel(UsedPurchaseWeight wt)
		{
			return new UsedPurchaseWeightModel
			{
				Id = wt.Id,
				Colour = wt.Colour,
				Weight = wt.Weight,
				UsedPurchaseId = wt.UsedPurchase.Id
			};
		}

		public UsedPurchaseWeight ToEntity(UsedPurchaseWeightModel wt, UsedPurchase purchase)
		{
			return new UsedPurchaseWeight
			{
				Id = wt.Id,
				Colour = wt.Colour,
				Weight = wt.Weight,
				UsedPurchase = purchase
			};
		}

		public async Task<UsedPurchaseModel> GetPurchaseWithWeights(int id)
		{
			var purchase = await _repo.FindOneAsync(id);

			var model = ToModel(purchase);

			var weights = purchase.Weights.Select(ToModel);

			model.Weights = weights;

			return model;
		}

		public void Dispose()
		{
			_repo.Dispose();
		}

		public async Task UpdateWeights(int id, IEnumerable<UsedPurchaseWeightModel> weights)
		{
			if (weights == null)
			{
				weights = new List<UsedPurchaseWeightModel>();
			}

			var purchase = await _repo.FindOneAsync(id);

			var existingWeights = purchase.Weights;
			var existingIds = existingWeights.Select(x => x.Id);

			var toAdd = weights.Where(x => !existingIds.Contains(x.Id) && !x.Delete).ToList();
			var toUpdate = weights.Where(x => existingIds.Contains(x.Id) && !x.Delete).ToList();
			var toDelete = weights.Where(x => x.Delete).ToList();

			// Add
			foreach (var wt in toAdd.Select(x => ToEntity(x, purchase)))
			{
				await _weightRepo.AddAsync(wt);
			}

			// Update
			foreach (var wt in toUpdate)
			{
				var entity = await _weightRepo.FindOneAsync(wt.Id);

				entity.Colour = wt.Colour;
				entity.Weight = wt.Weight;

				await _weightRepo.UpdateAsync(entity);
			}

			// Delete
			await _weightRepo.RemoveManyAsync(toDelete.Select(x => _weightRepo.FindOne(x.Id)));
		}
	}
}
