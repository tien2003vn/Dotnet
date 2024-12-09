using Backend.Controllers;
using Backend.Models;
using Backend.Repositories.Interface;
using Backend.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class MediaService : IService<Media>
	{
		private readonly IUnitOfWork _unit;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public MediaService(IUnitOfWork unit, IHttpContextAccessor httpContextAccessor)
		{
			_unit = unit;
			_httpContextAccessor = httpContextAccessor;
		}

		public Task<bool> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Media>> GetAll()
		{
			throw new NotImplementedException();
		}

		public string GetFullSrc(string value, string? type = "media")
		{
			return $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/{type}/{value}";
		}



		// public async Task<Media> FindProfilePictureByUserId(int UserId)
		// {
		// 	var UserMedia = await _unit.UserMedia.GetByConditionAsync<UserMedia>(u => u.UserId == UserId && u.IsProfilePicture == true);
		// 	if (UserMedia == null) return null;
		// 	var item = await _unit.Media.GetByConditionAsync<Media>(m => m.MediaId == UserMedia.MediaId);
		// 	item.Src = GetFullSrc(item.Src);

		// 	return item;
		// }

		// public async Task<Media> FindCoverPictureByUserId(int UserId)
		// {
		// 	var UserMedia = await _unit.UserMedia.GetByConditionAsync<UserMedia>(u => u.UserId == UserId && u.IsCoverPicture == true);
		// 	if (UserMedia == null) return null;
		// 	return await _unit.Media.GetByConditionAsync<Media>(m => m.MediaId == UserMedia.MediaId);
		// }

		public async Task<IEnumerable<Media>> FindByUserId(int UserId)
		{
			return await _unit.Users.FindAsync<Media>(query => query
						.Where(u => u.UserId == UserId)
						.SelectMany(u => u.Posts.SelectMany(p => p.Medias))
						);
		}

		public async Task<IEnumerable<Media>> FindByMessageId(int MessageId, string? type = "media")
		{
			if (MessageId <= 0) throw new ArgumentException("Mã đoạn chat không hợp lệ");
			try
			{
				var item = await _unit.Message
						.FindAsync(query => query.Where(m => m.MessagesId == MessageId)
						.SelectMany(m => m.ChatInMessages)
						.Where(m => m.MediaId != null)
						.Include(m => m.Media)
						.Where(cm => cm.Media.MediaType == 1 || cm.Media.MediaType == 2)
						.Select(m => m.Media)
						.GroupBy(m => m.MediaId)
						.Select(group => group.First()));

				if (type == "file")
				{
					item = await _unit.Message
						.FindAsync(query => query.Where(m => m.MessagesId == MessageId)
						.SelectMany(m => m.ChatInMessages)
						.Where(m => m.MediaId != null)
						.Include(m => m.Media)
						.Where(cm => cm.Media.MediaType == 3)
						.Select(m => m.Media)
						.GroupBy(m => m.MediaId)
						.Select(group => group.First()));
				}

				foreach (var media in item)
				{
					if (media.MediaType == 3) media.Src = GetFullSrc(media.Src, "file");
					else media.Src = GetFullSrc(media.Src);
				}

				return item;
			}
			catch (System.Exception ex)
			{
				throw new ArgumentException("Lỗi: " + ex);
			}
		}

		public Task<Media> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Media>> GetListById(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<Media> Add(Media value)
		{
			try
			{
				var item = await _unit.Media.AddAsync(value);
				var result = await _unit.CompleteAsync();
				if (result) return item;
				return null;
			}
			catch (System.Exception ex)
			{
				throw new ArgumentException("Lỗi: " + ex);
			}
		}

		public Task<bool> Update(Media value)
		{
			throw new NotImplementedException();
		}

		public async Task<int> IsHas(string hash)
		{
			try
			{
				var item = await _unit.Media.GetByConditionAsync<Media>(query => query.Where(m => m.HashCode == hash));
				if (item == null) return -1;
				return item.MediaId;
			}
			catch (System.Exception ex)
			{
				throw new ArgumentException("Lỗi: " + ex);
			}
		}
	}
}