using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Service.UploadService;
using Bloggy.Shared;
using Bloggy.Shared.Config;
using Bloggy.Shared.Extension;
using Bloggy.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Bloggy.Service.PostService
{
    public class PostService : IScopedDependency, IPostService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Post> _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUploadService _uploadService;
        public PostService(IRepository<Post> repository, IMapper mapper, IStringLocalizer<SharedResources> localizer, IUploadService uploadService)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _uploadService = uploadService;
        }
        public async Task<ResponseModel> Submit(PostInsertDto dto, CancellationToken cancellationToken)
        {
            if (!dto.CoverImageFile.IsImage())
                return new ResponseModel(false, _localizer["file-type-is-not-valid"]);

            if (dto.CoverImageFile != null)
            {
                var url = await _uploadService.UploadFile(dto.CoverImageFile, "postCoverImage");
                dto.CoverImage = url;
            }

            if (await CheckSlugIsDuplicated(dto.Slug, dto.Id))
                return new ResponseModel(false, _localizer["slug-duplicated"]);

            if (dto.Id == 0)
            {
                dto.IsPublished = true;
                await _repository.AddAsync(_mapper.Map<Post>(dto), cancellationToken);
            }
            else
            {
                var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == dto.Id, cancellationToken: cancellationToken);
                if (findedItem != null)
                {
                    var entity = _mapper.Map(dto, findedItem);
                    await _repository.UpdateAsync(entity, cancellationToken);
                }
                else return new ResponseModel(false, _localizer["not-found"]);
            }
            return new ResponseModel(true);
        }

        public async Task<bool> CheckSlugIsDuplicated(string Slug, int Id)
        {
            var query = _repository.TableNoTracking.Where(m => m.Slug == Slug);
            if (Id != 0)
                query = query.Where(m => m.Id != Id);
            return await query.AnyAsync();
        }

        public async Task<int> GetTotalPostCount(int CategoryId = 0, bool? IsPublished = null, string SearchString = "")
        {
            return await GetPostsQueriable(CategoryId, IsPublished, SearchString).CountAsync();
        }

        public async Task<List<PostListSelectDto>> List(Pageres arg, CancellationToken cancellationToken, int CategoryId = 0, bool? IsPublished = null, string SearchString = "")
        {
            return await GetPostsQueriable(CategoryId, IsPublished, SearchString).OrderByDescending(m => m.Id).Paginate(arg).ProjectTo<PostListSelectDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }

        public async Task<PostInsertDto> GetByIdForEdit(int Id, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<PostInsertDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => (m.Id == Id), cancellationToken);
        }

        public async Task<PostSelectDto> GetBySlug(string Slug, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<PostSelectDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => m.IsPublished == true && m.Slug == Slug, cancellationToken);
        }

        public async Task<bool> Delete(int Id, CancellationToken cancellationToken)
        {
            var findedCat = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
            if (findedCat != null)
            {
                DeleteCoverImageFromStorage(findedCat.CoverImage);

                await _repository.DeleteAsync(findedCat, cancellationToken);
                return true;
            }
            else { return false; }
        }

        public async Task RemovePicture(int Id, CancellationToken cancellationToken)
        {
            var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken: cancellationToken);
            if (findedItem != null && !string.IsNullOrEmpty(findedItem.CoverImage))
            {
                DeleteCoverImageFromStorage(findedItem.CoverImage);

                findedItem.CoverImage = null;
                await _repository.UpdateAsync(findedItem, cancellationToken, true);
            }
        }

        public void DeleteCoverImageFromStorage(string CoverImage)
        {
            if (string.IsNullOrEmpty(CoverImage)) return;
            _uploadService.DeleteFileFromStorage(CoverImage);
        }

        private IQueryable<Post> GetPostsQueriable(int CategoryId = 0, bool? IsPublished = null, string SearchString = "")
        {
            var Posts = _repository.TableNoTracking;
            if (CategoryId != 0)
                Posts = Posts.Where(m => m.PostCategoryId == CategoryId);
            if (IsPublished != null)
                Posts = Posts.Where(m => m.IsPublished == IsPublished);
            if (!string.IsNullOrEmpty(SearchString))
                Posts = Posts.Where(m => m.Title.ToLower().Contains(SearchString.ToLower()));
            return Posts;
        }
    }
}
