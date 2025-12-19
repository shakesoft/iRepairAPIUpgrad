using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using BEZNgCore.IRepairIAppService.Dto;
using BEZNgCore.IrepairModel;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BEZNgCore.IrepairAppService
{
    public class MsgNotificationAppService : BEZNgCoreAppServiceBase
    {
        private readonly IRepository<SqoopeMsgLog, Guid> _SqoopeMsgLogRepository;
        private readonly IRepository<Staff, Guid> _StaffRepository;

        public MsgNotificationAppService(
            IRepository<SqoopeMsgLog, Guid> SqoopeMsgLogRepository,
            IRepository<Staff, Guid> StaffRepository)
        {
            _SqoopeMsgLogRepository = SqoopeMsgLogRepository;
            _StaffRepository = StaffRepository;
        }
        [HttpGet]
        public async Task<ListResultDto<SqoopeMsgLogOutput>> GetNotiReadAsync()
        {
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
                var v = _SqoopeMsgLogRepository.GetAll()
               .Where(x => x.ToStaffKey == user.StaffKey)
               .OrderBy(x => x.CreatedOn)
               .Select(x => new SqoopeMsgLogOutput
               {
                   Id = x.Id,
                   Msg = x.Msg,
                   FromStaffKey = x.CreatedBy,
                   CreatedOn = x.CreatedOn,
                   Read = x.Read,
                   Send = x.Send,
                   ToStaffKey = x.ToStaffKey
               })
              .ToList();

                return new ListResultDto<SqoopeMsgLogOutput>(v);
            }
        }
        [HttpGet]
        public async Task<ListResultDto<SqoopeMsgLogOutput>> GetNotiSendAsync()
        {
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
                var v = _SqoopeMsgLogRepository.GetAll()
                .Where(x => x.Send == false && x.ToStaffKey == user.StaffKey)
                .OrderBy(x => x.CreatedOn)
                .Select(x => new SqoopeMsgLogOutput
                {
                    Id = x.Id,
                    Msg = x.Msg,
                    FromStaffKey = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Read = x.Read,
                    Send = x.Send,
                    ToStaffKey = x.ToStaffKey
                })
               .ToList();
                return new ListResultDto<SqoopeMsgLogOutput>(v);
            }
        }
        [HttpPost]
        public async Task SendUpdate(string notiid)
        {
            try
            {
                await UpdateMsg("Send", notiid);


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
        [HttpPost]
        public async Task ReadUpdate(string notiid)
        {
            try
            {
                await UpdateMsg("Read", notiid);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
        private async Task UpdateMsg(string status, string notiid)
        {
            try
            {
                Guid id = Guid.Parse(notiid);
                var Msg = _SqoopeMsgLogRepository.GetAll().Where(x => x.Id == id).Select(x => x).ToList();
                if (Msg.Count > 0)
                {
                    if (status == "Read")
                    {
                        Msg[0].Read = true;
                    }
                    else if (status == "Send")
                    {
                        Msg[0].Send = true;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
        //[HttpPut]
        [HttpPost]
        public async Task UpdateFirebaseToken(string fbk)
        {
            try
            {
                if (!string.IsNullOrEmpty(fbk))
                {
                    var sqopmsg = _StaffRepository.GetAll().Where(x => x.FirebaseToken_Id == fbk).Select(x => x).ToList();
                    if (sqopmsg.Count > 0)
                    {

                        sqopmsg.ForEach(u => u.FirebaseToken_Id = null);

                    }
                    var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                    var staffuser = _StaffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x).ToList();
                    if (staffuser.Count > 0)
                    {

                        staffuser[0].FirebaseToken_Id = fbk;

                    }
                }
                else {
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

                        var staffuser = _StaffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x).ToList();
                        if (staffuser.Count > 0)
                        {

                            staffuser[0].FirebaseToken_Id = null;

                        }
                    }
                }
               
                //var sqopmsg = _SqoopeMsgLogRepository.GetAll().Where(x => x.ToStaffKey == user.Result.StaffKey).Select(x => x).ToList();
                //if (sqopmsg.Count > 0)
                //{

                //    sqopmsg.ForEach(u => u.FirebaseToken_Id = fbk);

                //}


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
            //}
        }

        [HttpPost]
        public async Task UpdateFirebaseTokeniRepair(string fbk)
        {
            try
            {
                if (!string.IsNullOrEmpty(fbk))
                {
                    var sqopmsg = _StaffRepository.GetAll().Where(x => x.FirebaseToken_IdiRepair == fbk).Select(x => x).ToList();
                    if (sqopmsg.Count > 0)
                    {

                        sqopmsg.ForEach(u => u.FirebaseToken_IdiRepair = null);

                    }
                    var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
                    var staffuser = _StaffRepository.GetAll().Where(x => x.Id == user.Result.StaffKey).Select(x => x).ToList();
                    if (staffuser.Count > 0)
                    {

                        staffuser[0].FirebaseToken_IdiRepair = fbk;

                    }
                }
                else
                {
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

                        var staffuser = _StaffRepository.GetAll().Where(x => x.Id == user.StaffKey).Select(x => x).ToList();
                        if (staffuser.Count > 0)
                        {

                            staffuser[0].FirebaseToken_IdiRepair = null;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<List<MessageNotiView>> GetMessageGroupBy()
        {
            var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
            List<MessageNotiView> Alllst = new List<MessageNotiView>();
            listSqoopeMsgLog = _SqoopeMsgLogRepository.GetAll()
                .Where(x => x.CreatedBy == user.Result.StaffKey && x.ToStaffKey != null)
                .Select(x => new SqoopeMsgLog
                {
                    Id = x.Id,
                    SqoopeMessageKey = x.SqoopeMessageKey,
                    FromContactId = x.FromContactId,
                    CreatedBy = x.CreatedBy,
                    ToContactId = x.ToContactId,
                    Msg = x.Msg,
                    ToStaffKey = x.ToStaffKey,
                    FirebaseToken_Id = x.FirebaseToken_Id
                })
               .ToList();
            Alllst = listSqoopeMsgLog.GroupBy(u => u.Msg)
     .Select(g => new MessageNotiView
     {
         Message = g.Key,
         ToStaffList = g.Select(tb => new ToStaffList { to = tb.FirebaseToken_Id }).ToList()
     }).ToList();

            return Alllst;
        }
        [HttpGet]
        public async Task<List<MessageNotiViewLatest>> GetMessageGroupByLatest()
        {
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
                List<SqoopeMsgLog> listSqoopeMsgLog = new List<SqoopeMsgLog>();
                List<MessageNotiView> Alllst = new List<MessageNotiView>();

                List<MessageNotiViewLatest> Alllstlatest = new List<MessageNotiViewLatest>();
                listSqoopeMsgLog = _SqoopeMsgLogRepository.GetAll()
                    .Where(x => x.CreatedBy == user.StaffKey && x.ToStaffKey != null && x.FirebaseToken_Id != null)
                    .Select(x => new SqoopeMsgLog
                    {
                        Id = x.Id,
                        SqoopeMessageKey = x.SqoopeMessageKey,
                        FromContactId = x.FromContactId,
                        CreatedBy = x.CreatedBy,
                        ToContactId = x.ToContactId,
                        Msg = x.Msg,
                        ToStaffKey = x.ToStaffKey,
                        FirebaseToken_Id = x.FirebaseToken_Id
                    })
                   .ToList();
                Alllst = listSqoopeMsgLog.GroupBy(u => u.Msg)
         .Select(g => new MessageNotiView
         {
             Message = g.Key,
             ToStaffList = g.Select(tb => new ToStaffList { to = tb.FirebaseToken_Id }).ToList()
         }).ToList();

                foreach (var v in Alllst)
                {
                    MessageNotiViewLatest vc = new MessageNotiViewLatest();
                    if (v.ToStaffList.Count > 0)
                    {
                        string[] too = v.ToStaffList.Select(x => x.to).ToArray();
                        vc.to = too;
                    }

                    vc.Message = v.Message;


                    Alllstlatest.Add(vc);
                }
                return Alllstlatest;
            }
        }

        [HttpPost]
        public async Task<string> SendNotiTest()
        {
            string strReturnValue = "";

            string url = "https://fcm.googleapis.com/fcm/send";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "key=AAAAKyWo4oc:APA91bHqkfMO1M_t_thtJuwpuSKN8qadqR4Eqy5mOKFCLw3exIWjSYXHLuPZ3zuvS5pgFKfVmmlzmUSDERkN8PZVhN6iM1bVZdWRi4L5H8wHyq-G87aLD2GtNDUbAhORgtE1fXyuyzWH");
            var noti = new FbNoti();

            string[] str1 = new string[3] {"eHSztrUX-0fElM4z9MStBP:APA91bHvJILmwf8Zeez0LZim1CMLO7LUUTxZjazL6XJh7cK5tmuT2JzJlQt51_8e5Eh0tadAuniBTpNyyZhy3hZuMwfLLfmvxed5PVINruHIoZ5iCeercJgizxR3Rq4bY1NefefxB1_K",
            "eJivDZE0SSucHfjjt_K7gi:APA91bGi3SEEkYKm62hDneIMS4wrEqOfabV6FNc9l-dUG1W94Eli8uxjvcd24sy8GxOpgnDC_lhBRHMA3KFSjuBnov2KKxJzdEjth5V_DB5iO2sWTYosjT1igM83Urc3ZI5_s4n_eci9","epeoNZH2SNGovEmiER7A5d:APA91bHMgkZcH4W5mnt7BtRNMvCrOAzm1tUBtXb7lah31C0-rdoEthYrkpduGVKJvdDYdIUUZNmvxcUd7k47RzWCvHdpF95nZkGhNqo5SF0OsgjIks4ebEKZwWFhz-O_bW-1owVnYNzH"};
            noti.registration_ids = str1;
            Notification n = new Notification();
            n.body = "Testing From Web Send Noti";
            n.title = "Testing From Web";
            noti.notification = n;
            var json = JsonSerializer.Serialize(noti);
            var data = Encoding.UTF8.GetBytes(json);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            //if(response.StatusCode.ToString()=="200")
            //{
            //    strReturnValue = "success";
            //}
            //else
            //{
            //    strReturnValue = "Failed";
            //}
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            strReturnValue = responseString.ToString();
            return strReturnValue;
        }
        [HttpPost]
        public async Task<string> SendNotiOld(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                string url = "https://fcm.googleapis.com/fcm/send";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "key=AAAAKyWo4oc:APA91bHqkfMO1M_t_thtJuwpuSKN8qadqR4Eqy5mOKFCLw3exIWjSYXHLuPZ3zuvS5pgFKfVmmlzmUSDERkN8PZVhN6iM1bVZdWRi4L5H8wHyq-G87aLD2GtNDUbAhORgtE1fXyuyzWH");
                var noti = new FbNoti();
                noti.registration_ids = input.to;
                Notification n = new Notification();
                n.body = input.Message;
                n.title = input.Title;
                noti.notification = n;
                var json = JsonSerializer.Serialize(noti);
                var data = Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                strReturnValue = responseString.ToString();
            }

            return strReturnValue;
        }
        public async Task<string> SendNotiiClean(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                var credential = GoogleCredential.FromFile("iclean-362305-a5691d33f307.json")//C:/inetpub/wwwroot/iRepairAPI/
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
                
                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string url = "https://fcm.googleapis.com/v1/projects/iclean-362305/messages:send";

                #region send multiple device
                for (int i = 0; i < input.to.Length; i++)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        var message = new
                        {
                            message = new
                            {
                                token = input.to[i],
                                notification = new
                                {
                                    title = input.Title,
                                    body = input.Message
                                }
                            }
                        };

                        var jsonMessage = JsonSerializer.Serialize(message);
                        var httpContent = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(url, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            strReturnValue = "success";
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            strReturnValue = $"Error: {response.StatusCode} - {errorContent}";
                        }
                    }

                }
                #endregion
            }

            return strReturnValue;
        }

        public async Task<string> SendNotiiRepair(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                var credential = GoogleCredential.FromFile("irepairhgc-38cfab6bcfe7.json")//C:/inetpub/wwwroot/iRepairAPI/
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string url = "https://fcm.googleapis.com/v1/projects/irepairhgc/messages:send";

               
                #region send multiple device
                for (int i = 0; i < input.to.Length; i++)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        var message = new
                        {
                            message = new
                            {
                                token = input.to[i],
                                notification = new
                                {
                                    title = input.Title,
                                    body = input.Message
                                }
                            }
                        };

                        var jsonMessage = JsonSerializer.Serialize(message);
                        var httpContent = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(url, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            strReturnValue = "success";
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            strReturnValue = $"Error: {response.StatusCode} - {errorContent}";
                        }
                    }

                }
                #endregion
            }

            return strReturnValue;
        }
        //public async Task<string> SendNotiiRepair(MessageNotiWeb input)
        //{
        //    string strReturnValue = "";
        //    if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
        //    {
        //        // string url = "https://fcm.googleapis.com/v1/projects/iclean-362305/messages:send";
        //        // var request = (HttpWebRequest)WebRequest.Create(url);
        //        // request.Method = "POST";
        //        // request.ContentType = "application/json";
        //        //  request.Headers.Add("Authorization", "Bearer 61480c163aba207d4a491b092cfdfddd9736f36d");// e5e78ca13c1c304c084ede5efb8bce1cba996b7a");
        //        var credential = GoogleCredential.FromFile("C:/inetpub/wwwroot/iRepairAPI/iclean-362305-firebase-adminsdk-whqwb-e5e78ca13c.json")
        //                             .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
        //        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        //        string url = "https://fcm.googleapis.com/v1/projects/iclean-362305/messages:send";

        //        var request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Headers.Add("Authorization", $"Bearer {accessToken}");
        //        //var noti = new FbNoti();
        //        //noti.registration_ids = input.to;
        //        //Notification n = new Notification();
        //        //n.body = input.Message;
        //        //n.title = input.Title;
        //        //noti.notification = n;
        //        var noti = new
        //        {
        //            message = new
        //            {
        //                token = input.to[0],  // Assuming you're sending to a single device token. Adjust if needed.
        //                notification = new
        //                {
        //                    title = input.Title,
        //                    body = input.Message
        //                }
        //            }
        //        };

        //        var json = JsonSerializer.Serialize(noti);
        //        var data = Encoding.UTF8.GetBytes(json);
        //        request.ContentLength = data.Length;

        //        using (var stream = request.GetRequestStream())
        //        {
        //            stream.Write(data, 0, data.Length);
        //        }
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //        strReturnValue = responseString.ToString();
        //    }

        //    return strReturnValue;
        //}
        public async Task<string> SendNotiiCleanT(MessageNotiWeb input)
        {
            string strReturnValue = "";
            if (input != null && !string.IsNullOrEmpty(input.Message) && input.to.Length > 0)
            {
                // Authenticate with the service account credentials to get OAuth token  "GOOGLE_APPLICATION_CREDENTIALS") ?? "C:/inetpub/wwwroot/GCRewardApi/gcreward-d223f8c0f2eb.json";//gcreward-24661c05ef61
                var credential = GoogleCredential.FromFile("C:/inetpub/wwwroot/iRepairAPI/iclean-362305-e9056d3908da.json")
                                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string url = "https://fcm.googleapis.com/v1/projects/iclean-362305/messages:send";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"Bearer {accessToken}");

                // Creating the message according to FCM v1 structure
                var noti = new
                {
                    message = new
                    {
                        token = input.to[0],  // Assuming you're sending to a single device token. Adjust if needed.
                        notification = new
                        {
                            title = input.Title,
                            body = input.Message
                        }
                    }
                };

                // Serialize the object to JSON
                var json = JsonSerializer.Serialize(noti);
                var data = Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                // Write the data to the request stream
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                // Get the response from the server
                try
                {
                    var response = (HttpWebResponse)await request.GetResponseAsync();
                    using (var responseStream = new StreamReader(response.GetResponseStream()))
                    {
                        strReturnValue = await responseStream.ReadToEndAsync();
                    }
                }
                catch (WebException ex)
                {
                    // If there's an error, read the response and log it
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var errorStream = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            strReturnValue = $"Error: {await errorStream.ReadToEndAsync()}";
                        }
                    }
                }
            }

            return strReturnValue;
        }

    }
}
