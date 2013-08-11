namespace MiniDropbox.Domain.Services
{
    public interface IWriteOnlyRepository
    {
        T Create<T>(T itemToCreate) where T : class, IEntity;
        T Update<T>(T itemToUpdate) where T : class, IEntity;
        void Archive<T>(T itemToArchive); 
    }
}