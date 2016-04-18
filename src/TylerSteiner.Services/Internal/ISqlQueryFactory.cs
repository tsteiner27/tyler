namespace TylerSteiner.Services.Internal
{
    public interface ISqlQueryFactory
    {
        string CreateParameterizedInsertStatement<T>(string tableName);
    }
}