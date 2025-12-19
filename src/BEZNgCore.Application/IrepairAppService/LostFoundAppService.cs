using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IrepairAppService.DAL;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class LostFoundAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<LostFound, Guid> _lostfoundRepository;
        private readonly IRepository<LostFoundStatus, Guid> _lostfoundstatusRepository;
        private readonly IRepository<MArea, int> _mareaRepository;
        private readonly IRepository<Room, Guid> _roomRepository;
        private readonly ICommondalRepository _commondalRepository;

        RoomDAL dalroom;
        LostFoundDAL dallostfound;
        public LostFoundAppService(
            IRepository<LostFound, Guid> lostfoundRepository,
            IRepository<LostFoundStatus, Guid> lostfoundstatusRepository,
             IRepository<MArea, int> mareaRepository,
             IRepository<Room, Guid> roomRepository,
             ICommondalRepository commondalRepository)
        {
            _lostfoundRepository = lostfoundRepository;
            _lostfoundstatusRepository = lostfoundstatusRepository;
            _mareaRepository = mareaRepository;
            _roomRepository = roomRepository;
            dalroom = new RoomDAL(_roomRepository);
            dallostfound = new LostFoundDAL(_lostfoundRepository);
            _commondalRepository = commondalRepository;
        }
        [HttpGet]
        public ListResultDto<GetLostFoundViewData> GetLostFoundViewData()
        {
            List<GetLostFoundViewData> Alllst = new List<GetLostFoundViewData>();
            GetLostFoundViewData a = new GetLostFoundViewData();
            a.ItemStatus = _lostfoundstatusRepository.GetAll()
                .OrderBy(x => x.LostFoundStatusName)
                .Select(x => new DDLStatusICOutput
                {
                    LostFoundStatusKey = x.Id,
                    LostFoundStatus = x.LostFoundStatusName
                })
               .ToList();
            a.Room = dalroom.GetHotelRoom();
            a.Area = _mareaRepository.GetAll()
                .Where(x => x.Active == 1)
                .OrderBy(x => x.Description)
                .Select(x => new MareaOutput
                {
                    Seqno = x.Id,
                    Description = x.Description
                })
               .ToList();
            Alllst.Add(a);
            return new ListResultDto<GetLostFoundViewData>(Alllst);
        }
        public ListResultDto<LostFoundOutPut> GetAllLostFound()
        {
            var lst = (from lf in _lostfoundRepository.GetAll()
                       join lfs in _lostfoundstatusRepository.GetAll() on lf.LostFoundStatusKey equals lfs.Id into t
                       from rt in t.DefaultIfEmpty()
                       join a in _mareaRepository.GetAll() on lf.Area equals a.Id into t1
                       from rt1 in t1.DefaultIfEmpty()
                       select new LostFoundOutPut
                       {
                           LostFoundKey = lf.Id,
                           AutoReference = lf.AutoReference,
                           Reference = lf.Reference,
                           ReportedDate = lf.ReportedDate,
                           ItemName = lf.ItemName,
                           LostFoundStatus = rt.LostFoundStatusName,
                           MArea = rt1.Description,
                           Owner = lf.Owner,
                           Founder = lf.Founder,
                           Description = lf.Description,
                           Instruction = lf.Instruction,
                           AdditionalInfo = lf.AdditionalInfo
                       }).ToList();
            return new ListResultDto<LostFoundOutPut>(lst);
        }
        #region entry form
        public ListResultDto<DDLStatusICOutput> GetBindDDLStatus()//BindDDLStatus()
        {
            var v = _lostfoundstatusRepository.GetAll()
                .OrderBy(x => x.LostFoundStatusName)
                .Select(x => new DDLStatusICOutput
                {
                    LostFoundStatusKey = x.Id,
                    LostFoundStatus = x.LostFoundStatusName
                })
               .ToList();
            return new ListResultDto<DDLStatusICOutput>(v);
        }
        public ListResultDto<MareaOutput> GetBindDDLArea()//BindDDLArea()
        {
            var v = _mareaRepository.GetAll()
                .Where(x => x.Active == 1)
                .OrderBy(x => x.Description)
                .Select(x => new MareaOutput
                {
                    Seqno = x.Id,
                    Description = x.Description
                })
               .ToList();
            return new ListResultDto<MareaOutput>(v);
        }
        public ListResultDto<GetHotelRoom> GetBindDDLRoom()//BindDDLRoom()
        {
            var v = dalroom.GetHotelRoom();
            return new ListResultDto<GetHotelRoom>(v);
        }
        public async Task<string> CreateOrUpdateLostFound(LostFoundDto input)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());

                input.StaffKey = user.StaffKey;
                if (input.Area != 0)
                {
                    input.Area = input.Area;
                }
                if (input.LostFoundStatusKey != Guid.Empty)
                {
                    input.LostFoundStatusKey = input.LostFoundStatusKey;
                }
                if (input.OwnerRoomKey != Guid.Empty)
                {
                    input.OwnerRoomKey = input.OwnerRoomKey;
                }
                if (input.FounderRoomKey != Guid.Empty)
                {
                    input.FounderRoomKey = input.FounderRoomKey;
                }

                if (input.Id.HasValue)
                {
                    var o = ObjectMapper.Map<LostFound>(input);
                    o.Sort = 0;
                    o.Sync = 0;
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSuccess = dallostfound.Update(o);

                    if (intSuccess > 0)
                    {

                        message = "Record has been updated.";
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to save the record.");
                    }

                }
                else
                {
                    input.Id = null;
                    var o = ObjectMapper.Map<LostFound>(input);
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    o.Sort = 0;
                    o.Sync = 0;
                    Guid intSuccess = dallostfound.Save(o);
                    if (intSuccess != Guid.Empty)
                    {

                        message = "Record has been added.";
                        
                        
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
            }
            return message;
        }
        public async Task<string> CreateOrUpdateLostFoundImgOld(LostFoundDtoImgInput input)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());

                input.lf.StaffKey = user.StaffKey;
                if (input.lf.Area != 0)
                {
                    input.lf.Area = input.lf.Area;
                }
                if (input.lf.LostFoundStatusKey != Guid.Empty)
                {
                    input.lf.LostFoundStatusKey = input.lf.LostFoundStatusKey;
                }
                if (input.lf.OwnerRoomKey != Guid.Empty)
                {
                    input.lf.OwnerRoomKey = input.lf.OwnerRoomKey;
                }
                if (input.lf.FounderRoomKey != Guid.Empty)
                {
                    input.lf.FounderRoomKey = input.lf.FounderRoomKey;
                }

                if (input.lf.Id.HasValue)
                {
                    var o = ObjectMapper.Map<LostFound>(input.lf);
                    o.Sort = 0;
                    o.Sync = 0;
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSuccess = dallostfound.Update(o);

                    if (intSuccess > 0)
                    {
                        
                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add
                            foreach (FWOImage dr in input.imglst)
                            {
                                LfImage image = new LfImage();
                                image.DocumentKey = Guid.NewGuid();
                                image.Sort = dr.Id;
                                image.LastModifiedStaff = user.StaffKey;
                                image.Description = dr.FileName;
                                image.DocumentName = dr.ContentType;
                                image.LostFoundKey = input.lf.Id;
                                image.Signature = dr.Data;
                                int exitcount= _commondalRepository.CheckLfImage(image);
                                if (exitcount == 0)
                                {
                                    s = _commondalRepository.InsertLfImage(image);
                                }
                                else
                                {
                                    s = _commondalRepository.UpdateLfImage(image);
                                }
                                

                            }
                            #endregion
                            if (s > 0)
                            {
                                message = "Record has been updated.";
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        else
                        {
                            message = "Record has been updated.";
                        }

                        
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to save the record.");
                    }

                }
                else
                {
                    input.lf.Id = null;
                    var o = ObjectMapper.Map<LostFound>(input.lf);
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    o.Sort = 0;
                    o.Sync = 0;
                    
                    Guid intSuccess = dallostfound.Save(o);
                    if (intSuccess != Guid.Empty)
                    {
                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add

                            foreach (FWOImage dr in input.imglst)
                            {
                                LfImage image = new LfImage();
                                image.DocumentKey = Guid.NewGuid();
                                image.Sort = dr.Id;
                                image.LastModifiedStaff = user.StaffKey;
                                image.Description = dr.FileName;
                                image.DocumentName = dr.ContentType;
                                image.LostFoundKey = intSuccess;
                                image.Signature = dr.Data;
                                s = _commondalRepository.InsertLfImage(image);

                            }
                            #endregion
                            if (s > 0)
                            {
                                message = "Record has been added.";
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        else
                        {
                            message = "Record has been added.";
                        }

                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
            }
            return message;
        }

        public async Task<string> CreateOrUpdateLostFoundImg(LostFoundDtoImgInput input)
        {
            string message = "";
            if (!AbpSession.UserId.HasValue)
            {
                //throw new AbpException("Session has expired.");
                throw new UserFriendlyException("Session has expired.");

            }
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                //throw new Exception("There is no current user!");
                throw new UserFriendlyException("Session has expired.");
            }
            else
            {
                //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());

                input.lf.StaffKey = user.StaffKey;
                if (input.lf.Area != 0)
                {
                    input.lf.Area = input.lf.Area;
                }
                if (input.lf.LostFoundStatusKey != Guid.Empty)
                {
                    input.lf.LostFoundStatusKey = input.lf.LostFoundStatusKey;
                }
                if (input.lf.OwnerRoomKey != Guid.Empty)
                {
                    input.lf.OwnerRoomKey = input.lf.OwnerRoomKey;
                }
                if (input.lf.FounderRoomKey != Guid.Empty)
                {
                    input.lf.FounderRoomKey = input.lf.FounderRoomKey;
                }

                if (input.lf.Id.HasValue)
                {
                    var o = ObjectMapper.Map<LostFound>(input.lf);
                    o.Sort = 0;
                    o.Sync = 0;
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    int intSuccess = dallostfound.Update(o);

                    if (intSuccess > 0)
                    {

                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add
                            LostFoundImageDto lfimg = new LostFoundImageDto();
                            string images = "";
                            FWOImage filteredImage1 = input.imglst.FirstOrDefault(image => image.Id == 1);
                            if (filteredImage1 != null)
                            {
                                lfimg.LostFoundImages = filteredImage1.Data;
                                images = '[' + filteredImage1.Id.ToString() + ',' + filteredImage1.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages = null;
                            }
                            FWOImage filteredImage2 = input.imglst.FirstOrDefault(image => image.Id == 2);
                            if (filteredImage2 != null)
                            {
                                lfimg.LostFoundImages2 = filteredImage2.Data;
                                images += '[' + filteredImage2.Id.ToString() + ',' + filteredImage2.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages2 = null;
                            }
                            FWOImage filteredImage3 = input.imglst.FirstOrDefault(image => image.Id == 3);
                            if (filteredImage3 != null)
                            {
                                lfimg.LostFoundImages3 = filteredImage3.Data;
                                images += '[' + filteredImage3.Id.ToString() + ',' + filteredImage3.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages3 = null;
                            }
                            FWOImage filteredImage4 = input.imglst.FirstOrDefault(image => image.Id == 4);
                            if (filteredImage4 != null)
                            {
                                lfimg.LostFoundImages4 = filteredImage4.Data;
                                images += '[' + filteredImage4.Id.ToString() + ',' + filteredImage4.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages4 = null;
                            }
                            FWOImage filteredImage5 = input.imglst.FirstOrDefault(image => image.Id == 5);
                            if (filteredImage5 != null)
                            {
                                lfimg.LostFoundImages5 = filteredImage5.Data;
                                images += '[' + filteredImage5.Id.ToString() + ',' + filteredImage5.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages5 = null;
                            }
                            lfimg.LostFoundImage = images;

                            lfimg.LostFoundKey = input.lf.Id;
                            lfimg.CreatedUser = user.StaffKey;

                            if (AbpSession.TenantId != null)
                            {
                                lfimg.TenantId = (int?)AbpSession.TenantId;
                            }


                            int exitcount = _commondalRepository.CheckLfImagehms(lfimg);
                            if (exitcount == 0)
                            {
                                lfimg.LostFoundImageKey = Guid.NewGuid();
                                s = _commondalRepository.InsertLfImagehms(lfimg);
                            }
                            else
                            {
                                string updateimages = "";
                                DataTable dt = _commondalRepository.GetDocumentByLFhmsKey(input.lf.Id.Value);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    ArrayList list = new ArrayList();
                                    if (!DBNull.Value.Equals(dr["LostFoundImage"]))
                                    {
                                        string imgref = !DBNull.Value.Equals(dr["LostFoundImage"]) ? dr["LostFoundImage"].ToString() : "";
                                        // Regex to match the pattern [id,ContentType]
                                        string pattern = @"\[(\d+),([^\]]+)\]";
                                        MatchCollection matches = Regex.Matches(imgref, pattern);

                                        foreach (Match match in matches)
                                        {
                                            var id = match.Groups[1].Value;
                                            var contentType = match.Groups[2].Value;

                                            // Add a tuple (id, ContentType) to the ArrayList
                                            list.Add((id, contentType));
                                        }
                                    }
                                    if (list.Count > 0)
                                    {
                                        if (lfimg.LostFoundImages != null)
                                        {
                                            string newContentType = filteredImage1.ContentType;
                                            string filterId = "1";
                                            bool found=false;
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                var (id, contentType) = ((string, string))list[i];

                                                if (id == filterId)
                                                {
                                                    //list.RemoveAt(i);
                                                    list[i] = (id, newContentType);
                                                    found=true;

                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                list.Add((filterId, newContentType));
                                            }
                                        }
                                        if (lfimg.LostFoundImages2 != null)
                                        {
                                            string newContentType = filteredImage2.ContentType;
                                            string filterId = "2";
                                            bool found = false;
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                var (id, contentType) = ((string, string))list[i];

                                                if (id == filterId)
                                                {
                                                    list[i] = (id, newContentType);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                list.Add((filterId, newContentType));
                                            }
                                        }
                                        if (lfimg.LostFoundImages3 != null)
                                        {
                                            string newContentType = filteredImage3.ContentType;
                                            string filterId = "3";
                                            bool found = false;
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                var (id, contentType) = ((string, string))list[i];

                                                if (id == filterId)
                                                {
                                                    list[i] = (id, newContentType);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                list.Add((filterId, newContentType));
                                            }
                                        }
                                        if (lfimg.LostFoundImages4 != null)
                                        {
                                            string newContentType = filteredImage4.ContentType;
                                            string filterId = "4";
                                            bool found = false;
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                var (id, contentType) = ((string, string))list[i];

                                                if (id == filterId)
                                                {
                                                    list[i] = (id, newContentType);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                list.Add((filterId, newContentType));
                                            }
                                        }
                                        if (lfimg.LostFoundImages5 != null)
                                        {
                                            string newContentType = filteredImage5.ContentType;
                                            string filterId = "5";
                                            bool found = false;
                                            for (int i = 0; i < list.Count; i++)
                                            {
                                                var (id, contentType) = ((string, string))list[i];

                                                if (id == filterId)
                                                {
                                                    list[i] = (id, newContentType);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                list.Add((filterId, newContentType));
                                            }
                                        }
                                    }
                                    if(list.Count > 0)
                                    {
                                        // Initialize a StringBuilder to build the resulting string
                                        StringBuilder result = new StringBuilder();

                                        // Iterate through the ArrayList and append each tuple as a string in the format [id,contentType]
                                        foreach (var item in list)
                                        {
                                            var (id, contentType) = ((string, string))item;
                                            result.Append($"[{id},{contentType}]");
                                        }
                                        updateimages = result.ToString();
                                    }
                                    else
                                    {
                                        updateimages = images;
                                    }
                                    
                                }
                                lfimg.LostFoundImage = updateimages;
                                s = _commondalRepository.UpdateLfImagehms(lfimg);
                            }


                            #endregion
                            if (s > 0)
                            {
                                message = "Record has been updated.";
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        else
                        {
                            message = "Record has been updated.";
                        }


                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to save the record.");
                    }

                }
                else
                {
                    input.lf.Id = null;
                    var o = ObjectMapper.Map<LostFound>(input.lf);
                    if (AbpSession.TenantId != null)
                    {
                        o.TenantId = (int?)AbpSession.TenantId;
                    }
                    o.Sort = 0;
                    o.Sync = 0;

                    Guid intSuccess = dallostfound.Save(o);
                    if (intSuccess != Guid.Empty)
                    {
                        if (input.imglst.Count > 0)
                        {
                            int s = 0;
                            #region img add
                            LostFoundImageDto lfimg = new LostFoundImageDto();
                            string images = "";
                            FWOImage filteredImage1 = input.imglst.FirstOrDefault(image => image.Id == 1);
                            if (filteredImage1 != null)
                            {
                                lfimg.LostFoundImages = filteredImage1.Data;
                                images = '[' + filteredImage1.Id.ToString() + ',' + filteredImage1.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages = null;
                            }
                            FWOImage filteredImage2 = input.imglst.FirstOrDefault(image => image.Id == 2);
                            if (filteredImage2 != null)
                            {
                                lfimg.LostFoundImages2 = filteredImage2.Data;
                                images += '[' + filteredImage2.Id.ToString() + ',' + filteredImage2.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages2 = null;
                            }
                            FWOImage filteredImage3 = input.imglst.FirstOrDefault(image => image.Id == 3);
                            if (filteredImage3 != null)
                            {
                                lfimg.LostFoundImages3 = filteredImage3.Data;
                                images += '[' + filteredImage3.Id.ToString() + ',' + filteredImage3.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages3 = null;
                            }
                            FWOImage filteredImage4 = input.imglst.FirstOrDefault(image => image.Id == 4);
                            if (filteredImage4 != null)
                            {
                                lfimg.LostFoundImages4 = filteredImage4.Data;
                                images += '[' + filteredImage4.Id.ToString() + ',' + filteredImage4.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages4 = null;
                            }
                            FWOImage filteredImage5 = input.imglst.FirstOrDefault(image => image.Id == 5);
                            if (filteredImage5 != null)
                            {
                                lfimg.LostFoundImages5 = filteredImage5.Data;
                                images += '[' + filteredImage5.Id.ToString() + ',' + filteredImage5.ContentType + ']';
                            }
                            else
                            {
                                lfimg.LostFoundImages5 = null;
                            }
                            lfimg.LostFoundImage = images;

                            lfimg.LostFoundKey = intSuccess;
                            lfimg.CreatedUser = user.StaffKey;

                            if (AbpSession.TenantId != null)
                            {
                                lfimg.TenantId = (int?)AbpSession.TenantId;
                            }

                            lfimg.LostFoundImageKey = Guid.NewGuid();
                            s = _commondalRepository.InsertLfImagehms(lfimg);
                            
                            #endregion
                            
                            if (s > 0)
                            {
                                message = "Record has been added.";
                            }
                            else
                            {
                                throw new UserFriendlyException("Fail to add Image.");
                            }
                        }
                        else
                        {
                            message = "Record has been added.";
                        }

                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to add the record.");
                    }

                }
            }
            return message;
        }
        public async Task<GetLostfoundForEditOutput> GetLostFoundForEdit(EntityDto<Guid> input)
        {
            var lf = await _lostfoundRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLostfoundForEditOutput { LostFound = ObjectMapper.Map<LostFoundDto>(lf) };

            return output;
        }
        public async Task<GetLostfoundForEditOutputImg> GetLostFoundForEditImgOld(EntityDto<Guid> input)
        {
            var lf = await _lostfoundRepository.FirstOrDefaultAsync(input.Id);
            List<FWOImageD> lst = new List<FWOImageD>();
            DataTable dt = _commondalRepository.GetDocumentByLFKey(input.Id);
            foreach (DataRow dr in dt.Rows)
            {
                FWOImageD o = new FWOImageD();
                //o.Id = Convert.ToInt32(dr["sort"]);
                //o.FileName = dr["Description"].ToString();
                //o.ContentType = dr["Document"].ToString();
                //o.Data = dr["Signature"] as byte[];
                o.Id = !DBNull.Value.Equals(dr["sort"]) ? Convert.ToInt32(dr["sort"]) : 0;
                o.FileName = !DBNull.Value.Equals(dr["Description"]) ? dr["Description"].ToString() : "";
                o.ContentType = !DBNull.Value.Equals(dr["Document"]) ? dr["Document"].ToString() : "";
                o.Data = !DBNull.Value.Equals(dr["Signature"]) ? dr["Signature"] as byte[] : Array.Empty<byte>();
                var base64Image = Convert.ToBase64String(o.Data);
                o.imageSrc = $"data:{o.ContentType};base64,{base64Image}";
                lst.Add(o);
            }
           

            var output = new GetLostfoundForEditOutputImg { LostFound = ObjectMapper.Map<LostFoundDto>(lf),imglst=lst };

            return output;
        }
        public async Task<GetLostfoundForEditOutputImg> GetLostFoundForEditImg(EntityDto<Guid> input)
        {
            var lf = await _lostfoundRepository.FirstOrDefaultAsync(input.Id);
            List<FWOImageD> lst = new List<FWOImageD>();
            DataTable dt = _commondalRepository.GetDocumentByLFhmsKey(input.Id);
            foreach (DataRow dr in dt.Rows)
            {
                ArrayList list = new ArrayList();
                if (!DBNull.Value.Equals(dr["LostFoundImage"]))
                {
                    string imgref = !DBNull.Value.Equals(dr["LostFoundImage"]) ? dr["LostFoundImage"].ToString() : "";
                    // Regex to match the pattern [id,ContentType]
                    string pattern = @"\[(\d+),([^\]]+)\]";
                    MatchCollection matches = Regex.Matches(imgref, pattern);

                    foreach (Match match in matches)
                    {
                        var id = match.Groups[1].Value;
                        var contentType = match.Groups[2].Value;

                        // Add a tuple (id, ContentType) to the ArrayList
                        list.Add((id, contentType));
                    }
                }
                if (list.Count > 0)
                {
                    
                    if (!DBNull.Value.Equals(dr["LostFoundImages"]))
                    {
                        FWOImageD o1 = new FWOImageD();
                        o1.Id = 1;
                        string filterId = "1";
                        o1.ContentType = "";
                        o1.FileName = "LostFound Image";
                        for (int i = 0; i < list.Count; i++)
                        {
                            var (id, contentType) = ((string, string))list[i];

                            if (id == filterId)
                            {
                                o1.ContentType = contentType;
                                break;
                            }
                        }
                        
                        //string filterId = "1";
                        //var contentTypeResult = enumerableList.FirstOrDefault(item => item.Item1 == filterId)?.Item2;
                        //if (contentTypeResult != null)
                        //{
                        //    o1.ContentType = contentTypeResult;
                        //}
                        //else
                        //{
                        //    o1.ContentType = "";
                        //}
                        o1.Data = !DBNull.Value.Equals(dr["LostFoundImages"]) ? dr["LostFoundImages"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o1.Data);
                        o1.imageSrc = $"data:{o1.ContentType};base64,{base64Image}";
                        lst.Add(o1);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages2"]))
                    {
                        FWOImageD o2 = new FWOImageD();
                        o2.Id = 2;
                        string filterId = "2";
                        o2.ContentType = "";
                        o2.FileName = "LostFound Image";
                        for (int i = 0; i < list.Count; i++)
                        {
                            var (id, contentType) = ((string, string))list[i];

                            if (id == filterId)
                            {
                                o2.ContentType = contentType;
                                break;
                            }
                        }
                        o2.Data = !DBNull.Value.Equals(dr["LostFoundImages2"]) ? dr["LostFoundImages2"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o2.Data);
                        o2.imageSrc = $"data:{o2.ContentType};base64,{base64Image}";
                        lst.Add(o2);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages3"]))
                    {
                        FWOImageD o3 = new FWOImageD();
                        o3.Id = 3;
                        string filterId = "3";
                        o3.ContentType = "";
                        o3.FileName = "LostFound Image";
                        for (int i = 0; i < list.Count; i++)
                        {
                            var (id, contentType) = ((string, string))list[i];

                            if (id == filterId)
                            {
                                o3.ContentType = contentType;
                                break;
                            }
                        }
                        o3.Data = !DBNull.Value.Equals(dr["LostFoundImages3"]) ? dr["LostFoundImages3"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o3.Data);
                        o3.imageSrc = $"data:{o3.ContentType};base64,{base64Image}";
                        lst.Add(o3);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages4"]))
                    {
                        FWOImageD o4 = new FWOImageD();
                        o4.Id = 4;
                        string filterId = "4";
                        o4.ContentType = "";
                        o4.FileName = "LostFound Image";
                        for (int i = 0; i < list.Count; i++)
                        {
                            var (id, contentType) = ((string, string))list[i];

                            if (id == filterId)
                            {
                                o4.ContentType = contentType;
                                break;
                            }
                        }
                        o4.Data = !DBNull.Value.Equals(dr["LostFoundImages4"]) ? dr["LostFoundImages4"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o4.Data);
                        o4.imageSrc = $"data:{o4.ContentType};base64,{base64Image}";
                        lst.Add(o4);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages5"]))
                    {
                        FWOImageD o5 = new FWOImageD();
                        o5.Id = 5;
                        string filterId = "5";
                        o5.ContentType = "";
                        o5.FileName = "LostFound Image";
                        for (int i = 0; i < list.Count; i++)
                        {
                            var (id, contentType) = ((string, string))list[i];

                            if (id == filterId)
                            {
                                o5.ContentType = contentType;
                                break;
                            }
                        }
                        o5.Data = !DBNull.Value.Equals(dr["LostFoundImages5"]) ? dr["LostFoundImages5"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o5.Data);
                        o5.imageSrc = $"data:{o5.ContentType};base64,{base64Image}";
                        lst.Add(o5);
                    }
                }
                else
                {
                    if (!DBNull.Value.Equals(dr["LostFoundImages"]))
                    {
                        FWOImageD o1 = new FWOImageD();
                        o1.Id = 1;
                        o1.ContentType = "image/png";
                        o1.FileName = "LostFound Image";
                        o1.Data = !DBNull.Value.Equals(dr["LostFoundImages"]) ? dr["LostFoundImages"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o1.Data);
                        o1.imageSrc = $"data:{o1.ContentType};base64,{base64Image}";
                        lst.Add(o1);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages2"]))
                    {
                        FWOImageD o2 = new FWOImageD();
                        o2.Id = 2;
                        o2.ContentType = "image/png";
                        o2.FileName = "LostFound Image";
                        o2.Data = !DBNull.Value.Equals(dr["LostFoundImages2"]) ? dr["LostFoundImages2"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o2.Data);
                        o2.imageSrc = $"data:{o2.ContentType};base64,{base64Image}";
                        lst.Add(o2);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages3"]))
                    {
                        FWOImageD o3 = new FWOImageD();
                        o3.Id = 3;
                        o3.ContentType = "image/png";
                        o3.FileName = "LostFound Image";
                        o3.Data = !DBNull.Value.Equals(dr["LostFoundImages3"]) ? dr["LostFoundImages3"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o3.Data);
                        o3.imageSrc = $"data:{o3.ContentType};base64,{base64Image}";
                        lst.Add(o3);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages4"]))
                    {
                        FWOImageD o4 = new FWOImageD();
                        o4.Id = 4;
                        o4.ContentType = "image/png";
                        o4.FileName = "LostFound Image";
                        o4.Data = !DBNull.Value.Equals(dr["LostFoundImages4"]) ? dr["LostFoundImages4"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o4.Data);
                        o4.imageSrc = $"data:{o4.ContentType};base64,{base64Image}";
                        lst.Add(o4);
                    }
                    if (!DBNull.Value.Equals(dr["LostFoundImages5"]))
                    {
                        FWOImageD o5 = new FWOImageD();
                        o5.Id = 5;
                        o5.ContentType = "image/png";
                        o5.FileName = "LostFound Image";
                        o5.Data = !DBNull.Value.Equals(dr["LostFoundImages5"]) ? dr["LostFoundImages5"] as byte[] : Array.Empty<byte>();
                        var base64Image = Convert.ToBase64String(o5.Data);
                        o5.imageSrc = $"data:{o5.ContentType};base64,{base64Image}";
                        lst.Add(o5);
                    }
                }
            }


            var output = new GetLostfoundForEditOutputImg { LostFound = ObjectMapper.Map<LostFoundDto>(lf), imglst = lst };

            return output;
        }
        #endregion
    }
}
