using Abp.Domain.Repositories;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class ItemDAL
    {
        IRepository<Item, Guid> db;
        public ItemDAL(IRepository<Item, Guid> itemRepository)
        {
            db = itemRepository;
        }
        public List<ItemOutput> GetMinibarItem()
        {
            List<ItemOutput> lst = new List<ItemOutput>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1 && x.Minibar == 1).OrderBy(x => x.Description).Select
                (x => new ItemOutput
                {
                    ItemKey = x.Id,
                    Description = x.Description,
                    SalesPrice = x.SalesPrice,
                    PostCodeKey = x.PostCodeKey
                }).ToList();
            }
            catch { }
            return lst;
        }

        public List<ItemSelectedOutput> GetMinibarSelectedItem()
        {
            List<ItemSelectedOutput> lst = new List<ItemSelectedOutput>();
            try
            {
                //lst = db.GetAll().Where(x => x.Active == 1 && x.Minibar == 1).OrderBy(x => x.Description).Select
                //(x => new ItemOutput
                //{
                //    ItemKey = x.Id,
                //    Description = x.Description
                //}).ToList();
            }
            catch { }
            return lst;
        }

        public Item GetItemByItemKey(string itemKey)
        {
            Guid id = new Guid(itemKey);
            Item a = new Item();
            try
            {
                a = db.GetAll().Where(x => x.Id == id).FirstOrDefault();

            }
            catch { }
            return a;
        }

        public List<ItemOutput> GetLaundryItem()
        {
            List<ItemOutput> lst = new List<ItemOutput>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1 && x.Laundry == 1).OrderBy(x => x.Sort).Select
                (x => new ItemOutput
                {
                    ItemKey = x.Id,
                    Description = x.Description,
                    SalesPrice = x.SalesPrice,
                    PostCodeKey = x.PostCodeKey
                }).ToList();
            }
            catch { }
            return lst;

        }
    }
}
