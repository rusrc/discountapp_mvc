using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Location;
using Discountapp.Infrastructure;
using Newtonsoft.Json;
using System;

namespace Discountapp.Domain
{
    public class DiscountappMemoryContext
    {
        public DiscountappMemoryContext()
        {
            this.DiscountappMemoryContextInit();
            this.Seed(true);
        }

        public DiscountappMemoryContext(bool userDynamicIds)
        {
            this.DiscountappMemoryContextInit();
            this.Seed(userDynamicIds);
        }

        private void DiscountappMemoryContextInit()
        {
            this.Merchants = new List<MerchantEntity>();
            this.MerchantTypes = new List<MerchantType>();
            this.Addresses = new List<Address>();
            this.Promotions = new List<Promotion>();
            this.PromotionItems = new List<PromotionItem>();
            this.Subscriptions = new List<Subscription>();
            this.Cities = new List<City>();
            this.Categories = new List<Category>();
            this.Likes = new List<Like>();
            this.MobileUser = new List<MobileUser>();
            this.MerchantCategories = new List<MerchantCategory>();
            this.Companies = new List<Company>();
        }

        const string PROMOTION_NAME = "Название акции {0}";
        const string DEFAULT_NAME = "Название / имя по умолчанию '{0}'";
        private string _defaultImagePath = @"D:/TFS/Discountapp/Discountapp.MVC/Upload/Default/nophoto.jpg"; //Path.Combine(Config.UploadFolderFullPath, "Default", Config.DefaultPromotionItemImageName) ?? "";

        public ICollection<MerchantEntity> Merchants
        {
            get
            {
                List<MerchantEntity> list = new List<MerchantEntity>();

                for (long i = 0; i < 10; i++)
                {
                    list.Add(new MerchantEntity
                    {
                        Id = i,
                        MerchantCategoryId = 2,
                        MerchantCategory = this.MerchantCategories.SingleOrDefault(c => c.Id == 2),
                        ActiveStatus = ActiveStatus.Active,
                    });
                }

                return null;
            }
            set { }
        }
        public ICollection<MerchantType> MerchantTypes { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Promotion> Promotions
        {
            get
            {
                var list = new List<Promotion>
                {
                    new Promotion
                    {
                        Id = 1,
                        Begin = DateTime.Parse("10-04-2017"),
                        End = DateTime.Parse("15-05-2017"),
                        Name = string.Format(DEFAULT_NAME, 1),
                        UserId = 1,
                        NameMultiLangJson = JsonConvert.SerializeObject(new List<NameMultiLanguageJson>{
                            new NameMultiLanguageJson(Culture.DefaultCulture, string.Format(PROMOTION_NAME, Culture.DefaultCulture))
                        }),
                        SubscriptionNotifierIsActive = true,

                        PromotionItems = new List<PromotionItem>()
                    }
                };
                return list;
            }
            set { }
        }
        public ICollection<PromotionItem> PromotionItems
        {
            get
            {
                var list = new List<PromotionItem>
                {
                    new PromotionItem
                    {
                        Id = 1,
                        BeginPrice  = 1000,
                        PromotionalPrice = 500,
                        Discount = 50,
                        CategoryId = 1,
                        Category = new Category
                        {
                            Id = 1,
                            Name = string.Format(DEFAULT_NAME, "Категория 1")
                        },
                        Name = string.Format(DEFAULT_NAME, "Продажа собак"),
                        FolderWithImagePath = _defaultImagePath
                    },
                    new PromotionItem
                    {
                        Id = 2,
                        BeginPrice  = 2000,
                        PromotionalPrice = 1500,
                        Discount = 25,
                        CategoryId = 1,
                        Category = new Category
                        {
                            Id = 1,
                            Name = string.Format(DEFAULT_NAME, "Категория 1")
                        },
                        Name = string.Format(DEFAULT_NAME, "Проджа кошек"),
                        FolderWithImagePath = _defaultImagePath
                    }
                };
                return list;
            }
            set { }
        }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<MobileUser> MobileUser { get; set; }
        public ICollection<MerchantCategory> MerchantCategories { get; set; }
        public ICollection<Company> Companies { get; set; }

        public ICollection<T> GetAll<T>() where T : class
        {
            PropertyInfo[] props = this.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (typeof(T).Name == prop?.PropertyType.GetGenericArguments().FirstOrDefault()?.Name)
                {
                    return (ICollection<T>)prop.GetValue(this, new object[] { });
                };
            }

            return null;
        }

