namespace GameZone.Services
{
    public interface IGameService
    {
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        Task Create(CreateGameFormViewMoel model);
        Task<Game?> Update(EditGameFormViewModel model);
        bool Delete(int id);

    }
}
