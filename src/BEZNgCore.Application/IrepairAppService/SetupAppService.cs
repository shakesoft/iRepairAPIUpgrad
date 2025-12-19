using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BEZNgCore.CustomizeRepository;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using System.Net.NetworkInformation;
using Abp.Runtime.Session;

namespace BEZNgCore.IrepairAppService
{
    public class SetupAppService : BEZNgCoreAppServiceBase
    {
        //private readonly IRepository<MWorkOrderStatus, int> _mworkorderstatusRepository;
        //private readonly IRepository<MWorkType, int> _mworktypeRepository;
        //private readonly IRepository<MArea, int> _areaRepository;
        //private readonly IRepository<MWorkTimeSheetNoteTemplate, int> _mworktimesheetnoteRepository;
        //private readonly IRepository<MPriority, int> _mpriorityRepository;
        //private readonly IRepository<MTechnician, int> _mtechnicianRepository;
        private readonly ISetupdalRepository _setupdalRepository;
        public SetupAppService(
            //IRepository<MWorkOrderStatus, int> mworkorderstatusRepository,
            //IRepository<MWorkType, int> mworktypeRepository,
            //IRepository<MArea, int> areaRepository,
            //IRepository<MWorkTimeSheetNoteTemplate, int> mworktimesheetnoteRepository,
            //IRepository<MPriority, int> mpriorityRepository,
            //IRepository<MTechnician, int> mtechnicianRepository,
            ISetupdalRepository setupdalRepository)
        {
            //_mworkorderstatusRepository = mworkorderstatusRepository;
            //_mworktypeRepository = mworktypeRepository;
            //_areaRepository = areaRepository;
            //_mworktimesheetnoteRepository = mworktimesheetnoteRepository;
            //_mpriorityRepository = mpriorityRepository;
            //_mtechnicianRepository = mtechnicianRepository;
            _setupdalRepository = setupdalRepository;
        }
        [HttpGet]
        public ListResultDto<SetupViewData> GetSetupViewData(bool chkInactive = false)
        {
            List<SetupViewData> Alllst = new List<SetupViewData>();
            SetupViewData a = new SetupViewData();
            a.Status = GetAllStatus(chkInactive);
            a.Type = GetAllType(chkInactive);
            a.Area = GetAllArea(chkInactive);
            a.Template = GetAllTemplate(chkInactive);
            a.Priority = GetAllPriority(chkInactive);
            a.Technician = GetAllTechnician(chkInactive);
            Alllst.Add(a);
            return new ListResultDto<SetupViewData>(Alllst);
        }
        [HttpGet]
        public ListResultDto<SetupTabOutput> GetStatusCheckChangeData(bool chkInactive = false)
        {
            List<SetupTabOutput> Alllst = new List<SetupTabOutput>();
            Alllst = GetAllStatus(chkInactive);
            return new ListResultDto<SetupTabOutput>(Alllst);
        }
        [HttpGet]
        public ListResultDto<SetupTabOutput> GetTypeCheckChangeData(bool chkInactive = false)
        {
            List<SetupTabOutput> Alllst = new List<SetupTabOutput>();
            Alllst = GetAllType(chkInactive);
            return new ListResultDto<SetupTabOutput>(Alllst);
        }
        [HttpGet]
        public ListResultDto<SetupTabOutput> GetAreaCheckChangeData(bool chkInactive = false)
        {
            List<SetupTabOutput> Alllst = new List<SetupTabOutput>();
            Alllst = GetAllArea(chkInactive);
            return new ListResultDto<SetupTabOutput>(Alllst);
        }
        [HttpGet]
        public ListResultDto<SetupTabOutput> GetTemplateCheckChangeData(bool chkInactive = false)
        {
            List<SetupTabOutput> Alllst = new List<SetupTabOutput>();
            Alllst = GetAllTemplate(chkInactive);
            return new ListResultDto<SetupTabOutput>(Alllst);
        }
        [HttpGet]
        public ListResultDto<MPriorityOutput> GetPriorityCheckChangeData(bool chkInactive = false)
        {
            List<MPriorityOutput> Alllst = new List<MPriorityOutput>();
            Alllst = GetAllPriority(chkInactive);
            return new ListResultDto<MPriorityOutput>(Alllst);
        }
        [HttpGet]
        public ListResultDto<MTechnicianOutput> GetTechnicianCheckChangeData(bool chkInactive = false)
        {
            List<MTechnicianOutput> Alllst = new List<MTechnicianOutput>();
            Alllst = GetAllTechnician(chkInactive);

            return new ListResultDto<MTechnicianOutput>(Alllst);
        }

