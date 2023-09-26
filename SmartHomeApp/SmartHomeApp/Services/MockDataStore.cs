using SmartHomeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 


namespace SmartHomeApp.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Name = "Steckdose Zuhause", Description="Für die Lampe im Schlafzimmer.", Ip="192.168.188.45"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Second item", Description="This is an item description.", Ip="132.111.133.1"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Third item", Description="This is an item description.", Ip= "1.1.1.1" },
                new Item { Id = Guid.NewGuid().ToString(), Name = "Fourth item", Description="This is an item description.", Ip = "192.168.5.3"},
                new Item { Id = Guid.NewGuid().ToString(), Name = "Steckdose Schule", Description="Zum Testen.", Ip = "192.168.216.116"},
        //        new Item { Id = Guid.NewGuid().ToString(), Name = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}