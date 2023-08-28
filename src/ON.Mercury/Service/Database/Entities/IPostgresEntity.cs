using Microsoft.EntityFrameworkCore;

namespace ON.Mercury.Service.Database;

public interface IPostgresEntity<TProto, out TEntity>
{
    TEntity Clone();
    TEntity FromPb(TProto proto);
    TProto ToPb();
}