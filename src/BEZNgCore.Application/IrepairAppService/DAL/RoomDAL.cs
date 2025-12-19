using Abp.Domain.Repositories;
using BEZNgCore.Common;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService.DAL
{
    public class RoomDAL : BEZNgCoreAppServiceBase
    {
        IRepository<Room, Guid> db;
        public RoomDAL(IRepository<Room, Guid> roomRepository)
        {
            db = roomRepository;
        }
        public List<DDLRoomOutput> GetBindDDLRoom()
        {
            List<DDLRoomOutput> lst = new List<DDLRoomOutput>();
            lst = db.GetAll().Where(x => x.Active == 1).OrderBy(x => x.Floor).ThenBy(x => x.Unit)
               .Select(x => new DDLRoomOutput
               {
                   RoomKey = x.Id,
                   Unit = x.Unit
               })
               .ToList();
            return lst;
        }
        public async Task UpdateRoomAsync(RoomDto roomdto)
        {
            if (roomdto == null) throw new ArgumentNullException("roomdto");

            //await using var context = await _contextFactory.CreateDbContextAsync();
            //var character = db.GetAll()
            //    .Single(x => x.Id == roomdto.RoomKey);
            var character = db
        .GetAllIncluding(x => x)
        .Single(x => x.Id == roomdto.RoomKey);

            // Here you could check a row version on the DB vs. DTO to see if the DB had changed since the data used to build the DTO was read.
            var r = ObjectMapper.Map(roomdto, character);
            //ObjectMapper.Map<RoomDto>(r)
            //_mapper.Map(characterDto, character);

            //If personalities can be added or removed...
            var existingPersonalityIds = character;
            var personalityIdsToAdd = r;
            //var personalityIdsToRemove = roomdto;

            db.Delete(character);
            db.InsertOrUpdate(r);
            //var personalitiesToAdd = await db.GetAll().Where(x => personalityIdsToAdd.Contains(x.PersonalityId).ToListAsync();
            //var personalitiesToRemove = character.Personalities.Where(x => personalityIdsToRemove.Contains(x.PersonalityId)).ToList();

            //foreach (var personality in personalitiesToRemove)
            //    character.Personalities.Remove(personality);

            //foreach (var personality in personalitiesToAdd)
            //    character.Personalities.Add(personality);

            //await db.GetAllIncluding();
        }
        //public async Task<int> updateRoomAsync(Room d)
        //{
        //    int isSuccess = 0;
        //    try
        //    {
        //        await db.UpdateAsync(d);
        //        isSuccess = 1;
        //    }
        //    catch (Exception e)
        //    {
        //        string msg = e.InnerException.Message;
        //        Console.WriteLine(msg);
        //    }
        //    return isSuccess;
        //}

        public int Save(Room d)
        {
            int success = 0;
            try
            {
                Guid id = db.InsertOrUpdateAndGetId(d);
                if (id != Guid.Empty)
                    success = 1;
                // success = db.InsertAndGetId(d);
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        public List<Room> GetbyId(Guid id)
        {
            List<Room> lst = new List<Room>();
            try
            {
                lst = db.GetAll().Where(x => x.Id == id).ToList();
            }
            catch { }
            return lst;
        }
        public List<GetHotelFloor> BindHotelFloorList()
        {
            List<GetHotelFloor> lst = new List<GetHotelFloor>();
            try
            {
                lst = db.GetAll().GroupBy(x => x.Floor)
                       .OrderBy(x => x.Key)
                       .Select(x => new GetHotelFloor
                       {
                           btnFloor = CommomData.GetNumber(x.Key.ToString()),
                           Floor = x.Key.ToString()
                       })
                                        .ToList();
            }
            catch { }
            return lst;
        }
        public string GetRoomKeyByRoomNo(string roomNo)
        {
            string RoomKey = Guid.Empty.ToString();
            try
            {
                Guid id = db.GetAll().Where(x => x.Unit == roomNo).Select(x => x.Id).FirstOrDefault();
                if (id != Guid.Empty)
                    RoomKey = id.ToString();
            }
            catch (Exception ex)
            {
            }
            return RoomKey;
        }
        public List<GetHotelRoom> GetHotelRoom()
        {
            List<GetHotelRoom> lst = new List<GetHotelRoom>();
            try
            {
                lst = db.GetAll().Where(x => x.Active == 1)
                       .OrderBy(x => x.Floor).ThenBy(x => x.Unit)
                       .Select(x => new GetHotelRoom
                       {
                           RoomKey = x.Id,
                           Unit = x.Unit
                       }).ToList();
            }
            catch { }
            return lst;
        }
        public List<Room> GetbyUnit(string strRoomNo)
        {
            List<Room> lst = new List<Room>();
            try
            {
                lst = db.GetAll().Where(x => x.Unit == strRoomNo).Select(x => x).ToList();
            }
            catch { }
            return lst;
        }
        //public async Task UpdateAsync(Room d)
        //{
        //    try
        //    {
        //        await db.UpdateAsync(d);

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}
