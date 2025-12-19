using Abp.Application.Services.Dto;

namespace BEZNgCore.Authorization.Users.Dto;

public interface IGetLoginAttemptsInput : ISortedResultRequest
{
    string Filter { get; set; }
}

