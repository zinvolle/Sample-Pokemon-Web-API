using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using webAPI2.DTO;
using webAPI2.Interface;
using webAPI2.Models;
using webAPI2.Repository;

namespace webAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);

        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200)]
        public IActionResult GetCountry(int countryId) 
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]

        public IActionResult GetCountryByOwner (int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (country == null){
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }
        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200)]
        public IActionResult GetOwnersFromCountry (int countryId)
        {
            var owner = _countryRepository.GetOwnersFromCountry(countryId);
            if (owner == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null){
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);
            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
            {
                return BadRequest(ModelState);
            }
            if (countryId != updatedCountry.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var countryMap = _mapper.Map<Country>(updatedCountry);
            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
    }
}
