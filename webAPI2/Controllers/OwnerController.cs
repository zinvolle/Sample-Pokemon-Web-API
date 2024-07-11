using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using webAPI2.DTO;
using webAPI2.Interface;
using webAPI2.Models;

namespace webAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOwners()
        {
            var owners = _ownerRepository.GetOwners();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }
        [HttpGet("{ownerId}")]
        public IActionResult GetOwner(int ownerId)
        {
            var owner = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpGet("pokemon/{pokemonId}")]
        public IActionResult GetOwnerByPokemon(int pokemonId)
        {
            var owners = _ownerRepository.GetOwnerOfPokemon(pokemonId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("pokemon/owner/{ownerId}")]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            var pokemons = _ownerRepository.GetPokemonByOwner(ownerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [HttpPost]
        public IActionResult CreateOwner([FromQuery] int countryId,[FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var owner = _ownerRepository.GetOwners().Where(o => o.Name.Trim().ToUpper() == ownerCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{ownerId}")]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
            {
                return BadRequest(ModelState);
            }
            if (ownerId != updatedOwner.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(updatedOwner);
            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
    }
}
