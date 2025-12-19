using System.Collections.Generic;
using System.IO;

namespace BEZNgCore.Storage.FileValidator;
public interface IFileValidateInput
{
    string FileName { get; }
    string ContentType { get; }
    long Length { get; }
    Stream OpenReadStream();
}

