using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.ProjectsICHub.Models
{
	public class ProjectNews
	{
		[CandidName("content")]
		public string Content { get; set; }

		[CandidName("imageNews")]
		public string ImageNews { get; set; }

		[CandidName("linkButton")]
		public string LinkButton { get; set; }

		[CandidName("newsId")]
		public UnboundedUInt NewsId { get; set; }

		[CandidName("textButton")]
		public string TextButton { get; set; }

		[CandidName("title")]
		public string Title { get; set; }

		public ProjectNews(string content, string imageNews, string linkButton, UnboundedUInt newsId, string textButton, string title)
		{
			this.Content = content;
			this.ImageNews = imageNews;
			this.LinkButton = linkButton;
			this.NewsId = newsId;
			this.TextButton = textButton;
			this.Title = title;
		}

		public ProjectNews()
		{
		}
	}
}