        protected List<SetupTabOutput> GetAllStatus(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllWorkOrderStatus();
            else

                return _setupdalRepository.GetActiveWorkOrderStatus();
        }
        protected List<SetupTabOutput> GetAllType(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllMWorkType();
            else
                return _setupdalRepository.GetActiveMWorkType();
        }
        protected List<SetupTabOutput> GetAllArea(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllMArea();
            else
                return _setupdalRepository.GetActiveMArea();
        }
        protected List<SetupTabOutput> GetAllTemplate(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllMWorkTimeSheetNoteTemplate();
            else
                return _setupdalRepository.GetActiveMWorkTimeSheetNoteTemplate();
        }
        //protected List<MPriorityOutput> GetAllPriority()
        //{
        //    return _setupdalRepository.GetAllPriority();
        //}
        protected List<MPriorityOutput> GetAllPriority(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllPriority();
            else

                return _setupdalRepository.GetActivePriority();
        }
        protected List<MTechnicianOutput> GetAllTechnician(bool chkInactive)
        {
            if (chkInactive)
                return _setupdalRepository.GetAllMTechnician();
            else
                return _setupdalRepository.GetActiveMTechnician();
        }
        [HttpGet]
        public SetupTabInput SetupPopupViewdata(string type)
        {
            SetupTabInput o = new SetupTabInput();
            if (!string.IsNullOrEmpty(type))
            {

                o.litType = type;

                if (type.Equals("worktype"))
                {
                    o.litTitle = "Add Work Type ";

                }
                else if (type.Equals("workarea"))
                {
                    o.litTitle = "Add Work Area ";

                }
                else if (type.Equals("note"))
                {
                    o.litTitle = "Add Template Note";

                }
                else if (type.Equals("workstatus"))
                {
                    o.litTitle = "Add Work Status";
                }
                else if (type.Equals("priority"))
                {
                    o.litTitle = "Add Priority";
                }
            }

            return o;
        }
        #region popupaddbutton
        [HttpPost]
        public async Task<string> SetupPopupAdd(SetupTabInput input)
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
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());


