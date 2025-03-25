namespace Assessment.Tool.Xilvr.Base.Domain.SeedWork;

//
// Summary:
//     Unit of work defenitions. Always associated with the db context.
public interface IUnitOfWork : IDisposable
{
    //
    // Summary:
    //     Saving the changes to db for the associated entities in the db context;
    //
    // Parameters:
    //   cancellationToken:
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    //
    // Summary:
    //     Overload method to do custom actions before saving the changes to db
    //
    // Parameters:
    //   cancellationToken:
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
