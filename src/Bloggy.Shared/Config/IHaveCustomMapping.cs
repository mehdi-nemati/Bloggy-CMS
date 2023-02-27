using AutoMapper;

namespace Bloggy.Shared.Config
{
    public interface IHaveMapping
    {
        void CreateMappings(Profile profile);
    }
}