                try
                {


                    if (input.litType.Equals("worktype"))
                    {
                        MWorkType workType = new MWorkType();
                        workType.Description = input.Description.Trim();
                        workType.Active = (input.chkActive) ? 1 : 0;
                        workType.CreatedBy = user.StaffKey;
                        if (AbpSession.TenantId != null)
                        {
                            workType.TenantId = (int)AbpSession.TenantId;
                        }

                        int intSuccessful = InsertMWorkType(workType);
                        if (intSuccessful > 0)
                        {
                            message = "Record has been added.";

                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to add the record.");
                        }

                    }
                    else if (input.litType.Equals("workarea"))
                    {
                        MArea workArea = new MArea();
                        workArea.Description = input.Description.Trim();
                        workArea.Active = (input.chkActive) ? 1 : 0;
                        workArea.CreatedBy = user.StaffKey;
                        if (AbpSession.TenantId != null)
                        {
                            workArea.TenantId = (int)AbpSession.TenantId;
                        }


                        int intSuccessful = InsertMArea(workArea);
                        if (intSuccessful > 0)
                        {
                            message = "Record has been added.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to add the record.");
                        }
                    }
                    else if (input.litType.Equals("note"))
                    {
                        MWorkTimeSheetNoteTemplate note = new MWorkTimeSheetNoteTemplate();
                        note.Description = input.Description.Trim();
                        note.Active = (input.chkActive) ? 1 : 0;
                        note.CreatedBy = user.StaffKey;

                        if (AbpSession.TenantId != null)
                        {
                            note.TenantId = (int)AbpSession.TenantId;
                        }
                        int intSuccessful = InsertMWorkTimeSheetNoteTemplate(note);
                        if (intSuccessful > 0)
                        {
                            message = "Record has been added.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to add the record.");
                        }
                    }
                    else if (input.litType.Equals("workstatus"))
                    {
                        MWorkOrderStatus status = new MWorkOrderStatus();
                        status.Description = input.Description.Trim();
                        status.Active = (input.chkActive) ? 1 : 0;
                        status.CreatedBy = user.StaffKey;

                        if (AbpSession.TenantId != null)
                        {
                            status.TenantId = (int)AbpSession.TenantId;
                        }
                        int intSuccessful = InsertMWorkOrderStatus(status);
                        if (intSuccessful > 0)
                        {
                            message = "Record has been added.";

                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to add the record.");
                        }
                    }
                    else if (input.litType.Equals("priority"))
                    {
                        MPriorityModel priority = new MPriorityModel();
                        priority.Priority = input.Description.Trim();
                        priority.Sort = GetMaxSort() + 1;
                        priority.Active = (input.chkActive) ? 1 : 0;
                        if (AbpSession.TenantId != null)
                        {
                            priority.TenantId = (int)AbpSession.TenantId;
                        }
                        int intSuccessful = InsertMPriority(priority);
                        if (intSuccessful > 0)
                        {
                            message = "Record has been added.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to add the record.");
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;

        }
        private int GetMaxSort()
        {
            return _setupdalRepository.GetMaxSort();
        }

        private int InsertMPriority(MPriorityModel priority)
        {
            return _setupdalRepository.InsertMPriority(priority);
        }

        private int InsertMWorkOrderStatus(MWorkOrderStatus status)
        {
            return _setupdalRepository.InsertMWorkOrderStatus(status);
        }

        private int InsertMWorkTimeSheetNoteTemplate(MWorkTimeSheetNoteTemplate note)
        {
            return _setupdalRepository.InsertMWorkTimeSheetNoteTemplate(note);
        }

        private int InsertMArea(MArea workArea)
        {
            return _setupdalRepository.InsertMArea(workArea);
        }

        private int InsertMWorkType(MWorkType workType)
        {
            return _setupdalRepository.InsertMWorkType(workType);
        }
        #endregion
        #region Status
        [HttpPost]
        public async Task<string> btnSaveStatus(SetupViewData input)
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
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                try
                {

                    List<MWorkOrderStatus> listWOStatus = new List<MWorkOrderStatus>();
                    MWorkOrderStatus WOStatus;
                    if (input.Status.Count > 0)
                    {
                        foreach (SetupTabOutput item in input.Status)
                        {

                            WOStatus = new MWorkOrderStatus();
                            WOStatus.Id = item.Seqno;
                            WOStatus.Description = item.Description;
                            WOStatus.Active = (item.ActiveStatus) ? 1 : 0;
                            WOStatus.ModifiedBy = user.StaffKey;
                            if (AbpSession.TenantId != null)
                            {
                                WOStatus.TenantId = (int)AbpSession.TenantId;
                            }
                            listWOStatus.Add(WOStatus);
                        }

                        int intSuccess = UpdateMWorkOrderStatus(listWOStatus);
                        if (intSuccess > 0)
                        {
                            message = "Records have been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to update");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;
        }

        private int UpdateMWorkOrderStatus(List<MWorkOrderStatus> listWOStatus)
        {
            int intSuccessful = 0;
            foreach (MWorkOrderStatus woStatus in listWOStatus)
            {
                intSuccessful = _setupdalRepository.UpdateMWorkOrderStatus(woStatus);
            }
            return intSuccessful;
        }
        #endregion
        #region Type
        [HttpPost]
        public async Task<string> btnSaveType(SetupViewData input)
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
                // var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                try
                {

                    List<MWorkType> listWorkType = new List<MWorkType>();
                    MWorkType worktype;
                    if (input.Type.Count > 0)
                    {
                        foreach (SetupTabOutput item in input.Type)
                        {

                            worktype = new MWorkType();
                            worktype.Id = item.Seqno;
                            worktype.Description = item.Description;
                            worktype.Active = (item.ActiveStatus) ? 1 : 0;
                            worktype.ModifiedBy = user.StaffKey;
                            if (AbpSession.TenantId != null)
                            {
                                worktype.TenantId = (int)AbpSession.TenantId;
                            }
                            listWorkType.Add(worktype);
                        }

                        int intSuccess = UpdateMWorkType(listWorkType);
                        if (intSuccess > 0)
                        {
                            message = "Records have been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to update");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;
        }
        private int UpdateMWorkType(List<MWorkType> listWorkType)
        {
            int intSuccessful = 0;
            foreach (MWorkType wotype in listWorkType)
            {
                intSuccessful = _setupdalRepository.UpdateMWorkType(wotype);
            }
            return intSuccessful;
        }
        #endregion
        #region Area
        [HttpPost]
        public async Task<string> btnSaveArea(SetupViewData input)
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
                //  var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());

                try
                {

                    List<MArea> listArea = new List<MArea>();
                    MArea workArea;
                    if (input.Area.Count > 0)
                    {
                        foreach (SetupTabOutput item in input.Area)
                        {

                            workArea = new MArea();
                            workArea.Id = item.Seqno;
                            workArea.Description = item.Description;
                            workArea.Active = (item.ActiveStatus) ? 1 : 0;
                            workArea.ModifiedBy = user.StaffKey;
                            if (AbpSession.TenantId != null)
                            {
                                workArea.TenantId = (int)AbpSession.TenantId;
                            }
                            listArea.Add(workArea);
                        }

                        int intSuccess = UpdateMArea(listArea);
                        if (intSuccess > 0)
                        {
                            message = "Records have been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to update");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;
        }
        private int UpdateMArea(List<MArea> listarea)
        {
            int intSuccessful = 0;
            foreach (MArea area in listarea)
            {
                intSuccessful = _setupdalRepository.UpdateMArea(area);
            }
            return intSuccessful;
        }
        #endregion
        #region Template
        [HttpPost]
        public async Task<string> btnSaveTemplate(SetupViewData input)
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

                try
                {

                    List<MWorkTimeSheetNoteTemplate> listWorkNote = new List<MWorkTimeSheetNoteTemplate>();
                    MWorkTimeSheetNoteTemplate workNote;
                    if (input.Template.Count > 0)
                    {
                        foreach (SetupTabOutput item in input.Template)
                        {

                            workNote = new MWorkTimeSheetNoteTemplate();
                            workNote.Id = item.Seqno;
                            workNote.Description = item.Description;
                            workNote.Active = (item.ActiveStatus) ? 1 : 0;
                            workNote.ModifiedBy = user.StaffKey;
                            if (AbpSession.TenantId != null)
                            {
                                workNote.TenantId = (int)AbpSession.TenantId;
                            }
                            listWorkNote.Add(workNote);
                        }

                        int intSuccess = UpdateMWorkTimeSheetNoteTemplate(listWorkNote);
                        if (intSuccess > 0)
                        {
                            message = "Records have been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to update");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;
        }
        private int UpdateMWorkTimeSheetNoteTemplate(List<MWorkTimeSheetNoteTemplate> listWorkNote)
        {
            int intSuccessful = 0;
            foreach (MWorkTimeSheetNoteTemplate note in listWorkNote)
            {
                intSuccessful = _setupdalRepository.UpdateMWorkTimeSheetNoteTemplate(note);
            }
            return intSuccessful;
        }
        #endregion;
        #region Priority
        [HttpPost]
        public async Task<string> btnSavePriority(SetupViewData input)
        {
            string message = "";
            try
            {

                List<MPriorityModel> listPriority = new List<MPriorityModel>();
                MPriorityModel workArea;
                if (input.Priority.Count > 0)
                {
                    foreach (MPriorityOutput item in input.Priority)
                    {
                        workArea = new MPriorityModel();
                        workArea.PriorityID = item.PriorityID;
                        workArea.Priority = item.Priority;
                        workArea.Sort = item.Sort;
                        workArea.Active = (item.ActiveStatus) ? 1 : 0;
                        if (AbpSession.TenantId != null)
                        {
                            workArea.TenantId = (int)AbpSession.TenantId;
                        }
                        listPriority.Add(workArea);

                    }

                    int intSuccess = UpdateMPriority(listPriority);
                    if (intSuccess > 0)
                    {
                        message = "Records have been saved.";
                    }
                    else
                    {
                        throw new UserFriendlyException("Fail to update");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }

            return message;
        }
        private int UpdateMPriority(List<MPriorityModel> listPriority)
        {
            int intSuccessful = 0;
            foreach (MPriorityModel mp in listPriority)
            {
                intSuccessful = _setupdalRepository.UpdateMPriority(mp);
            }
            return intSuccessful;
        }
        #endregion
        #region Technician
        [HttpPost]
        public async Task<string> btnSaveTechnician(SetupViewData input)
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
                try
                {

                    List<MTechnician> listTechnician = new List<MTechnician>();
                    MTechnician technician;
                    if (input.Technician.Count > 0)
                    {
                        foreach (MTechnicianOutput item in input.Technician)
                        {

                            technician = new MTechnician();
                            technician.Id = item.Seqno;
                            technician.Active = (item.ActiveStatus) ? 1 : 0;
                            technician.ModifiedBy = user.StaffKey;
                            if (AbpSession.TenantId != null)
                            {
                                technician.TenantId = (int)AbpSession.TenantId;
                            }
                            listTechnician.Add(technician);
                        }

                        int intSuccess = UpdateMTechnician(listTechnician);
                        if (intSuccess > 0)
                        {
                            message = "Records have been saved.";
                        }
                        else
                        {
                            throw new UserFriendlyException("Fail to update");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message.ToString());
                }
            }
            return message;

        }
        private int UpdateMTechnician(List<MTechnician> listTechnician)
        {
            int intSuccessful = 0;
            foreach (MTechnician techn in listTechnician)
            {
                intSuccessful = _setupdalRepository.UpdateMTechnician(techn);
            }
            return intSuccessful;
        }
        #endregion
    }
}
