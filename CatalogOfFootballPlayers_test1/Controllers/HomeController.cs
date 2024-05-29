using CatalogOfFootballPlayers_test1.Data;
using CatalogOfFootballPlayers_test1.Hubs;
using CatalogOfFootballPlayers_test1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CatalogOfFootballPlayers_test1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<FootballersHub> _footballersHubContext;
        private readonly CatalogOfFootballPlayersContext _catalogOfFootballPlayersContext;

        public HomeController(IHubContext<FootballersHub> hubContext, CatalogOfFootballPlayersContext catalogOfFootballPlayersContext)
        {
            _footballersHubContext = hubContext;
            _catalogOfFootballPlayersContext = catalogOfFootballPlayersContext;
        }

        [HttpGet]
        public async Task<IActionResult> AddFootballer()
        {
            ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFootballer(Footballer footballer)
        {
            if (ModelState.IsValid)
            {
                string? selectedTeamName = footballer?.Team?.Name;

                Team? selectedTeam = await _catalogOfFootballPlayersContext.Teams.FirstOrDefaultAsync(t => t.Name == selectedTeamName);

                if (selectedTeam == null)
                {
                    ModelState.AddModelError("Team.Name", "Выбранная команда не найдена.");
                    ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
                    return View(footballer);
                }

                footballer.Team = selectedTeam;

                await _catalogOfFootballPlayersContext.Footballers.AddAsync(footballer);
                await _catalogOfFootballPlayersContext.SaveChangesAsync();

                await _footballersHubContext.Clients.All.SendAsync("ReceivefootballersUpdate", footballer);

                return RedirectToAction("Index");
            }

            ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
            return View(footballer);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var footballers = await _catalogOfFootballPlayersContext.Footballers.Include(f => f.Team).ToListAsync();
            return View(footballers);
        }

        [HttpGet]
        public async Task<IActionResult> EditFootballer(int id)
        {
            var footballer = await _catalogOfFootballPlayersContext.Footballers.Include(f => f.Team).FirstOrDefaultAsync(f => f.Id == id);
            ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
            return View(footballer);
        }

        [HttpPost]
        public async Task<IActionResult> EditFootballer(Footballer footballer)
        {
            if (ModelState.IsValid)
            {
                string? selectedTeamName = footballer?.Team?.Name;

                Team? selectedTeam = await _catalogOfFootballPlayersContext.Teams.FirstOrDefaultAsync(t => t.Name == selectedTeamName);

                if (selectedTeam == null)
                {
                    ModelState.AddModelError("Team.Name", "Выбранная команда не найдена.");
                    ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
                    return View(footballer);
                }

                footballer.Team = selectedTeam;

                _catalogOfFootballPlayersContext.Footballers.Update(footballer);
                await _catalogOfFootballPlayersContext.SaveChangesAsync();

                await _footballersHubContext.Clients.All.SendAsync("ReceivefootballersUpdate", footballer);

                return RedirectToAction("Index");
            }

            ViewData["Teams"] = await _catalogOfFootballPlayersContext.Teams.ToListAsync();
            return View(footballer);
        }

        [HttpGet]
        public async Task<List<Team>> GetTeams()
        {
            return await _catalogOfFootballPlayersContext.Teams.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam([FromBody] Team newTeam)
        {
            if (newTeam == null || string.IsNullOrEmpty(newTeam.Name.Trim()))
            {
                return BadRequest("Неверные данные о команде");
            }

            var existingTeam = await _catalogOfFootballPlayersContext.Teams.FirstOrDefaultAsync(t => t.Id == newTeam.Id);
            if (existingTeam != null)
            {
                return BadRequest("Команда уже существует");
            }

            await _catalogOfFootballPlayersContext.Teams.AddAsync(newTeam);
            await _catalogOfFootballPlayersContext.SaveChangesAsync();

            return CreatedAtAction("GetTeams", newTeam);
        }
    }
}
