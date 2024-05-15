

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly string _imagePath;

        public GameService(ApplicationDbContext context
            , IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
            _imagePath = $"{_webHost.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g=>g.Category)
                .Include(g=>g.Devices)
                .ThenInclude(d=>d.Device)
                .ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games
               .Include(g => g.Category)
               .Include(g => g.Devices)
               .ThenInclude(d => d.Device)
               .SingleOrDefault(g=>g.Id==id);
        }

        public async Task Create(CreateGameFormViewMoel model)
        {
            var coverName = await SaveCover(model.Cover);

            Game game = new Game
            {
                Name=model.Name,
                Cover=coverName,
                Description=model.Description,
                CategoryId=model.CategoryId,
                Devices=model.SelectedDevices.Select(d=>new GameDevice { DeviceId=d}).ToList()
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                .Include(g=>g.Devices)
                .SingleOrDefault(g=>g.Id==model.Id);
            if (game == null)
                return null;

            var hasNewCover=model.Cover != null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices=model.SelectedDevices.Select(d=>new GameDevice { DeviceId=d}).ToList();

            if (hasNewCover)
            {
                game.Cover=await SaveCover(model.Cover!);
            }
           var effectedRows= _context.SaveChanges();
            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagePath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagePath, game.Cover);
                File.Delete(cover);

                return null;
            }
        }
        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Games.Remove(game);
            var effectedRows=_context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover=Path.Combine(_imagePath, game.Cover);
                File.Delete(cover);
            }
            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagePath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }

       
    }
}
