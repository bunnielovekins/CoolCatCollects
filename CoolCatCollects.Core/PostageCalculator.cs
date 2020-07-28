using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Core
{
	/// <summary>
	/// Class to work out the postage options for a particular listing, in order to get ebay premium
	/// </summary>
	public static class PostageCalculator
	{
		public static IEnumerable<PostageResult> GetPostages(string weight, string price, string size)
		{
			decimal w = decimal.Parse(weight) - 0.01m;
			decimal p = decimal.Parse(price);
			Size s;
			if (size == "LL")
			{
				s = Size.LargeLetter;
			}
			else if (size == "SP")
			{
				s = Size.SmallParcel;
			}
			else
			{
				s = Size.Parcel;
			}

			var postages = new List<IPostageService>
			{
				new Economy.RoyalMailSecond(),
				new Economy.RoyalMailTracked48(),
				new Economy.Hermes(),
				new Standard.RoyalMailFirst(),
				new Standard.RoyalMailTracked24(),
				new Express.RoyalMailSpecialDelivery()
			};

			var results = postages.Select(x => x.GetPostage(w, p, s)).Where(x => x.Suitable);

			return results;
		}

		public enum Size
		{
			LargeLetter,
			SmallParcel,
			Parcel
		};

		public enum ServiceType
		{
			Economy,
			Standard,
			Express
		}

		public interface IPostageService
		{
			string PostName { get; }
			IEnumerable<PostageBand> Bands { get; }

			/// <summary>
			/// Works out the postage
			/// </summary>
			/// <param name="weight">Weight in kg</param>
			/// <param name="price">Price in pounds</param>
			/// <param name="size">Size, which is a dropdown</param>
			/// <returns></returns>
			PostageResult GetPostage(decimal weight, decimal price, Size size);
		}

		public class PostageResult
		{
			public PostageResult()
			{
				Suitable = false;
			}

			public PostageResult(string name, PostageBand band, ServiceType type)
			{
				Suitable = true;
				Name = name;
				Price = band.Price;
				Band = band;
				Type = type;
			}

			public PostageResult(string name, decimal price, PostageBand band, ServiceType type)
			{
				Suitable = true;
				Name = name;
				Price = price;
				Band = band;
				Type = type;
			}

			public bool Suitable { get; }
			public string Name { get; }
			public decimal Price { get; }
			public PostageBand Band { get; }
			public ServiceType Type { get; }
		}

		public class Economy
		{
			public class RoyalMailTracked48 : IPostageService
			{
				public string PostName { get => "Royal Mail Tracked 48"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 1, 3.6m, Size.LargeLetter),
						new PostageBand(0, 2, 4.74m, Size.SmallParcel),
						new PostageBand(0, 5, 10.55m, Size.Parcel)
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					if (price <= 20)
					{
						return new PostageResult();
					}

					var bands = Bands;

					if (size == Size.SmallParcel)
					{
						bands = bands.Where(x => x.Size == Size.SmallParcel || x.Size == Size.Parcel);
					}
					else if (size == Size.Parcel)
					{
						bands = bands.Where(x => x.Size == Size.Parcel);
					}

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					return new PostageResult(PostName, band, ServiceType.Economy);
				}
			}

			public class RoyalMailSecond : IPostageService
			{
				public string PostName { get => "Royal Mail 2nd Class"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 0.1m, 0.88m, Size.LargeLetter),
						new PostageBand(0.1m, 0.25m, 1.40m, Size.LargeLetter),
						new PostageBand(0.25m, 0.5m, 1.83m, Size.LargeLetter),
						new PostageBand(0.5m, 0.75m, 2.48m, Size.LargeLetter),
						new PostageBand(0, 2, 2.95m, Size.SmallParcel)
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					if (price > 20 || size == Size.Parcel)
					{
						return new PostageResult();
					}

					var bands = Bands;

					if (size == Size.SmallParcel)
					{
						bands = bands.Where(x => x.Size == Size.SmallParcel);
					}

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					return new PostageResult(PostName, band, ServiceType.Economy);
				}
			}

			public class Hermes : IPostageService
			{
				public string PostName { get => "Hermes Tracked"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 2, 2.95m, Size.Parcel),
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					if (price <= 20)
					{
						return new PostageResult();
					}

					var bands = Bands;

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					decimal insurance = 0;

					if (price <= 30)
					{
						insurance = 0.3m;
					}
					else if (price <= 40)
					{
						insurance = 0.6m;
					}
					else if (price <= 50)
					{
						insurance = 0.9m;
					}
					else if (price <= 60)
					{
						insurance = 1.8m;
					}
					else if (price <= 70)
					{
						insurance = 1.9m;
					}
					else if (price <= 150)
					{
						insurance = 4.5m;
					}
					else if (price <= 300)
					{
						insurance = 10.5m;
					}

					return new PostageResult(PostName, band.Price + insurance, band, ServiceType.Economy);
				}
			}
		}

		public class Standard
		{
			public class RoyalMailTracked24 : IPostageService
			{
				public string PostName { get => "Royal Mail Tracked 24"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 1, 4.02m, Size.LargeLetter),
						new PostageBand(0, 1, 5.46m, Size.SmallParcel),
						new PostageBand(1, 2, 7.80m, Size.SmallParcel),
						new PostageBand(0, 5, 18m, Size.Parcel)
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					if (price <= 20)
					{
						return new PostageResult();
					}

					var bands = Bands;

					if (size == Size.SmallParcel)
					{
						bands = bands.Where(x => x.Size == Size.SmallParcel || x.Size == Size.Parcel);
					}
					else if (size == Size.Parcel)
					{
						bands = bands.Where(x => x.Size == Size.Parcel);
					}

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					return new PostageResult(PostName, band, ServiceType.Standard);
				}
			}

			public class RoyalMailFirst : IPostageService
			{
				public string PostName { get => "Royal Mail 1st Class"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 0.1m, 1.15m, Size.LargeLetter),
						new PostageBand(0.1m, 0.25m, 1.64m, Size.LargeLetter),
						new PostageBand(0.25m, 0.5m, 2.14m, Size.LargeLetter),
						new PostageBand(0.5m, 0.75m, 2.95m, Size.LargeLetter),
						new PostageBand(0, 1, 3.58m, Size.SmallParcel),
						new PostageBand(1, 2, 5.47m, Size.SmallParcel)
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					if (price > 20 || size == Size.Parcel)
					{
						return new PostageResult();
					}

					var bands = Bands;

					if (size == Size.SmallParcel)
					{
						bands = bands.Where(x => x.Size == Size.SmallParcel);
					}

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					return new PostageResult(PostName, band, ServiceType.Standard);
				}
			}
		}

		public class Express
		{
			public class RoyalMailSpecialDelivery : IPostageService
			{
				public string PostName { get => "Royal Mail Special Delivery (1pm)"; }
				public IEnumerable<PostageBand> Bands
				{
					get => new List<PostageBand>
					{
						new PostageBand(0, 0.1m, 6.70m, Size.Parcel),
						new PostageBand(0.1m, 0.5m, 7.50m, Size.Parcel),
						new PostageBand(0.5m, 1, 8.80m, Size.Parcel),
						new PostageBand(1, 2, 11m, Size.Parcel),
						new PostageBand(2, 10, 26.6m, Size.Parcel),
						new PostageBand(10, 20, 41.2m, Size.Parcel)
					};
				}

				public PostageResult GetPostage(decimal weight, decimal price, Size size)
				{
					var bands = Bands;

					bands = bands.Where(x => weight >= x.Bottom && weight < x.Top);

					var band = bands.OrderBy(x => x.Price).FirstOrDefault();

					if (band == null)
					{
						return new PostageResult();
					}

					return new PostageResult(PostName, band, ServiceType.Express);
				}
			}
		}

		public class PostageBand
		{
			public decimal Bottom { get; set; }
			public decimal Top { get; set; }
			public decimal Price { get; set; }
			public Size Size { get; set; }
			public PostageBand()
			{

			}

			public PostageBand(decimal bottom, decimal top, decimal price, Size size)
			{
				Bottom = bottom;
				Top = top;
				Price = price;
				Size = size;
			}
		}
	}
}
