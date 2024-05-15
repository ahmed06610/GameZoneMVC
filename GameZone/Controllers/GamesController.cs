

using GameZone.Models;
using GameZone.Services;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IDeviceService _deviceService;
        private readonly IGameService _gameService;

        public GamesController(ICategoryService categoryService
            , IDeviceService deviceService
            , IGameService gameService)
        {
            _categoryService = categoryService;
            _deviceService = deviceService;
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            var games = _gameService.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game =_gameService.GetById(id);

            if(game is null)
                return NotFound();

            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewMoel Model = new()
            {
                Categories = _categoryService.GetCategories(),
                Devices= _deviceService.GetDevices(),
            };
            return View(Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewMoel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Categories = _categoryService.GetCategories();
                viewModel.Devices = _deviceService.GetDevices();
                return View(viewModel);
            }

          await _gameService.Create(viewModel);


           return Redirect(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var game = _gameService.GetById(id);

            if (game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new EditGameFormViewModel
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoryService.GetCategories(),
                Devices = _deviceService.GetDevices(),
                CurrentCover=game.Cover,
            };

            return View(viewModel);  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryService.GetCategories();
                viewModel.Devices = _deviceService.GetDevices();
                return View(viewModel);
            }

           var game= await _gameService.Update(viewModel);
            if (game is null)
                return NotFound();


            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted=_gameService.Delete(id);

            return isDeleted? Ok():BadRequest();
        }
    }
}
