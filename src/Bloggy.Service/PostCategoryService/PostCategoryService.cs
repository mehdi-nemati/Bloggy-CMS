using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Service.PostCategoryService
{

    public class PostCategoryService : IScopedDependency, IPostCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<PostCategory> _repository;
        private readonly IRepository<Post> _postRepository;
        public PostCategoryService(IRepository<PostCategory> repository, IMapper mapper, IRepository<Post> postRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public async Task<bool> Submit(PostCategoryInsertDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id == 0)
            {
                await _repository.AddAsync(_mapper.Map<PostCategory>(dto), cancellationToken);
            }
            else
            {
                var findedCat = await _repository.Table.FirstOrDefaultAsync(m => m.Id == dto.Id);
                if (findedCat != null)
                {
                    var entity = _mapper.Map(dto, findedCat);
                    await _repository.UpdateAsync(entity, cancellationToken);
                }
                else { return false; }
            }
            return true;
        }

        public async Task<List<PostCategorySelectDto>> List(CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<PostCategorySelectDto>(_mapper.ConfigurationProvider).OrderByDescending(m => m.Id).ToListAsync(cancellationToken); ;
        }

        public async Task<PostCategoryInsertDto> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<PostCategoryInsertDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
        }

        public async Task<bool> Delete(int Id, CancellationToken cancellationToken)
        {
            var findedCat = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
            if (findedCat != null)
            {
                if(await _postRepository.TableNoTracking.AnyAsync(m=>m.PostCategoryId == Id))
                {
                    return false;
                }
                await _repository.DeleteAsync(findedCat, cancellationToken);
                return true;
            }
            else { return false; }
        }
    }
}