        public void Seed(bool userDynamicIds = false)
        {
            new List<MerchantType>
            {
                new MerchantType
                {
                    Name = "Магазины",
                    NameMultiLangJson = MultiLanguage.GenerateNameMultiLangJson(new List<NameMultiLanguageJson>
                    {
                        new NameMultiLanguageJson {Name = "ru-Ru", Value = "Магазины"},
                        new NameMultiLanguageJson {Name = "kk", Value = "Магазинде"}
                    })

                },
                new MerchantType
                {
                    Name = "Спорт зал",
                    NameMultiLangJson = MultiLanguage.GenerateNameMultiLangJson(new List<NameMultiLanguageJson>
                    {
                        new NameMultiLanguageJson {Name = "ru-Ru", Value = "Спорт зал"},
                        new NameMultiLanguageJson {Name = "kk", Value = "Спорт залде"}
                    })
                }
            }.ForEach((i, e) =>
            {
                if (userDynamicIds) e.Id = i + 1;
                this.MerchantTypes.Add(e);
            });

            new List<City>
                {
                    new City {
                        Name = "Акколь",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 51.995855, 70.952948, 0)})
                    },
                    new City {Name = "Аксай",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 51.165499, 53.021901, 0)})
                    },
                    new City {Name = "Аксу",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 52.040616, 76.926367, 0)})
                    },
                    new City {Name = "Астана",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 51.128422, 71.430564, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Атбасар",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 51.815766, 68.358299, 0)})
                    },
                    new City
                    {
                        Name = "Ерейментау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 51.620112, 73.104476, 0)})
                    },
                    new City {Name = "Кокшетау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 53.284635, 69.377554, 0)})
                    },
                    new City {Name = "Макинск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 52.636117, 70.416321, 0)})
                    },
                    new City {Name = "Степногорск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 52.346664, 71.879793, 0)})
                    },
                    new City {Name = "Щучинск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 52.942107, 70.210131, 0)})
                    },
                    new City {Name = "Актобе",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 50.300411, 57.154636, 0)}),
                        ActiveStatus = ActiveStatus.Active
                    },
                    new City {Name = "Алга",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.896725, 57.328586, 0)})
                    },
                    new City {Name = "Кандыагаш",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.464870, 57.415507, 0)})
                    },
                    new City {Name = "Хромтау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 50.257871, 58.433002, 0)})
                    },
                    new City {Name = "Алматы",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.238286, 76.945456, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Иссык",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})
                    },
                    new City {Name = "Капшагай",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.899938, 77.531445, 0)})},
                    new City {Name = "Каскелен",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.202284, 76.623383, 0)})},
                    new City {Name = "Талгар",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.307864, 77.228902, 0)})},
                    new City {Name = "Талдыкорган",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 45.017958, 78.383794, 0)})},
                    new City {Name = "Текели",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 44.863420, 78.765057, 0)})},
                    new City {Name = "Атырау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 47.106719, 51.903130, 0)})},
                    new City {Name = "Кульсары",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 46.948599, 53.988308, 0)})},
                    new City {Name = "Аягоз",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 47.965266, 80.436238, 0)})},
                    new City {Name = "Зайсан",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 47.467605, 84.876376, 0)})},
                    new City {Name = "Зыряновск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.725608, 84.274505, 0)})},
                    new City {Name = "Риддер",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 50.339113, 83.506994, 0)})},
                    new City {Name = "Сатпаев",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 47.900388, 67.537697, 0)})},
                    new City {Name = "Семей",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 50.416526, 80.256170, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Серебрянск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.679563, 83.300579, 0)})},
                    new City {Name = "Солнечный",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 52.034976, 75.462230, 0)})},
                    new City {Name = "Усть-Каменогорск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.948759, 82.628459, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Шар",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 49.584958, 81.047164, 0)})},
                    new City {Name = "Шемонаиха",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 50.625035, 81.911927, 0)})},
                    new City {Name = "Жанатас",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.554650, 69.722516, 0)})},
                    new City {Name = "Каратау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.179555, 70.465441, 0)})},
                    new City {Name = "Тараз",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 42.901183, 71.378309, 0)}),





                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Шу",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Актау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Уральск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Байконур",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Балхаш",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Жезказган",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Караганда",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Сатпаев",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Темиртау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Аркалык",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Костанай",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Рудный",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Торгай",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Аральск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Кызылорда",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)}),
                        ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Баутино",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Бейнеу",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Жанаозен",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Казалы",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Павлодар",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)}), ActiveStatus = ActiveStatus.Active},
                    new City {Name = "Солнечный",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Экибастуз",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Петропавловск",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Арысь",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Жетысай",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Кентау",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Ленгер",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Сарыагаш",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Туркестан",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})},
                    new City {Name = "Шымкент",
                        MapJsonCoord = JsonConvert.SerializeObject(
                            new List<MapJsonCoord> { new MapJsonCoord("yandex", 43.358029, 77.451540, 0)})}
                }
                .ForEach((i, e) =>
                {
                    if (userDynamicIds) e.Id = i + 1;
                    e.Alias = Transliteration.Front(e.Name);
                    this.Cities.Add(e);
                });

            new List<Category>
            {
                new Category { Id = 1, Name = "Багаж и сумки", ParentId = null },
                new Category {Id = 2, Name = "Дорожные косметички, несессеры", ParentId = 1 },
                new Category {Id = 3, Name = "Сумки для ноутбуков", ParentId = 1 },
                new Category {Id = 4, Name = "Сумки для покупок", ParentId = 1 },
                new Category {Id = 5, Name = "Чемоданы", ParentId = 1 },

                new Category { Id = 6, Name = "Бизнес и промышленность", ParentId = null },
                new Category { Id = 7, Name = "Лабораторное и научное оборудование", ParentId = 6 },
                new Category { Id = 8, Name = "Лабораторное оборудование", ParentId = 7 },
                new Category { Id = 9, Name = "Лабораторные холодильники", ParentId = 8 },

                new Category { Id = 10, Name = "Дом и сад", ParentId = null },
                new Category { Id = 11, Name = "Бытовые приборы", ParentId = 10 },
                new Category { Id = 12, Name = "Водонагреватели (Бойлеры, Газовые колонки)", ParentId = 11 },
                new Category { Id = 13, Name = "Климат-контроль", ParentId = 11 },
                new Category { Id = 14, Name = "Вентиляторы", ParentId = 13 },
                new Category { Id = 15, Name = "Настольные и напольные вентиляторы", ParentId = 14 },
                new Category { Id = 16, Name = "Кондиционеры воздуха", ParentId = 13 },
                new Category { Id = 17, Name = "Обогреватели, конвекторы, тепловентиляторы", ParentId = 13 },
                new Category { Id = 18, Name = "Очистители воздуха", ParentId = 13 },
                new Category { Id = 19, Name = "Увлажнители воздуха", ParentId = 13 },
            }.ForEach((i, e) =>
            {
                if (userDynamicIds) e.Id = i + 1;
                this.Categories.Add(e);
            });

            new List<MerchantCategory>
            {
                new MerchantCategory { Name = "Продукты" },
                new MerchantCategory { Name = "Электроника и Бытовая Техника" },
                new MerchantCategory { Name = "Косметика и бытовая химия" },
                new MerchantCategory { Name = "Ремонт и товары для дома" },
                new MerchantCategory { Name = "Товары для детей" },
                new MerchantCategory { Name = "Одежда и обувь" },
                new MerchantCategory { Name = "Мебель" },
                new MerchantCategory { Name = "Аптеки" },
                new MerchantCategory { Name = "Интернет магазины" },
                new MerchantCategory { Name = "Ювелирные изделия" },
                new MerchantCategory { Name = "Авто" },
                new MerchantCategory { Name = "Товары для животных" },
                new MerchantCategory { Name = "Другое" }
            }.ForEach((i, e) =>
            {
                if (userDynamicIds) e.Id = i + 1;
                this.MerchantCategories.Add(e);
            });
        }
    }
}
