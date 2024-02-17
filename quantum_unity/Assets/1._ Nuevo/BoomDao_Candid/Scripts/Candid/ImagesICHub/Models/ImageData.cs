using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.ImagesICHub.Models
{
	public class ImageData
	{
		[CandidName("iType")]
		public string IType { get; set; }

		[CandidName("image")]
		public List<byte> Image { get; set; }

		[CandidName("user")]
		public Principal User { get; set; }

		public ImageData(string iType, List<byte> image, Principal user)
		{
			this.IType = iType;
			this.Image = image;
			this.User = user;
		}

		public ImageData()
		{
		}
	}
}