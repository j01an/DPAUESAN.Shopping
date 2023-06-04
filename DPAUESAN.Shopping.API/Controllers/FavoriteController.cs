using DPAUESAN.Shopping.DOMAIN.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DPAUESAN.Shopping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            var favorites = await _favoriteRepository.GetAll(id);
            return Ok(favorites);
               
        }
    }
}
