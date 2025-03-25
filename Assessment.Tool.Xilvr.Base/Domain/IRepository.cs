using Assessment.Tool.Xilvr.Base.Domain.SeedWork;

namespace Assessment.Tool.Xilvr.Base.Domain;

//
// Summary:
//     The repository for Aggrgate roots.
//
// Type parameters:
//   T:
public interface IRepository<T>
{
    //
    // Summary:
    //     Unit of work to represent the current context
    IUnitOfWork UnitOfWork { get; }
}
