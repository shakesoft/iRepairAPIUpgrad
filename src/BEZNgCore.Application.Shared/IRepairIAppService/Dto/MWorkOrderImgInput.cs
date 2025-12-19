using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class MWorkOrderImgInput
    {
        public MWorkOrderImgInput()
        {
            imglst = new HashSet<FWOImage>();
        }
        public MWorkOrderInput wo { get; set; }
        public ICollection<FWOImage> imglst { get; set; }
    }
    public class LostFoundDtoImgInput
    {
        public LostFoundDtoImgInput()
        {
            imglst = new HashSet<FWOImage>();
        }
        public LostFoundDto lf { get; set; }
        public ICollection<FWOImage> imglst { get; set; }
    }
    public class FWOImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
    public class DndImg
    {
        public int Id { get; set; }
        public string RoomKey { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
    public class FWOImageD
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string imageSrc { get; set; }
    }

    public class WOImage
    {
        public WOImage()
        {
            
            Description = "WorkOrder Image";
            DocumentName = "WorkOrder Image";
        }
        public Guid DocumentKey { get; set; }
        public int Sort { get; set; }
       
        public Guid? LastModifiedStaff { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public byte[] Signature { get; set; }
        public Guid? MWorkOrderKey { get; set; }
      
    }

    public class DndImage
    {
        public DndImage()
        {
            DocumentName = "Dnd Image";
        }
        public Guid DndphotoKey { get; set; }
        public Guid RoomKey { get; set; }
        public Guid? LastModifiedStaff { get; set; }
        public int Sort { get; set; }
        public string DocumentName { get; set; }
        public byte[] Signature { get; set; }

    }
    public class LfImage
    {
        public LfImage()
        {

            Description = "LostFound Image";
            DocumentName = "LostFound Image";
        }
        public Guid DocumentKey { get; set; }
        public int Sort { get; set; }

        public Guid? LastModifiedStaff { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public byte[] Signature { get; set; }
        public Guid? LostFoundKey { get; set; }

    }
    public class LostFoundImageDto
    {
        public  Guid LostFoundImageKey { get; set; }
        public int? TenantId { get; set; }
        public Guid? LostFoundKey { get; set; }
        public  string LostFoundImage { get; set; }
        public  DateTime CreatedDate { get; set; }
        public  DateTime? ModifiedDate { get; set; }
        public  Guid? CreatedUser { get; set; }
        public  byte[] LostFoundImages { get; set; }
        public  byte[] LostFoundImages2 { get; set; }
        public  byte[] LostFoundImages3 { get; set; }
        public  byte[] LostFoundImages4 { get; set; }
        public  byte[] LostFoundImages5 { get; set; }
    }
}
