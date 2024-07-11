using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using webAPI2.DTO;
using webAPI2.Interface;
using webAPI2.Models;
using webAPI2.Repository;

namespace webAPI2.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<Pokemon>))]

        public IActionResult getPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpPost]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
            {
                return BadRequest(ModelState);
            }
            var pokemon = _pokemonRepository.GetPokemons().Where(
                p=>p.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "This Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }   
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "An error occurred while creating the pokemon");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }

        [HttpPut("{pokemonId}")]
        public IActionResult UpdateOwner(int pokemonId,[FromQuery] int ownerId, 
            [FromQuery] int catId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
            {
                return BadRequest(ModelState);
            }
            if (pokemonId != updatedPokemon.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);
            if (!_pokemonRepository.UpdatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }

        [HttpDelete("{pokemonId}")]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong with deleting reviews");
                return StatusCode(500, ModelState);
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong with deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted");
        }
    }
}
